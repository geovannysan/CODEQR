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
        private void button2_Click(object sender, EventArgs e)
        {
            var datos = dataGridView1.Rows;
            CodigosRepository repo = new CodigosRepository();

            foreach (DataGridViewRow row in datos)
            {
                // Evita filas nuevas vacías
                if (row.IsNewRow) continue;

                var info = new Entity.Codigos
                {
                    Name = row.Cells.Count > 1 ? row.Cells[1].Value?.ToString() ?? "" : "",
                    Codigo = row.Cells.Count > 0 ? row.Cells[0].Value?.ToString() ?? "" : "",
                    Asiento = row.Cells.Count > 2 ? row.Cells[2].Value?.ToString() ?? "" : "",
                    info = row.Cells.Count > 3 ? row.Cells[3].Value?.ToString() ?? "" : "",
                    EventoID = _ID
                };

                repo.Insert(info,0);
            }
                var lista = repo.GetList(Convert.ToString(_ID)).ToList();
         
            if (lista.Count== 0) {
                this.Close();
                return;
            }
            MessageBox.Show($"Total {lista.Count()} códigos para el evento");
            bool form1Abierto = Application.OpenForms.OfType<EventosIDServer>().Any();
            if (!form1Abierto)
            {
                EventosIDServer eventosIDServer = new EventosIDServer(_ID);
                eventosIDServer.Show();
                this.Close();
                return ;
            }
            else
            {
               // form1Instance.BringToFront();
                //form1Instance.Focus();
                this.Close();
            }
            
        }

        private void Codigos_Load(object sender, EventArgs e)
        {

        }
    }
}
   
