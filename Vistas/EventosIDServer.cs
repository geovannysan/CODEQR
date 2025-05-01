using ClosedXML.Excel;
using Entity;
using Google.Protobuf.WellKnownTypes;
using IronBarCode;
using MaterialSkin.Controls;
using Microsoft.Extensions.Logging;
using NEWCODES.Aplicacion.DTO;
using NEWCODES.Infraestructura.Persistencia;
using NEWCODES.Infraestructura.Utils;
using NEWCODES.Vistas.Codigos;
using NEWCODES.Vistas.Componetes;
using NEWCODES.Vistas.Despositivo;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace NEWCODES.Vistas
{
    public partial class EventosIDServer : MaterialForm
    {
        private int _ID;
        private HttpListener httpListener;
        private Thread listenerThread;
        private bool isRunning = false;
        string ip;
        string puerto;
        moduleExcel excelImp = new moduleExcel();
        public EventosIDServer(int id)
        {
            InitializeComponent();
            _ID = id;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            Dispositivos.TabPages[0].Text = "SCANEO";
            Dispositivos.TabPages[1].Text = "DISPOCITIVOS";
            Dispositivos.TabPages[2].Text = "LOCALIDAD";
            ipserver.Text = GetLocalIPAddress();
            ip = GetLocalIPAddress();
            puerto = puertoTxt.Text;


            //Dispositivos.TabPages[2].Text = "ESTADO";
        }

        private void EventosIDServer_Load(object sender, EventArgs e)
        {

            try
            {
                if (datagridCliente.Columns["Accion"] == null)
                {
                    DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
                    btnEliminar.Name = "Eliminar";
                    btnEliminar.HeaderText = "Eliminar";
                    btnEliminar.Text = "Eliminar";
                    btnEliminar.UseColumnTextForButtonValue = true;
                    datagridCliente.Columns.Add(btnEliminar);
                }
                if (dataDispositi.Columns["Accion"] == null)
                {
                    DataGridViewButtonColumn btnElimniar = new DataGridViewButtonColumn();
                    DataGridViewButtonColumn btnPermitir = new DataGridViewButtonColumn();
                    btnElimniar.Name = "EliminarDis";
                    btnElimniar.HeaderText = "Eliminar";
                    btnElimniar.Text = "Quitar";
                    btnElimniar.UseColumnTextForButtonValue = true;
                    btnPermitir.Name = "PermitirDisp";
                    btnPermitir.HeaderText = "Permisos";
                    btnPermitir.Text = "Agg/Rem";
                    btnPermitir.UseColumnTextForButtonValue = true;
                    dataDispositi.Columns.Add(btnElimniar);
                    dataDispositi.Columns.Add(btnPermitir);
                }

                Eventosinfo();

                //Add Barcode value below the generated barcode
                var qrcode = QRCodeWriter.CreateQrCode("absolute");
                qrcode.AddBarcodeValueTextBelowBarcode();
                qrcode.ToJpegBinaryData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Erorr", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private Dictionary<string, WebSocket> connectedClients = new Dictionary<string, WebSocket>();

        private async void StartWebSocketServer()
        {
            try
            {
                if (button1.Text == "PARAR")
                {
                    this.Close();
                    return;
                }

                string puertoTexto = puertoTxt.Text.Trim();

                // Validación del puerto
                if (!int.TryParse(puertoTexto, out int puerto) || puertoTexto.Length < 4)
                {
                    MessageBox.Show("Ingrese un número de puerto válido de mínimo 4 dígitos.", "Error de puerto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (httpListener != null && httpListener.IsListening)
                {
                    AppendLog("⚠️ El servidor ya está en ejecución.");

                    return;
                }

                if (IsPortInUse(puerto))
                {
                    AppendLog($"❌ El puerto {puerto} ya está en uso. Intenta con otro.");
                    MessageBox.Show($"El puerto {puerto} ya está en uso. Intenta con otro.", "Puerto ocupado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Obtener IP local válida
                string ip = GetLocalIPAddress();
                // string prefijo = $"http://+:{puerto}/ws/";

                string prefijo = $"http://127.0.0.1:{puerto}/ws/";


                if (!httpListener.Prefixes.Contains(prefijo))
                {
                    httpListener.Prefixes.Add(prefijo);
                }

                httpListener.Start();
                AppendLog($"✅ Servidor WebSocket iniciado en http://{GetLocalIPAddress()}:{puerto}/ws/");

                listenerThread = new Thread(ListenForWebSocketConnections);
                listenerThread.IsBackground = true;
                listenerThread.Start();
                puertoTxt.Visible = false;
                label1.Visible = false;
                ipserver.Text = GetLocalIPAddress() + ":" + puerto;
                httpListener = new HttpListener();
                pictureBox1.Visible = true;
                button1.Text = "PARAR";
                button1.BackColor = Color.Red;
                button1.ForeColor = Color.White;
            }
            catch (Exception ex)
            {
                //httpListener = new HttpListener();
                // button1.Text = "INICIAR";
                AppendLog($"❌ Error al iniciar el servidor: {ex.Message}");
                MessageBox.Show($"Error al iniciar el servido:\n Verifiqur si ejecuto como administrador", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
         * Verificacion de puerto en usao 
         * */
        private bool IsPortInUse(int port)
        {
            try
            {

                TcpListener tcpListener = new TcpListener(IPAddress.Loopback, port);
                tcpListener.Start();
                tcpListener.Stop();
                return false;
            }
            catch (SocketException)
            {
                return true;
            }
        }

        /* escucha Conexion Socket
         *
         */
        private async void ListenForWebSocketConnections()
        {
            while (httpListener.IsListening)
            {
                try
                {
                    DispoditivosRepsoitory dispoditivos = new DispoditivosRepsoitory();
                    var context = await httpListener.GetContextAsync();
                    if (context.Request.IsWebSocketRequest)
                    {

                        string clientId = context.Request.QueryString["clientId"] ?? "";
                        var consulta = dispoditivos.GetUnic(clientId, _ID);
                        if (consulta != null)
                        {
                            WebSocket webSocket = (await context.AcceptWebSocketAsync(null)).WebSocket;
                            connectedClients[clientId] = webSocket;
                            AppendLog("✅ Conexión WebSocket aceptada.");
                            var desonectado = dispoditivos.Update(new Dispositivos
                            {
                                Id = consulta.Id,
                                Name = consulta.Name,
                                IDequipo = consulta.IDequipo,
                                Ip = consulta.Ip,
                                Estado = "Conectado",
                                EventoID = consulta.EventoID,
                            });


                            connectedClients[clientId] = webSocket;
                            DispoditivosRepsoitory dispod = new DispoditivosRepsoitory();
                            //  DispoditivosRepsoitory dispoditiv = new DispoditivosRepsoitory();
                            var disp = dispod.Getlist(_ID);
                            this.Invoke(new Action(() =>
                            {
                                dataDispositi.Rows.Clear();

                                foreach (var device in disp)
                                {
                                    dataDispositi.Rows.Add(device.Id, device.Name, device.IDequipo, device.Estado);
                                }

                                CargarClientesEnGrid();
                            }));
                            await HandleWebSocketMessages(consulta, webSocket);
                        }
                        else
                        {
                            string remoteIP = context.Request.RemoteEndPoint?.ToString() ?? "Desconocido";
                            bool aceptar = false;
                            using (var form = new FormAprobacion(clientId))
                            {
                                form.ShowDialog();
                                aceptar = form.Aprobado;
                            }
                            /* Invoke(new Action(() =>
                             {
                                 var result = MessageBox.Show($"Nueva conexión entrante desde {remoteIP} {clientId}. ¿Aceptar?", "Aprobar conexión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                 aceptar = (result == DialogResult.Yes);
                             }));*/

                            if (aceptar)
                            {
                                var registro = dispoditivos.Insert(new Dispositivos
                                {
                                    Name = clientId,
                                    IDequipo = clientId,
                                    Ip = remoteIP,
                                    Estado = "conectado",
                                    EventoID = _ID,
                                }, _ID);
                                WebSocket webSocket = (await context.AcceptWebSocketAsync(null)).WebSocket;
                                connectedClients[clientId] = webSocket;


                                AppendLog("✅ Conexión WebSocket aceptada.");
                                await HandleWebSocketMessages(registro, webSocket);
                                DispoditivosRepsoitory dispoditiv = new DispoditivosRepsoitory();
                                var disp = dispoditivos.Getlist(_ID);
                                this.Invoke(new Action(() =>
                                {
                                    dataDispositi.Rows.Clear();

                                    foreach (var device in disp)
                                    {
                                        dataDispositi.Rows.Add(device.Id, device.Name, device.IDequipo, device.Estado);
                                    }
                                    CargarClientesEnGrid();
                                }));
                            }
                            else
                            {
                                AppendLog("❌ Conexión rechazada.");
                                context.Response.StatusCode = 403;
                                context.Response.Close();
                            }
                        }
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

        /**
         * Escucha Mensaje del socket
         */
        private async Task HandleWebSocketMessages(Dispositivos clientId, WebSocket webSocket)
        {
            byte[] buffer = new byte[1024];
            WebSocketReceiveResult result = null;

            try
            {
                DispoditivosRepsoitory dispoditivo = new DispoditivosRepsoitory();
                while (webSocket.State == WebSocketState.Open)
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        // Responder
                        string response = "Mensaje recibido: " + message;
                        var datosrecibidos = JsonSerializer.Deserialize<MessageSocket>(message);
                        Console.WriteLine(datosrecibidos);

                        AppendLog($"📩 [{clientId.IDequipo}] → {datosrecibidos.Type}");

                        var verifica = VerificaCodigo(Convert.ToString(clientId.Id), datosrecibidos);

                        Console.Write(verifica);
                        byte[] responseBuffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(verifica));

                        await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                        this.Invoke(new Action(() =>
                        {
                            Eventosinfo();

                        }));

                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {

                        var dispo = new Dispositivos
                        {
                            Id = clientId.Id,
                            Name = clientId.Name,
                            IDequipo = clientId.IDequipo,
                            Ip = clientId.Ip,
                            Estado = "Desconectado",
                            EventoID = clientId.EventoID,
                        };

                        var desonectado = dispoditivo.Update(dispo);
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Desconectado", CancellationToken.None);

                        var disp = dispoditivo.Getlist(_ID);
                        this.Invoke(new Action(() =>
                        {
                            dataDispositi.Rows.Clear();

                            foreach (var device in disp)
                            {
                                dataDispositi.Rows.Add(device.Id, device.Name, device.IDequipo, device.Estado);
                            }
                            CargarClientesEnGrid();
                        }));

                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog($"⚠️ Error con cliente {clientId}: {ex.Message}");
            }
            finally
            {
                // Limpieza al desconectar
                if (connectedClients.ContainsKey(clientId.IDequipo))
                {
                    connectedClients.Remove(clientId.IDequipo);
                }

            }
        }
        private MessageSocket VerificaCodigo(string cliente, MessageSocket message)
        {
            CodigosRepository codigos = new CodigosRepository();

            var vrc = codigos.Verifica(cliente, Convert.ToString(_ID), message);

            return vrc;

        }

        private void Eventosinfo()
        {
            EventosRepository eventosRepository = new EventosRepository();
            var dato = eventosRepository.Get(Convert.ToString(_ID));
            txtEvento.Text = dato.Nombre;
            DispoditivosRepsoitory dispoditivos = new DispoditivosRepsoitory();
            LocalidadesRepository localidades = new LocalidadesRepository();
            CodigosRepository codigos = new CodigosRepository();
            LogsEvenRepository LogsEven = new LogsEvenRepository();
            var dispo = dispoditivos.Getlist(_ID);
            dataDispositi.Rows.Clear();
            datagridLocalidad.Rows.Clear();
            foreach (var device in dispo)
            {
                dataDispositi.Rows.Add(device.Id, device.Name, device.IDequipo, device.Estado);
            }
            var localidad = localidades.GetInfo(Convert.ToString(_ID));
            Console.WriteLine(localidad);
            foreach (var device in localidad)
            {
                datagridLocalidad.Rows.Add(device.Name, device.Count);
            }

            var codi = codigos.GetList(Convert.ToString(_ID)).Select(x => new { x.Name, x.Codigo, x.Precio, x.info, x.Estado, x.time }).ToList();
            var logs = LogsEven.ObtenerLista(_ID);
            txtValidos.Text = codi.Where(x => x.Estado == "Scaneado").ToList().Count().ToString();
            txtRechazados.Text = logs.Where(x => x.Estado.Equals("Rechazado")).ToList().Count().ToString();
            txtRepetidos.Text = logs.Where(x => x.Estado.Equals("Repetido")).ToList().Count().ToString();
            datagridcoidgos.DataSource = null;
            datagridcoidgos.DataSource = codi;
        }
        /** Logs de socket
         * 
         */
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
            try
            {
                // Cancelar el cierre si querés evitarlo (por ejemplo, desactivar la X)
                 if (puerto=="")
                {
                    e.Cancel = false;
                    return;
                }
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
            catch (Exception ex)
            {
                e.Cancel = false;
                isRunning = false;
            }
        }


        public string GetLocalIPAddress()
        {
            string localIP = string.Empty;
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Verificamos si la interfaz está activa y conectada
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    var ipProperties = networkInterface.GetIPProperties();
                    foreach (var ipAddress in ipProperties.UnicastAddresses)
                    {
                        // Filtramos por la dirección IPv4
                        if (ipAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            localIP = ipAddress.Address.ToString();
                            return localIP;
                        }
                    }
                }
            }
            return localIP; // En caso de no encontrar la IP, regresamos vacío
            // MessageBox.Show("Tú IP Local Es: " + localIP);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void CargarClientesEnGrid()
        {
            datagridCliente.DataSource = null;
            List<ClienteSocket> lista = new List<ClienteSocket>();

            foreach (var kvp in connectedClients)
            {
                var id = kvp.Key;
                var estado = kvp.Value.State.ToString();

                lista.Add(new ClienteSocket
                {
                    Dispositivo = id,
                    Estado = estado == "Closed" ? "Desconectado" : "Conectado"
                });
            }

            datagridCliente.DataSource = lista;
            var dat = lista.Where(x => x.Estado == "Conectado").ToList();
            scanerconetado.Text = dat.Count.ToString();
            CodigosRepository codigos = new CodigosRepository();
            LogsEvenRepository LogsEven = new LogsEvenRepository();
            var codi = codigos.GetList(Convert.ToString(_ID)).Select(x => new { x.Name, x.Codigo, x.Precio, x.info, x.Estado, x.time }).ToList();
            var logs = LogsEven.ObtenerLista(_ID);
            txtValidos.Text = codi.Where(x => x.Estado == "Scaneado").ToList().Count().ToString();
            txtRechazados.Text = logs.Where(x => x.Estado.Equals("Rechazado")).ToList().Count().ToString();
            txtRepetidos.Text = logs.Where(x => x.Estado.Equals("Repetido")).ToList().Count().ToString();

        }

        private void datagridCliente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var column = datagridCliente.Columns[e.ColumnIndex];

                if (column.Name == "Eliminar")
                {
                    var id = datagridCliente.Rows[e.RowIndex].Cells["Dispositivo"].Value?.ToString();
                    DialogResult result = MessageBox.Show("¿Estás seguro que deseas Desconectar?", "Confirmar", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.OK)
                    {
                        DesconectarClienteAsync(id);
                        MessageBox.Show("Registro eliminado.");
                    }
                }
            }
        }
        public async Task DesconectarClienteAsync(string clientId)
        {
            if (connectedClients.TryGetValue(clientId, out WebSocket socket))
            {
                if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
                {
                    await socket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Desconexión solicitada",
                        CancellationToken.None
                    );
                }
                connectedClients.Remove(clientId);
            }
        }
        private void ExportarCodigos_Click(object sender, EventArgs e)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Datos");
                for (int i = 0; i < datagridcoidgos.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = datagridcoidgos.Columns[i].HeaderText;
                }
                for (int i = 0; i < datagridcoidgos.Rows.Count; i++)
                {
                    for (int j = 0; j < datagridcoidgos.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = datagridcoidgos.Rows[i].Cells[j].Value.ToString();
                    }
                }
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Guardar como Excel";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("¡Exportado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
        }

        private void dataDispositi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var column = dataDispositi.Columns[e.ColumnIndex];
                var idequipo = dataDispositi.Rows[e.RowIndex].Cells["Equipogrid"].Value?.ToString();
                var id = dataDispositi.Rows[e.RowIndex].Cells["IDDispo"].Value?.ToString();
                if (column.Name == "EliminarDis")
                {

                    DialogResult result = MessageBox.Show("¿Estás seguro que deseas Eliminar este Dispositivo?", "Confirmar", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.OK)
                    {
                        DispoditivosRepsoitory dispoditivos = new DispoditivosRepsoitory();
                        dispoditivos.Delete(id);
                        // Console.WriteLine(id,column);
                        DesconectarClienteAsync(idequipo);
                        dataDispositi.Rows.RemoveAt(e.RowIndex);
                        MessageBox.Show("Registro eliminado.");
                    }
                }
                if (column.Name == "PermitirDisp")
                {
                    // Obtener el ID de la fila
                    DIspositivosF dIspositivos = new DIspositivosF(_ID, int.Parse(id));
                    dIspositivos.ShowDialog();
                }

            }

        }

        private void materialFloatingActionButton1_Click(object sender, EventArgs e)
        {
            var qrcodetxt = JsonSerializer.Serialize(new { ip = GetLocalIPAddress(), puertos = puerto });

            //Add Barcode value below the generated barcode
            //var qrcode = QRCodeWriter.CreateQrCode(qrcodetxt);
            //qrcode.AddBarcodeValueTextBelowBarcode();
            //qrcode.ToJpegBinaryData();
            Qrcode qrcode1 = new Qrcode(qrcodetxt);
            qrcode1.ShowDialog();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var qrcodetxt = JsonSerializer.Serialize(new { ip = GetLocalIPAddress(), puertos = puerto });

            //Add Barcode value below the generated barcode
            //var qrcode = QRCodeWriter.CreateQrCode(qrcodetxt);
            //qrcode.AddBarcodeValueTextBelowBarcode();
            //qrcode.ToJpegBinaryData();
            Qrcode qrcode1 = new Qrcode(qrcodetxt);
            qrcode1.ShowDialog();
        }
    }
}
