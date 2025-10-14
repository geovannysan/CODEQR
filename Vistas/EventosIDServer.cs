using ClosedXML.Excel;
using Entity;
using MaterialSkin.Controls;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using NEWCODES.Aplicacion.DTO;
using NEWCODES.Infraestructura.Persistencia;
using NEWCODES.Infraestructura.Utils;
using NEWCODES.Vistas.Codigos;
using NEWCODES.Vistas.Componetes;
using NEWCODES.Vistas.Despositivo;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;

namespace NEWCODES.Vistas
{
    public partial class EventosIDServer : MaterialSkin.Controls.MaterialForm
    {

        private readonly ConcurrentQueue<MessageSocket> _colaMensajes = new ConcurrentQueue<MessageSocket>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        private int _ID;
        private HttpListener httpListener;
        private Thread listenerThread;
        private bool isRunning = false;
        string ip;
        string puerto;
        moduleExcel excelImp = new moduleExcel();
        public EventosIDServer(int id)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
            _ID = id;
            Dispositivos.TabPages[0].Text = "SCANEO";
            Dispositivos.TabPages[1].Text = "DISPOCITIVOS";
            Dispositivos.TabPages[2].Text = "ESTADOS";
            Dispositivos.TabPages[3].Text = "LOCALIDAD";
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
                dataDispositi.CellEndEdit += dataDispositi_CellEndEdit;
                Eventosinfo();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Erorr", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataDispositi_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dgv = sender as DataGridView;
                var fila = dgv.Rows[e.RowIndex];
                var columna = dgv.Columns[e.ColumnIndex].Name;
                //var identifica = dgv.Columns[e.ColumnIndex;
                // Obtener el nuevo valor
                var nuevoValor = fila.Cells[e.ColumnIndex].Value?.ToString();
                var indes = fila.Cells[0].Value?.ToString() ?? string.Empty;
                // Ejemplo: imprimir o guardar
                if (nuevoValor == string.Empty) return;
                DispoditivosRepsoitory dispoditivos = new DispoditivosRepsoitory();
                var nuevo = new Dispositivos
                {
                    Id = int.Parse(indes),
                    Name = nuevoValor,
                    IDequipo = fila.Cells[2].Value?.ToString(),
                    Estado = fila.Cells[3].Value?.ToString(),
                    EventoID = _ID
                };
                dispoditivos.Update(nuevo);
                var disp = dispoditivos.Getlist(_ID);
                this.Invoke(new Action(() =>
                {
                    dataDispositi.Rows.Clear();
                    foreach (var device in disp)
                        dataDispositi.Rows.Add(device.Id, device.Name, device.IDequipo, device.Estado);
                    CargarClientesEnGrid();
                }));
            }
            catch (Exception ex)
            {

            }
            //MessageBox.Show(indes + "Erorr", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void StartWebSocketServer()
        {
            try
            {
                string puertoTexto = puertoTxt.Text.Trim();
                if (string.IsNullOrEmpty(puertoTxt.Text)) return;
                if (button1.Text == "PARAR")
                {
                    puertoTxt.Visible = true;
                    label1.Visible = true;
                    //   ipserver.Text = GetLocalIPAddress() + ":" + puerto;
                    pictureBox1.Visible = false;
                    httpListener?.Stop();

                    button1.Text = "INICIAR";
                    button1.BackColor = Color.White;
                    button1.ForeColor = Color.Black;
                    isRunning = false;
                    _ = Task.Run(() => EliminarPuertoFirewall(Convert.ToInt32(puertoTexto)));
                    // this.Close();                
                    return;
                }

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

                string ip = GetLocalIPAddress();

                //string prefijo = $"http://127.0.0.1:{puerto}/ws/";

                httpListener = new HttpListener();

                string prefijo = $"http://+:{puerto}/ws/";

                if (!httpListener.Prefixes.Contains(prefijo))
                {
                    httpListener.Prefixes.Add(prefijo);
                }
                _ = Task.Run(() => AbrirPuertoFirewall(Convert.ToInt32(puertoTexto)));
                //_ = Task.Run(=>)  AbrirPuertoFirewall(Convert.ToInt32(puerto));

                httpListener.Start();
                AppendLog($"✅ Servidor WebSocket iniciado en http://{GetLocalIPAddress()}:{puerto}/ws/");

                listenerThread = new Thread(ListenForWebSocketConnections);
                listenerThread.IsBackground = true;
                listenerThread.Start();
                puertoTxt.Visible = false;
                label1.Visible = false;
                ipserver.Text = GetLocalIPAddress() + ":" + puerto;
                pictureBox1.Visible = true;
                button1.Text = "PARAR";
                isRunning = true;
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

        private Dictionary<string, List<WebSocket>> connectedClients = new Dictionary<string, List<WebSocket>>();
        private async void ListenForWebSocketConnections()
        {
            while (httpListener.IsListening)
            {
                try
                {
                    DispoditivosRepsoitory dispoditivos = new DispoditivosRepsoitory();
                    var context = await httpListener.GetContextAsync();

                    if (!context.Request.IsWebSocketRequest)
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                        continue;
                    }

                    string clientId = context.Request.QueryString["clientId"]?.Trim('"') ?? "";
                    string modelo = context.Request.QueryString["name"]?.Trim('"') ?? "";
                    if (string.IsNullOrWhiteSpace(clientId))
                    {
                        AppendLog("❌ Conexión sin clientId.");
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                        continue;
                    }

                    var consulta = dispoditivos.GetUnic(clientId, _ID);

                    if (consulta != null)
                    {
                        // Cliente ya registrado
                        WebSocket webSocket = (await context.AcceptWebSocketAsync(null)).WebSocket;

                        if (!connectedClients.ContainsKey(clientId))
                            connectedClients[clientId] = new List<WebSocket>();

                        connectedClients[clientId].Add(webSocket);

                        AppendLog($"✅ Conexión WebSocket aceptada.{clientId} {modelo}");
                        var dispo = dispoditivos.GetUnic(clientId, _ID);
                        dispoditivos.Update(new Dispositivos
                        {
                            Id = dispo.Id,
                            Name = dispo.Name,
                            IDequipo = dispo.IDequipo,
                            Ip = dispo.Ip,
                            Estado = "Conectado",
                            EventoID = dispo.EventoID,
                        });

                     

                        _ = Task.Run(() => HandleWebSocketMessages(consulta, webSocket));
                        var disp = dispoditivos.Getlist(_ID);
                        this.Invoke(new Action(() =>
                        {
                            dataDispositi.Rows.Clear();
                            //foreach (var device in disp)
                            //    dataDispositi.Rows.Add(device.Id, device.Name, device.IDequipo, device.Estado);
                            CargarClientesEnGrid();
                        }));
                    }
                    else
                    {
                        // Cliente no registrado
                        string remoteIP = context.Request.RemoteEndPoint?.ToString() ?? "Desconocido";


                        // Mostrar el formulario de aprobación en un hilo aparte para evitar bloqueo
                        if (materialCheckbox2.Checked)
                        {
                            String cli = clientId.Substring(0, 7);
                            var registro = dispoditivos.Insert(new Dispositivos
                            {
                                Name = modelo,
                                IDequipo = clientId,
                                Ip = remoteIP,
                                Estado = "Conectado",
                                EventoID = _ID,
                            }, _ID);

                            WebSocket webSocket = (await context.AcceptWebSocketAsync(null)).WebSocket;

                            if (!connectedClients.ContainsKey(clientId))
                                connectedClients[clientId] = new List<WebSocket>();

                            connectedClients[clientId].Add(webSocket);

                            // AppendLog("✅ Conexión WebSocket aceptada .");
                            AppendLog($"✅ Conexión WebSocket aceptada.{clientId} {modelo}");
                            var disp = dispoditivos.Getlist(_ID);
                            this.Invoke(new Action(() =>
                            {
                                dataDispositi.Rows.Clear();
                                foreach (var device in disp)
                                    dataDispositi.Rows.Add(device.Id, device.Name, device.IDequipo, device.Estado);
                                CargarClientesEnGrid();
                            }));

                            _ = Task.Run(() => HandleWebSocketMessages(registro, webSocket));


                        }
                        if (!materialCheckbox2.Checked)
                        {
                            Task<bool> solicitudAprobacion = Task.Run(() =>
                        {
                            bool aprobado = false;

                            this.Invoke(new MethodInvoker(() =>
                            {
                                using (var form = new FormAprobacion(clientId, modelo))
                                {
                                    form.StartPosition = FormStartPosition.CenterScreen; // Centrado
                                    form.TopMost = true; // Siempre al frente
                                    System.Media.SystemSounds.Exclamation.Play(); // Sonido
                                    form.ShowDialog();
                                    aprobado = form.Aprobado;
                                }
                            }));

                            return aprobado;
                        });

                            bool aceptar = await solicitudAprobacion;
                            AppendLog("✅ Conexión entrante (nuevo cliente).");
                            if (aceptar)
                            {
                                String cli = clientId.Substring(0, 7);
                                var registro = dispoditivos.Insert(new Dispositivos
                                {
                                    Name = modelo,
                                    IDequipo = clientId,
                                    Ip = remoteIP,
                                    Estado = "Conectado",
                                    EventoID = _ID,
                                }, _ID);

                                WebSocket webSocket = (await context.AcceptWebSocketAsync(null)).WebSocket;

                                if (!connectedClients.ContainsKey(clientId))
                                    connectedClients[clientId] = new List<WebSocket>();

                                connectedClients[clientId].Add(webSocket);

                                // AppendLog("✅ Conexión WebSocket aceptada .");
                                AppendLog($"✅ Conexión WebSocket aceptada.{clientId} {modelo}");
                                var disp = dispoditivos.Getlist(_ID);
                                this.Invoke(new Action(() =>
                                {
                                    dataDispositi.Rows.Clear();
                                    foreach (var device in disp)
                                        dataDispositi.Rows.Add(device.Id, device.Name, device.IDequipo, device.Estado);
                                    CargarClientesEnGrid();
                                }));

                                _ = Task.Run(() => HandleWebSocketMessages(registro, webSocket));
                            }
                            else
                            {
                                AppendLog("❌ Conexión rechazada.");
                                context.Response.StatusCode = 403;
                                context.Response.Close();
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    AppendLog($"❌ Error en la conexión WebSocket: {ex.Message}");
                }
            }
        }



        public void AbrirPuertoFirewall(int puerto)
        {
            if (!EsAdministrador())
            {
                MessageBox.Show("Debe ejecutar la aplicación como administrador para modificar el firewall.");
                return;
            }

            string nombreRegla = $"Regla WebSocket Puerto {puerto}";
            string comando = $"advfirewall firewall add rule name=\"{nombreRegla}\" dir=in action=allow protocol=TCP localport={puerto}";

            ProcessStartInfo psi = new ProcessStartInfo("netsh", comando)
            {
                Verb = "runas",
                CreateNoWindow = true,
                UseShellExecute = true
            };

            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear la regla del firewall: " + ex.Message);
            }
        }

        static bool EsAdministrador()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }


        public void EliminarPuertoFirewall(int puertos)
        {
            if (!EsAdministrador())
            {
                MessageBox.Show("Debe ejecutar la aplicación como administrador para modificar el firewall.");
                return;
            }
            string nombreRegla = $"Regla WebSocket Puerto {puertos}";
            string comando = $"advfirewall firewall delete rule name=\"{nombreRegla}\" protocol=TCP localport={puertos}";

            ProcessStartInfo psi = new ProcessStartInfo("netsh", comando)
            {
                Verb = "runas",
                CreateNoWindow = true,
                UseShellExecute = true
            };

            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar la regla del firewall: " + ex.Message);
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
                        var datosrecibidos = JsonSerializer.Deserialize<MessageSocket>(message);
                        datosrecibidos.Type = clientId.IDequipo;
                        AppendLog($"📩 Equipo [{clientId.IDequipo}] → Código entrante");

                        var verifica = VerificaCodigo(Convert.ToString(clientId.Id), datosrecibidos);
                        var options = new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        };

                        byte[] responseBuffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(verifica, options));

                        await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);


                        this.Invoke(new Action(() =>
                        {
                            Eventosinfo();
                        }));
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break; // permite salir para que se ejecute el finally
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog($"⚠️ Error con cliente {clientId.IDequipo}: {ex.Message}");
            }
            finally
            {
                try
                {
                    // Actualiza estado en DB
                    DispoditivosRepsoitory dispoditivo = new DispoditivosRepsoitory();
                    var cliente = dispoditivo.GetUnic(clientId.IDequipo, clientId.EventoID);
                    var dispo = new Dispositivos
                    {
                        Id = clientId.Id,
                        Name = cliente.Name,
                        IDequipo = clientId.IDequipo,
                        Ip = clientId.Ip,
                        Estado = "Desconectado",
                        EventoID = clientId.EventoID,
                    };
                    dispoditivo.Update(dispo);

                    // Cierra socket si aún está abierto
                    if (webSocket.State != WebSocketState.Closed)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Desconectado", CancellationToken.None);
                    }

                    // Elimina solo este socket del diccionario
                    if (connectedClients.TryGetValue(clientId.IDequipo, out var sockets))
                    {
                        sockets.Remove(webSocket);
                        if (sockets.Count == 0)
                            connectedClients.Remove(clientId.IDequipo);
                    }

                    // Refresca UI
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

                    AppendLog($"🔌 Cliente {clientId.IDequipo} desconectado.");
                }
                catch (Exception cleanupEx)
                {
                    AppendLog($"⚠️ Error durante la limpieza de {clientId.IDequipo}: {cleanupEx.Message}");
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
            materialCheckbox1.Checked = (dato.SelecionLocation == 1);
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
                datagridLocalidad.Rows.Add(device.Name, device.Count, device.Scaneado);
            }

            var codi = codigos.GetList(Convert.ToString(_ID)).Select(x => new { x.Name, x.Codigo, x.Asiento, x.info, x.Estado, x.time }).ToList();
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
                if (isRunning)
                {
                    e.Cancel = true;
                    DialogResult result = MessageBox.Show("Debe Detener El servicio Antes de Salir", "INFO", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);



                    return;
                }
                e.Cancel = false;

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
                // Solo interfaces activas y que no sean loopback
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    var ipProperties = networkInterface.GetIPProperties();

                    // Verificar si tiene puerta de enlace válida
                    if (ipProperties.GatewayAddresses.Any(g => g.Address.AddressFamily == AddressFamily.InterNetwork &&
                                                               !IPAddress.IsLoopback(g.Address) &&
                                                               !g.Address.Equals(IPAddress.Any)))
                    {
                        foreach (var ipAddress in ipProperties.UnicastAddresses)
                        {
                            if (ipAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return ipAddress.Address.ToString();
                            }
                        }
                    }
                }
            }

            return string.Empty; // No se encontró IP con puerta de enlace válida
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

                var conexiones = kvp.Value;

                // Si al menos un socket está abierto, consideramos al cliente como conectado
                bool hayConectado = conexiones.Any(ws => ws.State == WebSocketState.Open);
                lista.Add(new ClienteSocket
                {
                    Dispositivo = id,
                    Estado = hayConectado ? "Conectado" : "Desconectado"
                });
            }

            datagridCliente.DataSource = lista;
            var dat = lista.Where(x => x.Estado == "Conectado").ToList();
            scanerconetado.Text = dat.Count.ToString();
            CodigosRepository codigos = new CodigosRepository();
            LogsEvenRepository LogsEven = new LogsEvenRepository();
            DispoditivosRepsoitory dispoditivos = new DispoditivosRepsoitory();
            var codi = codigos.GetList(Convert.ToString(_ID)).Select(x => new { x.Name, x.Codigo, x.Asiento, x.info, x.Estado, x.time }).ToList();
            var logs = LogsEven.ObtenerLista(_ID);
            var disp = dispoditivos.Getlist(_ID);
            txtValidos.Text = codi.Where(x => x.Estado == "Scaneado").ToList().Count().ToString();
            txtRechazados.Text = logs.Where(x => x.Estado.Equals("Rechazado")).ToList().Count().ToString();
            txtRepetidos.Text = logs.Where(x => x.Estado.Equals("Repetido")).ToList().Count().ToString();

            dataDispositi.Rows.Clear();
            foreach (var device in disp)
                dataDispositi.Rows.Add(device.Id, device.Name, device.IDequipo, device.Estado);

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
            if (connectedClients.TryGetValue(clientId, out List<WebSocket> sockets))
            {
                foreach (var socket in sockets.ToList()) // Clonar para evitar modificar mientras se itera
                {
                    if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
                    {
                        try
                        {
                            await socket.CloseAsync(
                                WebSocketCloseStatus.NormalClosure,
                                "Desconexión solicitada",
                                CancellationToken.None
                            );
                        }
                        catch (Exception ex)
                        {
                            //
                            AppendLog($"Error al cerrar conexión WebSocket: {ex.Message}");
                        }
                    }
                }

                // Limpiar conexiones cerradas
                sockets.RemoveAll(ws => ws.State != WebSocketState.Open);
                if (sockets.Count == 0)
                {
                    connectedClients.Remove(clientId);
                }
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
                        worksheet.Cell(i + 2, j + 1).Value = datagridcoidgos.Rows[i].Cells[j].Value?.ToString();
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
                    DialogResult result = MessageBox.Show("¿Estás seguro que deseas Eliminar este Dispositivo?", "Confirmar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        DispoditivosRepsoitory dispoditivos = new DispoditivosRepsoitory();
                        dispoditivos.Delete(id);
                        dataDispositi.Rows.RemoveAt(e.RowIndex);
                        DesconectarClienteAsync(idequipo);
                        MessageBox.Show("Registro eliminado." + idequipo);
                    }
                }
                if (column.Name == "PermitirDisp")
                {
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
            var qrcodetxt = JsonSerializer.Serialize(new { ip = GetLocalIPAddress(), puerto = puertoTxt.Text });

            //Add Barcode value below the generated barcode
            //var qrcode = QRCodeWriter.CreateQrCode(qrcodetxt);
            //qrcode.AddBarcodeValueTextBelowBarcode();
            //qrcode.ToJpegBinaryData();
            Qrcode qrcode1 = new Qrcode(qrcodetxt);
            qrcode1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /* if (button1.Text == "PARAR")
             {
                 DialogResult result = MessageBox.Show("¿Estás seguro que deseas Eliminar este Dispositivo?", "Confirmar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                 if (result == DialogResult.OK)
                 {

                 }
             }*/
            CodigosView codigos = new CodigosView(_ID);
            // this.Close();
            codigos.ShowDialog();
        }

        private void datagridLocalidad_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataDispositi2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void datagridcoidgos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void ActualizarGrid(List<Dispositivos> lista)
        {
            dataDispositi.Rows.Clear();
            foreach (var device in lista)
            {
                dataDispositi.Rows.Add(device.Id, device.Name, device.IDequipo, device.Estado);
            }
        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            DispoditivosRepsoitory dispoditivos = new DispoditivosRepsoitory();
            var disp = dispoditivos.Getlist(_ID);
            string texto = txtBuscar.Text.Trim().ToLower();
            var filtrados = disp
                .Where(d => d.Name.ToLower().Contains(texto) || d.IDequipo.ToLower().Contains(texto))
                .ToList();
            ActualizarGrid(filtrados);
        }

        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {

            EventosRepository eventosRepository = new EventosRepository();
            var dato = eventosRepository.Get(Convert.ToString(_ID));
            if (materialCheckbox1.Checked)
            {
                eventosRepository.Update(new Eventos
                {
                    Id = _ID,
                    Description = dato.Description,
                    Nombre = dato.Nombre,
                    Fecha = dato.Fecha,
                    SelecionLocation = materialCheckbox1.Checked ? 1 : 0,
                });

                // Está activado
                // MessageBox.Show("CheckBox marcado");
            }
            else
            {
                eventosRepository.Update(new Eventos
                {
                    Id = _ID,
                    Description = dato.Description,
                    Nombre = dato.Nombre,
                    Fecha = dato.Fecha,
                    SelecionLocation = materialCheckbox1.Checked ? 1 : 0,
                });
                // Está desactivado
                //MessageBox.Show("CheckBox desmarcado");
            }
        }

        private void materialCheckbox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
