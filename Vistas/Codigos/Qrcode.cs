using IronBarCode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEWCODES.Vistas.Codigos
{
    public partial class Qrcode : Form
    {
        private string _jpegData;
        public Qrcode(string jpegData)
        {
            InitializeComponent();
            _jpegData = jpegData;
        }

        private void Qrcode_Load(object sender, EventArgs e)
        {
            //byte[] jpegData = _jpegData.ToJpegBinaryData();
            var qrcode = QRCodeWriter.CreateQrCode(_jpegData);
            qrcode.AddBarcodeValueTextBelowBarcode();
           // qrcode.ToJpegBinaryData();
            byte[] jpegData = qrcode.ToJpegBinaryData();
            using (var ms = new MemoryStream(jpegData))
             {
                 Image qrImage = Image.FromStream(ms);
                 pictureBox1.Image = qrImage; // Asignarlo al PictureBox
             }
        }
    }
}
