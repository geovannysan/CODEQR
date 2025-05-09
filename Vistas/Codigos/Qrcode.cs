using QRCoder;

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
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(_jpegData, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            pictureBox1.Image = qrCodeImage;
        }
    }
}
