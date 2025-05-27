using MaterialSkin.Controls;

namespace NEWCODES.Vistas.Componetes
{
    public partial class FormAprobacion : MaterialForm
    {

        private int tiempoRestante = 10; // segundos
        public bool Aprobado { get; private set; } = false;
        private System.Windows.Forms.Timer timer;

        public FormAprobacion(string clientId,string nombre)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            progressBar.Value = 10;
            lblMensaje.Text = $"Nueva conexión desde ID: {nombre}";
            lblTiempo.Text = $"Tiempo restante: {tiempoRestante} s";

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tiempoRestante--;
            lblTiempo.Text = $"Tiempo restante: {tiempoRestante} s";
            if (tiempoRestante >= 0)
            {
                progressBar.Value = tiempoRestante;
            }
            if (tiempoRestante <= 0)
            {
                Aprobado = true;
                timer.Stop();
                this.Close(); // Se cierra automáticamente
            }
        }




        private void FormAprobacion_Load(object sender, EventArgs e)
        {

        }

        private void btnAceptar_Click_1(object sender, EventArgs e)
        {
            Aprobado = true;
            timer.Stop();
            this.Close();
        }

        private void btnRechazar_Click(object sender, EventArgs e)
        {
            Aprobado = false;
            timer.Stop();
            this.Close();
        }
    }
}
