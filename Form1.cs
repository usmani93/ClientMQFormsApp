using System;
using RabbitMQ.Client;
using System.Windows.Forms;
using RabbitMQ.Client.Events;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Image = System.Drawing.Image;
using ClientLibrary;

namespace RabbitMQClient
{
    public partial class Form1 : Form
    {
        private delegate void SafeCallDelegate(MemoryStream imageStream); 

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "192.168.0.103";
        }

        void Connect()
        {
            try
            {
                Receive receive = new Receive();
                receive.SetupConnection(textBox1.Text);
                receive.ReceiveMessageInitialize("hello");
                receive.DataReceived += GetSetImage;
            }
            catch(Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Connect();
            }
            catch(Exception ex)
            {

            }
        }

        private void GetSetImage(string message)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    var imageBuffer = Convert.FromBase64String(message);
                    ms.Write(imageBuffer, 0, imageBuffer.Length);
                    SetImageInPictureBox(ms);
                }
            }
            catch(Exception ex)
            {

            }
        }

        public void SetImageInPictureBox(MemoryStream imageStream)
        {
            if (pictureBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(SetImageInPictureBox);
                pictureBox1.Invoke(d, new object[] { imageStream });
            }
            else
            {
                pictureBox1.Image = Image.FromStream(imageStream);
            }
        }
    }
}
