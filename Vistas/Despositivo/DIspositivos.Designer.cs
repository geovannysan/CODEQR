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
            materialButton1 = new MaterialSkin.Controls.MaterialButton();
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
            datagridLocalidad.Location = new Point(6, 97);
            datagridLocalidad.Name = "datagridLocalidad";
            datagridLocalidad.Size = new Size(494, 256);
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
            // materialButton1
            // 
            materialButton1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton1.Depth = 0;
            materialButton1.HighEmphasis = true;
            materialButton1.Icon = null;
            materialButton1.Location = new Point(208, 362);
            materialButton1.Margin = new Padding(4, 6, 4, 6);
            materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton1.Name = "materialButton1";
            materialButton1.NoAccentTextColor = Color.Empty;
            materialButton1.Size = new Size(88, 36);
            materialButton1.TabIndex = 3;
            materialButton1.Text = "GUARDAR";
            materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton1.UseAccentColor = false;
            materialButton1.UseVisualStyleBackColor = true;
            materialButton1.Click += materialButton1_Click;
            // 
            // DIspositivosF
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(506, 432);
            Controls.Add(materialButton1);
            Controls.Add(datagridLocalidad);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "DIspositivosF";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DIspositivos";
            Load += DIspositivos_Load;
            ((System.ComponentModel.ISupportInitialize)datagridLocalidad).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private DataGridView datagridLocalidad;
        private DataGridViewCheckBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private MaterialSkin.Controls.MaterialButton materialButton1;
    }
}