using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleTCP;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            serialPort1.Open();
            
        }

        SimpleTcpClient client;
        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            //InvokeRequired required compares the thread ID of the
            //calling thread to the tread ID of the creating thead.
            //If these threads are different, it returns true. 
            if (this.txtMessage.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });

            }
            else
            {
                this.txtMessage.Text = text;
            }
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            txtHost.Enabled = false;
            txtPort.Enabled = false; 

            int port = Convert.ToInt32(txtPort.Text);

            try {
                client.Connect(txtHost.Text, port);
            } catch {
                btnConnect.Enabled = true;
                txtHost.Enabled = true;
                txtPort.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e) {
            txtStatus.Invoke((MethodInvoker)delegate () {
                txtStatus.Text += e.MessageString + Environment.NewLine;             
            });
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            client.WriteLineAndGetReply(txtMessage.Text, TimeSpan.FromSeconds(0));
        }

        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string txt = null;
            txt += serialPort1.ReadExisting().ToString();          
            
            if (txt.StartsWith("Pizza"))
            {
                btnSend.Invoke((MethodInvoker)delegate ()
                {
                    txtMessage.Text = "Pizza is Ready!";
                    btnSend.PerformClick();
                });
            }
            else
            {
                SetText(txt.ToString());
            }

        }
    }
}
    
