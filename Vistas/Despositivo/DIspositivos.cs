using Entity;
using MaterialSkin.Controls;
using NEWCODES.Infraestructura.Persistencia;

namespace NEWCODES.Vistas.Despositivo
{
    public partial class DIspositivosF : MaterialForm
    {
        private int _ID;
        private int _IDDis;
        public DIspositivosF(int id, int idis)
        {
            InitializeComponent();
            _ID = id;
            _IDDis = idis;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void DIspositivos_Load(object sender, EventArgs e)
        {
            /*   if (datagridLocalidades.Columns["Accion"] == null) {
                   DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                   checkBoxColumn.Name = "Seleccionar";
                   checkBoxColumn.HeaderText = "Seleccionar";
                   checkBoxColumn.Width = 100;
                   checkBoxColumn.TrueValue = true;
                   checkBoxColumn.FalseValue = false;
                   datagridLocalidades.Columns.Add(checkBoxColumn);
            }*/
            CargaLocalid();
        }
        private void CargaLocalid()
        {
            LocalidadesRepository localidad = new LocalidadesRepository();

            var data = localidad.GetInfolocali(Convert.ToString(_ID), Convert.ToString(_IDDis));
            datagridLocalidad.Rows.Clear();
            foreach (var item in data)
            {
                var ite = item.DispoId == 0 ? false : true;
                datagridLocalidad.Rows.Add(ite, item.Name, item.LocalidadID, item.DispoId);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var datos = datagridLocalidad.Rows;
            foreach (DataGridViewRow item in datos)
            {
                DispositivoLocalidadRepositoriy dispositivoLocal = new DispositivoLocalidadRepositoriy();
                if (Convert.ToBoolean(item.Cells[0].Value.ToString()) && int.Parse(item.Cells[3].Value.ToString()) == 0)
                {
                    dispositivoLocal.Insert(new DispositivoLocation
                    {
                        Name = item.Cells[1].Value.ToString() ?? "",
                        LocalidadID = int.Parse(item.Cells[2].Value.ToString()),
                        DispoId = _IDDis
                    });
                }
                else if (!Convert.ToBoolean(item.Cells[0].Value.ToString()))
                {
                    dispositivoLocal.Delete(item.Cells[3].Value.ToString());
                }
            }
            CargaLocalid();
            MessageBox.Show("PErmisos Actualizados");
            this.Close();

        }

        private void datagridLocalidad_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            var datos = datagridLocalidad.Rows;
            foreach (DataGridViewRow item in datos)
            {
                DispositivoLocalidadRepositoriy dispositivoLocal = new DispositivoLocalidadRepositoriy();
                if (Convert.ToBoolean(item.Cells[0].Value.ToString()) && int.Parse(item.Cells[3].Value.ToString()) == 0)
                {
                    dispositivoLocal.Insert(new DispositivoLocation
                    {
                        Name = item.Cells[1].Value.ToString() ?? "",
                        LocalidadID = int.Parse(item.Cells[2].Value.ToString()),
                        DispoId = _IDDis
                    });
                }
                else if (!Convert.ToBoolean(item.Cells[0].Value.ToString()))
                {
                    dispositivoLocal.Delete(item.Cells[3].Value.ToString());
                }
            }
            CargaLocalid();
            MessageBox.Show("PErmisos Actualizados");
            this.Close();


        }
    }
}
