using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEWCODES.Vistas
{
    public partial class EventosIDServer : Form
    {
        private int _ID;
        private HttpListener httpListener;
        private Thread listenerThread;
        private bool isRunning = false;
        public EventosIDServer(int id)
        {
            InitializeComponent();
            _ID = id;
        }

        private void EventosIDServer_Load(object sender, EventArgs e)
        {

        }
        private async void StartWebSocketServer()
        {
            if (httpListener != null && httpListener.IsListening)
            {
                AppendLog("Servidor ya está en ejecución.");
                return;
            }

            httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:8080/ws/");
            httpListener.Start();

            AppendLog("Servidor WebSocket iniciado en ws://localhost:8080/ws/");

            listenerThread = new Thread(ListenForWebSocketConnections);
            listenerThread.IsBackground = true;
            listenerThread.Start();
        }

        private async void ListenForWebSocketConnections()
        {
            while (httpListener.IsListening)
            {
                try
                {
                    var context = await httpListener.GetContextAsync();
                    if (context.Request.IsWebSocketRequest)
                    {
                        string clientId = context.Request.QueryString["clientId"];
                        string remoteIP = context.Request.RemoteEndPoint?.ToString() ?? "Desconocido";

                        // Mostrar ventana de confirmación en el hilo UI
                        bool aceptar = false;
                        Invoke(new Action(() =>
                        {
                            var result = MessageBox.Show($"Nueva conexión entrante desde {remoteIP} {clientId}. ¿Aceptar?", "Aprobar conexión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            aceptar = (result == DialogResult.Yes);
                        }));

                        if (aceptar)
                        {
                            WebSocket webSocket = (await context.AcceptWebSocketAsync(null)).WebSocket;
                            AppendLog("✅ Conexión WebSocket aceptada.");
                            await HandleWebSocketMessages(webSocket);
                        }
                        else
                        {
                            AppendLog("❌ Conexión rechazada.");
                            context.Response.StatusCode = 403;
                            context.Response.Close();
                        }
                        //WebSocket webSocket = (await context.AcceptWebSocketAsync(null)).WebSocket;
                        //  AppendLog("Conexión WebSocket aceptada.");

                        // Leer mensajes del cliente
                        // await HandleWebSocketMessages(webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                    }
                }
                catch (Exception ex)
                {
                    AppendLog($"Error en la conexión WebSocket: {ex.Message}");
                }
            }
        }

        private async System.Threading.Tasks.Task HandleWebSocketMessages(WebSocket webSocket)
        {
            byte[] buffer = new byte[1024];
            WebSocketReceiveResult result = null;

            while (webSocket.State == WebSocketState.Open)
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    AppendLog($"Mensaje recibido: {message}");

                    // Responder al cliente
                    string responseMessage = "Mensaje recibido: " + message;
                    byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                    await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }

            webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Cerrando", CancellationToken.None);
        }

        private void AppendLog(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action<string>(AppendLog), message);
            }
            else
            {
                txtLog.AppendText(message + Environment.NewLine);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StartWebSocketServer();
        }
        private void EventosIDServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
            httpListener?.Stop();
            listenerThread?.Abort();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Cancelar el cierre si querés evitarlo (por ejemplo, desactivar la X)
            e.Cancel = true;
            DialogResult result = MessageBox.Show("¿Estás seguro que deseas cerrar el Programa?", "INFO", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                e.Cancel = false;
                isRunning = false;
                httpListener?.Stop();
               // listenerThread?.Abort();
            }
        }
    }
}
