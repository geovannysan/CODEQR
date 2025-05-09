namespace NEWCODES.Vistas.Componetes
{
    partial class FormAprobacion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblMensaje = new Label();
            lblTiempo = new Label();
            btnAceptar = new Button();
            btnRechazar = new Button();
            progressBar = new ProgressBar();
            SuspendLayout();
            // 
            // lblMensaje
            // 
            lblMensaje.AutoSize = true;
            lblMensaje.Location = new Point(98, 76);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new Size(194, 15);
            lblMensaje.TabIndex = 0;
            lblMensaje.Text = "Nueva conexión desde ID: {clientId}";
            // 
            // lblTiempo
            // 
            lblTiempo.AutoSize = true;
            lblTiempo.Location = new Point(128, 104);
            lblTiempo.Name = "lblTiempo";
            lblTiempo.Size = new Size(118, 15);
            lblTiempo.TabIndex = 1;
            lblTiempo.Text = "Tiempo restante: 10 s";
            lblTiempo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(48, 174);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(75, 23);
            btnAceptar.TabIndex = 2;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click_1;
            // 
            // btnRechazar
            // 
            btnRechazar.Location = new Point(260, 174);
            btnRechazar.Name = "btnRechazar";
            btnRechazar.Size = new Size(75, 23);
            btnRechazar.TabIndex = 3;
            btnRechazar.Text = "Rechazar";
            btnRechazar.UseVisualStyleBackColor = true;
            btnRechazar.Click += btnRechazar_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(48, 136);
            progressBar.MarqueeAnimationSpeed = 10;
            progressBar.Maximum = 10;
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(287, 23);
            progressBar.TabIndex = 4;
            // 
            // FormAprobacion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(404, 254);
            Controls.Add(lblMensaje);
            Controls.Add(lblTiempo);
            Controls.Add(progressBar);
            Controls.Add(btnRechazar);
            Controls.Add(btnAceptar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FormAprobacion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Aprobacion";
            Load += FormAprobacion_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMensaje;
        private Label lblTiempo;
        private Button btnAceptar;
        private Button btnRechazar;
        private ProgressBar progressBar;
    }
}