using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using NEWCODES.Infraestructura;
using Microsoft.EntityFrameworkCore;
namespace NEWCODES.Vistas
{
    public partial class Comand : Form
    {

        private readonly EevntoContext _context;
        public Comand()
        {
            InitializeComponent();

            _context = new EevntoContext(); // crea una instancia del DbContext
        }

        private void Comand_Load(object sender, EventArgs e)
        {

            this.Text = "Ejecutor SQL - NEWCODES";
            this.Width = 900;
            this.Height = 600;

            var lbl = new Label { Text = "Comando SQL:", Top = 10, Left = 10 };
            var txtComando = new TextBox { Name = "txtComando", Top = 30, Left = 10, Width = 850, Height = 100, Multiline = true, ScrollBars = ScrollBars.Vertical };
            var btnEjecutar = new Button { Text = "Ejecutar", Top = 140, Left = 10, Width = 100, Height = 30 };
            var dgvResultados = new DataGridView { Name = "dgvResultados", Top = 180, Left = 10, Width = 850, Height = 380, ReadOnly = true };

            btnEjecutar.Click += (s, ev) => EjecutarComando(txtComando.Text, dgvResultados);

            this.Controls.Add(lbl);
            this.Controls.Add(txtComando);
            this.Controls.Add(btnEjecutar);
            this.Controls.Add(dgvResultados);
        }
        private void EjecutarComando(string sql, DataGridView dgv)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                MessageBox.Show("Por favor ingresa un comando SQL.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Detectar si es SELECT
                if (sql.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                {
                    var conn = _context.Database.GetDbConnection();
                    conn.Open();

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    using var reader = cmd.ExecuteReader();

                    var dt = new DataTable();
                    dt.Load(reader);
                    dgv.DataSource = dt;

                    conn.Close();
                    MessageBox.Show($"Consulta completada. Filas: {dt.Rows.Count}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    int affected = _context.Database.ExecuteSqlRaw(sql);
                    MessageBox.Show($"Comando ejecutado. Filas afectadas: {affected}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }


}
