using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MaterialSkin.Controls;
using NEWCODES.Infraestructura.Persistencia;
using System.Data;

namespace NEWCODES.Vistas.Codigos
{
    public partial class CodigosView :MaterialForm
    {
        private int _ID;
        public CodigosView(int id)
        {
            InitializeComponent();
            _ID = id;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file
            if (file.ShowDialog() == DialogResult.OK) //if there is a file chosen by the user
            {
                string fileExt = Path.GetExtension(file.FileName); //get the file extension
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                    try
                    {
                        DataTable dtExcel = ReadExcel(file.FileName); //read excel file
                        dataGridView1.Visible = true;
                        dataGridView1.DataSource = dtExcel;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error
                }
            }
        }
        private DataTable ReadExcel(string fileName)
        {
            DataTable dataTable = new DataTable();

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = doc.WorkbookPart;
                Sheet sheet = workbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                var rows = sheetData.Elements<Row>();

                bool isHeader = true;

                foreach (Row row in rows)
                {
                    var rowValues = new List<string>();

                    foreach (Cell cell in row.Elements<Cell>())
                    {
                        rowValues.Add(GetCellValue(doc, cell));
                    }

                    if (isHeader)
                    {
                        foreach (string header in rowValues)
                        {
                            dataTable.Columns.Add(header);
                        }
                        isHeader = false;
                    }
                    else
                    {
                        dataTable.Rows.Add(rowValues.ToArray());
                    }
                }
            }

            return dataTable;
        }
        private string GetCellValue(SpreadsheetDocument doc, Cell cell)
        {
            if (cell == null || cell.CellValue == null)
                return string.Empty;

            string value = cell.CellValue.InnerText;

            // Check if the cell has a shared string
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable
                    .Elements<SharedStringItem>().ElementAt(int.Parse(value)).InnerText;
            }

            return value;
        }

        private EventosIDServer form1Instance;
        //private Form2 form2Instance;
        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos en la tabla.");
                    return;
                }

                CodigosRepository repo = new CodigosRepository();
                var nuevosCodigos = new List<Entity.Codigos>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    string codigo = row.Cells.Count > 0 ? row.Cells[0].Value?.ToString()?.Trim() ?? "" : "";
                    string name = row.Cells.Count > 1 ? row.Cells[1].Value?.ToString()?.Trim() ?? "" : "";
                    string asiento = row.Cells.Count > 2 ? row.Cells[2].Value?.ToString()?.Trim() ?? "" : "";
                    string info = row.Cells.Count > 3 ? row.Cells[3].Value?.ToString()?.Trim() ?? "" : "";

                    if (string.IsNullOrWhiteSpace(codigo))
                        continue;

                    nuevosCodigos.Add(new Entity.Codigos
                    {
                        Codigo = codigo,
                        Name = name,
                        Asiento = asiento,
                        info = info,
                        EventoID = _ID,
                        time = DateTime.Now
                    });
                }

                if (nuevosCodigos.Count == 0)
                {
                    MessageBox.Show("No hay códigos válidos para insertar.");
                    return;
                }

                // 🔹 Insertar por lotes, evitando duplicados
                await repo.InsertBatchAsync(nuevosCodigos);

                // 🔹 Ver cuántos quedaron finalmente en la base
                var lista = await repo.GetListAsync(_ID);
                MessageBox.Show($"✅ Total de códigos en el evento: {lista.Count}");

                // 🔹 Mostrar o actualizar ventana de evento
                bool formAbierto = Application.OpenForms.OfType<EventosIDServer>().Any();
                if (!formAbierto)
                {
                    EventosIDServer eventosIDServer = new EventosIDServer(_ID);
                    eventosIDServer.Show();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error al insertar: " + ex.Message);
            }

        }

        private void Codigos_Load(object sender, EventArgs e)
        {

        }
    }
}
   
