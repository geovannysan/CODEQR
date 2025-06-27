namespace NEWCODES.Vistas
{
    partial class EventosIDServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventosIDServer));
            button1 = new Button();
            puertoTxt = new TextBox();
            label1 = new Label();
            txtLog = new TextBox();
            Dispositivos = new TabControl();
            tabPage1 = new TabPage();
            groupBox3 = new GroupBox();
            datagridCliente = new DataGridView();
            groupBox2 = new GroupBox();
            txtRepetidos = new Label();
            label12 = new Label();
            txtRechazados = new Label();
            txtValidos = new Label();
            scanerconetado = new Label();
            label14 = new Label();
            label13 = new Label();
            label11 = new Label();
            tabPage2 = new TabPage();
            txtBuscar = new TextBox();
            dataDispositi = new DataGridView();
            IDDispo = new DataGridViewTextBoxColumn();
            Name = new DataGridViewTextBoxColumn();
            Equipogrid = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            tabPage3 = new TabPage();
            splitContainer2 = new SplitContainer();
            button2 = new Button();
            datagridLocalidad = new DataGridView();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Scaneado = new DataGridViewTextBoxColumn();
            button3 = new Button();
            ExportarCodigos = new Button();
            datagridcoidgos = new DataGridView();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            countScxaner = new Label();
            label3 = new Label();
            label2 = new Label();
            groupBox1 = new GroupBox();
            materialCheckbox2 = new MaterialSkin.Controls.MaterialCheckbox();
            materialCheckbox1 = new MaterialSkin.Controls.MaterialCheckbox();
            pictureBox1 = new PictureBox();
            ipserver = new Label();
            txtEvento = new Label();
            Server = new Label();
            label10 = new Label();
            Dispositivos.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)datagridCliente).BeginInit();
            groupBox2.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataDispositi).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)datagridLocalidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)datagridcoidgos).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(553, 47);
            button1.Name = "button1";
            button1.Size = new Size(100, 41);
            button1.TabIndex = 0;
            button1.Text = "INICIAR";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // puertoTxt
            // 
            puertoTxt.Location = new Point(553, 16);
            puertoTxt.Name = "puertoTxt";
            puertoTxt.Size = new Size(100, 23);
            puertoTxt.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(480, 19);
            label1.Name = "label1";
            label1.Size = new Size(53, 15);
            label1.TabIndex = 2;
            label1.Text = "PUERTO";
            // 
            // txtLog
            // 
            txtLog.BackColor = SystemColors.ButtonHighlight;
            txtLog.Location = new Point(6, 486);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.Size = new Size(668, 76);
            txtLog.TabIndex = 3;
            // 
            // Dispositivos
            // 
            Dispositivos.Controls.Add(tabPage1);
            Dispositivos.Controls.Add(tabPage2);
            Dispositivos.Controls.Add(tabPage3);
            Dispositivos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            Dispositivos.Location = new Point(12, 179);
            Dispositivos.Name = "Dispositivos";
            Dispositivos.SelectedIndex = 0;
            Dispositivos.Size = new Size(673, 286);
            Dispositivos.TabIndex = 4;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox3);
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(665, 258);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(datagridCliente);
            groupBox3.Location = new Point(271, 6);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(387, 246);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            // 
            // datagridCliente
            // 
            datagridCliente.AllowUserToAddRows = false;
            datagridCliente.AllowUserToDeleteRows = false;
            datagridCliente.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datagridCliente.BackgroundColor = SystemColors.Menu;
            datagridCliente.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            datagridCliente.Dock = DockStyle.Fill;
            datagridCliente.EditMode = DataGridViewEditMode.EditProgrammatically;
            datagridCliente.Location = new Point(3, 19);
            datagridCliente.Name = "datagridCliente";
            datagridCliente.ReadOnly = true;
            datagridCliente.Size = new Size(381, 224);
            datagridCliente.TabIndex = 0;
            datagridCliente.CellContentClick += datagridCliente_CellContentClick;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtRepetidos);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(txtRechazados);
            groupBox2.Controls.Add(txtValidos);
            groupBox2.Controls.Add(scanerconetado);
            groupBox2.Controls.Add(label14);
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(label11);
            groupBox2.Location = new Point(3, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.RightToLeft = RightToLeft.No;
            groupBox2.Size = new Size(262, 246);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            // 
            // txtRepetidos
            // 
            txtRepetidos.AutoSize = true;
            txtRepetidos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtRepetidos.ForeColor = Color.FromArgb(255, 128, 0);
            txtRepetidos.Location = new Point(184, 194);
            txtRepetidos.Name = "txtRepetidos";
            txtRepetidos.Size = new Size(14, 15);
            txtRepetidos.TabIndex = 7;
            txtRepetidos.Text = "0";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label12.Location = new Point(29, 194);
            label12.Name = "label12";
            label12.Size = new Size(66, 15);
            label12.TabIndex = 6;
            label12.Text = "Repetidos:";
            // 
            // txtRechazados
            // 
            txtRechazados.AutoSize = true;
            txtRechazados.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtRechazados.ForeColor = Color.DodgerBlue;
            txtRechazados.Location = new Point(184, 161);
            txtRechazados.Name = "txtRechazados";
            txtRechazados.Size = new Size(14, 15);
            txtRechazados.TabIndex = 5;
            txtRechazados.Text = "0";
            // 
            // txtValidos
            // 
            txtValidos.AutoSize = true;
            txtValidos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtValidos.ForeColor = Color.Green;
            txtValidos.Location = new Point(184, 128);
            txtValidos.Name = "txtValidos";
            txtValidos.Size = new Size(14, 15);
            txtValidos.TabIndex = 4;
            txtValidos.Text = "0";
            // 
            // scanerconetado
            // 
            scanerconetado.AutoSize = true;
            scanerconetado.BackColor = Color.Transparent;
            scanerconetado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            scanerconetado.ForeColor = Color.Red;
            scanerconetado.Location = new Point(184, 95);
            scanerconetado.Name = "scanerconetado";
            scanerconetado.Size = new Size(14, 15);
            scanerconetado.TabIndex = 3;
            scanerconetado.Text = "0";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label14.Location = new Point(29, 161);
            label14.Name = "label14";
            label14.Size = new Size(70, 15);
            label14.TabIndex = 2;
            label14.Text = "Rechazado:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label13.Location = new Point(32, 128);
            label13.Name = "label13";
            label13.Size = new Size(48, 15);
            label13.TabIndex = 1;
            label13.Text = "Validos:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label11.Location = new Point(29, 95);
            label11.Name = "label11";
            label11.Size = new Size(117, 15);
            label11.TabIndex = 0;
            label11.Text = "Scanner Conectado:";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(txtBuscar);
            tabPage2.Controls.Add(dataDispositi);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(665, 258);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(6, 7);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(647, 23);
            txtBuscar.TabIndex = 1;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            // 
            // dataDispositi
            // 
            dataDispositi.AllowUserToAddRows = false;
            dataDispositi.AllowUserToDeleteRows = false;
            dataDispositi.AllowUserToOrderColumns = true;
            dataDispositi.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataDispositi.Columns.AddRange(new DataGridViewColumn[] { IDDispo, Name, Equipogrid, Column3 });
            dataDispositi.Location = new Point(3, 33);
            dataDispositi.MultiSelect = false;
            dataDispositi.Name = "dataDispositi";
            dataDispositi.Size = new Size(659, 219);
            dataDispositi.TabIndex = 0;
            dataDispositi.CellContentClick += dataDispositi_CellContentClick;
            // 
            // IDDispo
            // 
            IDDispo.HeaderText = "ID";
            IDDispo.Name = "IDDispo";
            IDDispo.Visible = false;
            // 
            // Name
            // 
            Name.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Name.HeaderText = "Name";
            Name.Name = "Name";
            // 
            // Equipogrid
            // 
            Equipogrid.HeaderText = "Equipo";
            Equipogrid.Name = "Equipogrid";
            Equipogrid.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.HeaderText = "Estado";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(splitContainer2);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(665, 258);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(3, 3);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(button2);
            splitContainer2.Panel1.Controls.Add(datagridLocalidad);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(button3);
            splitContainer2.Panel2.Controls.Add(ExportarCodigos);
            splitContainer2.Panel2.Controls.Add(datagridcoidgos);
            splitContainer2.Size = new Size(659, 252);
            splitContainer2.SplitterDistance = 316;
            splitContainer2.TabIndex = 0;
            // 
            // button2
            // 
            button2.Location = new Point(3, 5);
            button2.Name = "button2";
            button2.Size = new Size(277, 23);
            button2.TabIndex = 8;
            button2.Text = "Exportar Actividad";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            // 
            // datagridLocalidad
            // 
            datagridLocalidad.AllowUserToAddRows = false;
            datagridLocalidad.AllowUserToDeleteRows = false;
            datagridLocalidad.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            datagridLocalidad.Columns.AddRange(new DataGridViewColumn[] { Column4, Column5, Scaneado });
            datagridLocalidad.Location = new Point(0, 34);
            datagridLocalidad.Name = "datagridLocalidad";
            datagridLocalidad.ReadOnly = true;
            datagridLocalidad.Size = new Size(313, 215);
            datagridLocalidad.TabIndex = 0;
            datagridLocalidad.CellContentClick += datagridLocalidad_CellContentClick;
            // 
            // Column4
            // 
            Column4.HeaderText = "Localidad";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Width = 90;
            // 
            // Column5
            // 
            Column5.HeaderText = "Cantidad";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            // 
            // Scaneado
            // 
            Scaneado.HeaderText = "Scaneado";
            Scaneado.Name = "Scaneado";
            Scaneado.ReadOnly = true;
            // 
            // button3
            // 
            button3.Location = new Point(155, 5);
            button3.Name = "button3";
            button3.Size = new Size(145, 23);
            button3.TabIndex = 3;
            button3.Text = "Agregsr Códigos";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // ExportarCodigos
            // 
            ExportarCodigos.Location = new Point(3, 5);
            ExportarCodigos.Name = "ExportarCodigos";
            ExportarCodigos.Size = new Size(145, 23);
            ExportarCodigos.TabIndex = 2;
            ExportarCodigos.Text = "Export Códigos";
            ExportarCodigos.UseVisualStyleBackColor = true;
            ExportarCodigos.Click += ExportarCodigos_Click;
            // 
            // datagridcoidgos
            // 
            datagridcoidgos.AllowUserToAddRows = false;
            datagridcoidgos.AllowUserToDeleteRows = false;
            datagridcoidgos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            datagridcoidgos.Location = new Point(3, 34);
            datagridcoidgos.Name = "datagridcoidgos";
            datagridcoidgos.ReadOnly = true;
            datagridcoidgos.Size = new Size(333, 215);
            datagridcoidgos.TabIndex = 0;
            datagridcoidgos.CellContentClick += datagridcoidgos_CellContentClick;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Times New Roman", 12F);
            label9.ForeColor = Color.Red;
            label9.Location = new Point(136, 125);
            label9.Name = "label9";
            label9.Size = new Size(17, 19);
            label9.TabIndex = 7;
            label9.Text = "0";
            label9.TextAlign = ContentAlignment.TopRight;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Times New Roman", 12F);
            label8.ForeColor = Color.Red;
            label8.Location = new Point(136, 93);
            label8.Name = "label8";
            label8.Size = new Size(17, 19);
            label8.TabIndex = 6;
            label8.Text = "0";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Times New Roman", 12F);
            label7.ForeColor = Color.Red;
            label7.Location = new Point(136, 49);
            label7.Name = "label7";
            label7.Size = new Size(17, 19);
            label7.TabIndex = 5;
            label7.Text = "0";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Times New Roman", 12F);
            label6.Location = new Point(3, 125);
            label6.Name = "label6";
            label6.Size = new Size(104, 19);
            label6.TabIndex = 4;
            label6.Text = "No encontrado:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Times New Roman", 12F);
            label5.Location = new Point(3, 93);
            label5.Name = "label5";
            label5.Size = new Size(85, 19);
            label5.TabIndex = 3;
            label5.Text = "Rechazados:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 12F);
            label4.Location = new Point(3, 49);
            label4.Name = "label4";
            label4.Size = new Size(85, 19);
            label4.TabIndex = 2;
            label4.Text = "Aprobados: ";
            // 
            // countScxaner
            // 
            countScxaner.AutoSize = true;
            countScxaner.Font = new Font("Times New Roman", 12F);
            countScxaner.ForeColor = Color.Red;
            countScxaner.Location = new Point(136, 12);
            countScxaner.Name = "countScxaner";
            countScxaner.Size = new Size(17, 19);
            countScxaner.TabIndex = 1;
            countScxaner.Text = "0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 12F);
            label3.Location = new Point(3, 12);
            label3.Name = "label3";
            label3.Size = new Size(127, 19);
            label3.TabIndex = 0;
            label3.Text = "Scaner conectados:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(12, 468);
            label2.Name = "label2";
            label2.Size = new Size(98, 15);
            label2.TabIndex = 5;
            label2.Text = "Log de conexion";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(materialCheckbox2);
            groupBox1.Controls.Add(materialCheckbox1);
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Controls.Add(ipserver);
            groupBox1.Controls.Add(txtEvento);
            groupBox1.Controls.Add(Server);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(puertoTxt);
            groupBox1.Controls.Add(label1);
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBox1.Location = new Point(16, 79);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(659, 94);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Server";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // materialCheckbox2
            // 
            materialCheckbox2.AutoSize = true;
            materialCheckbox2.Depth = 0;
            materialCheckbox2.Location = new Point(283, 10);
            materialCheckbox2.Margin = new Padding(0);
            materialCheckbox2.MouseLocation = new Point(-1, -1);
            materialCheckbox2.MouseState = MaterialSkin.MouseState.HOVER;
            materialCheckbox2.Name = "materialCheckbox2";
            materialCheckbox2.ReadOnly = false;
            materialCheckbox2.Ripple = true;
            materialCheckbox2.Size = new Size(190, 37);
            materialCheckbox2.TabIndex = 10;
            materialCheckbox2.Text = "Autorizar Dispositivos";
            materialCheckbox2.UseVisualStyleBackColor = true;
            // 
            // materialCheckbox1
            // 
            materialCheckbox1.AutoSize = true;
            materialCheckbox1.Depth = 0;
            materialCheckbox1.Location = new Point(284, 47);
            materialCheckbox1.Margin = new Padding(0);
            materialCheckbox1.MouseLocation = new Point(-1, -1);
            materialCheckbox1.MouseState = MaterialSkin.MouseState.HOVER;
            materialCheckbox1.Name = "materialCheckbox1";
            materialCheckbox1.ReadOnly = false;
            materialCheckbox1.Ripple = true;
            materialCheckbox1.Size = new Size(189, 37);
            materialCheckbox1.TabIndex = 9;
            materialCheckbox1.Text = "Autorizar Localidades";
            materialCheckbox1.UseVisualStyleBackColor = true;
            materialCheckbox1.CheckedChanged += materialCheckbox1_CheckedChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(501, 47);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(32, 31);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            pictureBox1.Visible = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // ipserver
            // 
            ipserver.AutoSize = true;
            ipserver.Location = new Point(106, 60);
            ipserver.Name = "ipserver";
            ipserver.Size = new Size(0, 15);
            ipserver.TabIndex = 6;
            // 
            // txtEvento
            // 
            txtEvento.AutoSize = true;
            txtEvento.Location = new Point(106, 26);
            txtEvento.Name = "txtEvento";
            txtEvento.Size = new Size(0, 15);
            txtEvento.TabIndex = 5;
            // 
            // Server
            // 
            Server.AutoSize = true;
            Server.Location = new Point(18, 60);
            Server.Name = "Server";
            Server.Size = new Size(65, 15);
            Server.TabIndex = 4;
            Server.Text = "Ip Server: ";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(18, 26);
            label10.Name = "label10";
            label10.Size = new Size(49, 15);
            label10.TabIndex = 3;
            label10.Text = "Evento:";
            // 
            // EventosIDServer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(691, 568);
            Controls.Add(groupBox1);
            Controls.Add(label2);
            Controls.Add(Dispositivos);
            Controls.Add(txtLog);
           // Name = "EventosIDServer";
            StartPosition = FormStartPosition.CenterParent;
            Text = "EventosIDServer";
            Load += EventosIDServer_Load;
            Dispositivos.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)datagridCliente).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataDispositi).EndInit();
            tabPage3.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)datagridLocalidad).EndInit();
            ((System.ComponentModel.ISupportInitialize)datagridcoidgos).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox puertoTxt;
        private Label label1;
        private TabControl Dispositivos;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private Label label2;
        private GroupBox groupBox1;
        private Label countScxaner;
        private Label label3;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label9;
        private SplitContainer splitContainer2;
        private DataGridView datagridLocalidad;
        private DataGridView datagridcoidgos;
        private Button ExportarCodigos;
        private Label label10;
        private Label txtEvento;
        private Label Server;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private Label ipserver;
        private Label label14;
        private Label label13;
        private Label label11;
        private Label txtRechazados;
        private Label txtValidos;
        private Label scanerconetado;
        private DataGridView dataDispositi;
        private DataGridView datagridCliente;
        private Label txtRepetidos;
        private Label label12;
        private Button button2;
        private PictureBox pictureBox1;
        private Button button3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Scaneado;
        private DataGridViewTextBoxColumn IDDispo;
        private DataGridViewTextBoxColumn Name;
        private DataGridViewTextBoxColumn Equipogrid;
        private DataGridViewTextBoxColumn Column3;
        public TextBox txtLog;
        private TextBox txtBuscar;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox1;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox2;
    }
}