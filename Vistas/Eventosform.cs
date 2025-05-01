using Aplicacion.Persistencia;
using Entity;
using MaterialSkin.Controls;
using Microsoft.EntityFrameworkCore;
using NEWCODES.Infraestructura;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NEWCODES.Vistas
{
    public partial class Eventosform : MaterialForm
    {
        // private readonly EevntoContext? context;
        public Eventosform()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
        // EevntoContext eevntoContext = new EevntoContext();
        private async void button1_Click(object sender, EventArgs e)
        {

            //  this.context = new EevntoContext();
            // this.dbContext.Database.EnsureCreated();

            //            this.dbContext.Categories.Load();

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
   string.IsNullOrWhiteSpace(dateTimePicker1.Text) ||
   string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }
            string formato = dateTimePicker1.Text;
            var evento = new Eventos
            {
                Nombre = textBox1.Text,
                Fecha = DateTime.Parse(formato),
                Description = textBox2.Text,
                SelecionLocation = checkBox1.Checked ? 1 : 0
            };
            using (var context = new EevntoContext())
            {

                try
                {
                    var form1 = Application.OpenForms["Form1"] as Form1;
                    context.Eventos.Add(evento);
                    await context.SaveChangesAsync();
                    //Form form = new Form1();
                    form1.LoadData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            Close();

        }

        private void Eventosform_Load(object sender, EventArgs e)
        {

        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {

            //  this.context = new EevntoContext();
            // this.dbContext.Database.EnsureCreated();

            //            this.dbContext.Categories.Load();

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
   string.IsNullOrWhiteSpace(dateTimePicker1.Text) ||
   string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }
            string formato = dateTimePicker1.Text;
            var evento = new Eventos
            {
                Nombre = textBox1.Text,
                Fecha = DateTime.Parse(formato),
                Description = textBox2.Text,
                SelecionLocation = checkBox1.Checked ? 1 : 0
            };
            using (var context = new EevntoContext())
            {

                try
                {
                    var form1 = Application.OpenForms["Form1"] as Form1;
                    context.Eventos.Add(evento);
                    await context.SaveChangesAsync();
                    //Form form = new Form1();
                    form1.LoadData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            Close();
        }
    }
}
