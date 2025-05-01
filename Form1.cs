using MaterialSkin;
using MaterialSkin.Controls;
using NEWCODES.Infraestructura.Persistencia;
using NEWCODES.Vistas;
using NEWCODES.Vistas.Codigos;

namespace NEWCODES
{
    public partial class Form1 : MaterialForm
    {
        private MaterialSkinManager materialSkinManager;
        public Form1()
        {
            InitializeComponent();
           // this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            this.MaximizeBox = false;
            materialSkinManager = MaterialSkinManager.Instance;
            // materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue600, Primary.Blue700,
                Primary.Blue200, Accent.LightBlue200,
                TextShade.WHITE
            );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Eventosform form = new Eventosform();

            form.Show();
            //this.Hide();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //   this.OnLoad(e);
            try
            {
                if (dataGridView1.Columns["Accion"] == null)
                {
                    DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn();
                    btnEditar.Name = "Editar";
                    btnEditar.HeaderText = "Editar";
                    btnEditar.Text = "Editar";
                    btnEditar.UseColumnTextForButtonValue = true;

                    DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
                    btnEliminar.Name = "Eliminar";
                    btnEliminar.HeaderText = "Eliminar";
                    btnEliminar.Text = "Eliminar";
                    btnEliminar.UseColumnTextForButtonValue = true;


                    DataGridViewButtonColumn btnEliminarIniciar = new DataGridViewButtonColumn();
                    btnEliminarIniciar.Name = "Accion";
                    btnEliminarIniciar.HeaderText = "Accion";
                    btnEliminarIniciar.Text = "Accion";
                    btnEliminarIniciar.UseColumnTextForButtonValue = true;
                    dataGridView1.Columns.Add(btnEditar);
                    dataGridView1.Columns.Add(btnEliminar);
                    dataGridView1.Columns.Add(btnEliminarIniciar);
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Erorr", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        public void LoadData()
        {
            EventosRepository evento = new EventosRepository();
            var students = evento.GetAll();
            dataGridView1.Rows.Clear();
            Console.WriteLine(students);
            foreach (var student in students)
            {
                var estado = Convert.ToInt32(student.SelecionLocation) == 0 ? "Permitir Acceso" : "Autorizar Acceso";
                dataGridView1.Rows.Add(student.Id, student.Nombre, student.Description, student.Fecha, estado);
            }
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var column = dataGridView1.Columns[e.ColumnIndex];

                if (column.Name == "Eliminar")
                {
                    // Obtener el ID de la fila
                    var id = dataGridView1.Rows[e.RowIndex].Cells["Id"].Value?.ToString();
                    // MessageBox.Show($"Eliminar fila con ID: {id}");
                    DialogResult result = MessageBox.Show("¿Estás seguro que deseas eliminar este registro?", "Confirmar", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.OK)
                    {
                        EventosRepository studentManager = new EventosRepository();
                        var students = studentManager.Delete(id);
                        LoadData();
                        // Usuario confirmó
                        MessageBox.Show("Registro eliminado.");
                    }
                    // Aquí podés llamar a tu repositorio para eliminar y luego refrescar:
                    // EventosRepository.DeleteById(id);
                    // LoadData();
                }
                else if (column.Name == "Editar")
                {
                    var id = dataGridView1.Rows[e.RowIndex].Cells["Id"].Value?.ToString();
                    MessageBox.Show($"Editar fila con ID: {id}");
                    // Abrí un formulario para editar o lo que necesites
                }
                else if (column.Name == "Accion")
                {
                    var id = dataGridView1.Rows[e.RowIndex].Cells["Id"].Value?.ToString();
                    CodigosRepository codigosRepository = new CodigosRepository();
                    Console.Write(id);
                    var codigos = codigosRepository.Get(id);
                    Console.WriteLine(codigos);
                    if (codigos != null)
                    {
                        EventosIDServer codigos1 = new EventosIDServer(Convert.ToInt32(id));
                        codigos1.ShowDialog();
                    }
                    else
                    {
                        Codigos codigos1 = new Codigos(Convert.ToInt32(id));
                        codigos1.ShowDialog();
                    }

                    //MessageBox.Show($"Eliminar fila con ID: {id}");
                    // Abrí un formulario para editar o lo que necesites
                }
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Cancelar el cierre si querés evitarlo (por ejemplo, desactivar la X)
            e.Cancel = true;
            DialogResult result = MessageBox.Show("¿Estás seguro que deseas cerrar el Programa?", "INFO", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                e.Cancel = false;

            }
        }
        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CodeEvente.db");
        private void button2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // EevntoContext().Database.GetDbConnection().Close();

                saveFileDialog.Filter = "Archivo SQLite (*.db)|*.db";
                saveFileDialog.Title = "Exportar base de datos";
                saveFileDialog.FileName = "backup.db";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.Copy(dbPath, saveFileDialog.FileName, true);
                        MessageBox.Show("Base de datos exportada correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al exportar: " + ex.Message);
                    }
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivo SQLite (*.db)|*.db";
                openFileDialog.Title = "Importar base de datos";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Cerrar cualquier contexto anterior si es necesario (no crearlo antes)
                        // Copiar el archivo nuevo de la base de datos
                        File.Copy(openFileDialog.FileName, dbPath, true);

                        MessageBox.Show("Base de datos importada correctamente. par aver lso cambios debe reiniciar la aplicación");

                        // 🔄 Crear un nuevo contexto después de importar
                        Application.Restart();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al importar: " + ex.Message);
                    }
                }
            }
        }

        private void materialCard1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
