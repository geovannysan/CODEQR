namespace NEWCODES.Vistas.Despositivo
{
    partial class DIspositivosF
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
            button1 = new Button();
            datagridLocalidad = new DataGridView();
            Column1 = new DataGridViewCheckBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)datagridLocalidad).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(105, 256);
            button1.Name = "button1";
            button1.Size = new Size(271, 36);
            button1.TabIndex = 1;
            button1.Text = "GUARDAR";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // datagridLocalidad
            // 
            datagridLocalidad.AllowUserToAddRows = false;
            datagridLocalidad.AllowUserToDeleteRows = false;
            datagridLocalidad.BackgroundColor = SystemColors.ControlLightLight;
            datagridLocalidad.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            datagridLocalidad.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4 });
            datagridLocalidad.Location = new Point(14, 40);
            datagridLocalidad.Name = "datagridLocalidad";
            datagridLocalidad.Size = new Size(445, 210);
            datagridLocalidad.TabIndex = 2;
            datagridLocalidad.CellContentClick += datagridLocalidad_CellContentClick;
            // 
            // Column1
            // 
            Column1.HeaderText = "Autorizado";
            Column1.Name = "Column1";
            // 
            // Column2
            // 
            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column2.HeaderText = "Localidad";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.True;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            Column3.HeaderText = "IDlolalidad";
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.True;
            Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column3.Visible = false;
            Column3.Width = 202;
            // 
            // Column4
            // 
            Column4.HeaderText = "DispoID";
            Column4.Name = "Column4";
            Column4.Visible = false;
            // 
            // DIspositivosF
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(471, 305);
            Controls.Add(datagridLocalidad);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "DIspositivosF";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DIspositivos";
            Load += DIspositivos_Load;
            ((System.ComponentModel.ISupportInitialize)datagridLocalidad).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private DataGridView datagridLocalidad;
        private DataGridViewCheckBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
    }
}