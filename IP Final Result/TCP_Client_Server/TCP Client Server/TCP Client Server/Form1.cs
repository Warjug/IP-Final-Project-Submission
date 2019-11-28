using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleTCP;

namespace TCP_Client_Server
{
    public partial class Form1 : Form
    {
        List<string> myOrders;

        private void showOrderList()
        {
            listBox1.Items.Clear();
            foreach (string member in myOrders)
            {
                listBox1.Items.Add(member);
            }
        }

        public Form1()
        {
            InitializeComponent();
            myOrders = new List<string>();
            orderCount++;
            label3.Text = "...";
            label7.Text = "1";
            label17.Text = "";
        }
        SimpleTcpServer server;

        private void Form1_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13;//enter
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataReceived;
            server.ClientConnected += Server_ClientConnected;
        }

        private void Server_ClientConnected(object sender, TcpClient e)
        {
            txtStatus.Invoke((MethodInvoker)delegate () {
                txtStatus.Text += "Accepting connection from " + e.Client.RemoteEndPoint + Environment.NewLine;
            });
        }

        public int orderCount;
        public int counter;
        public int IndexHunter(string orderdigit)
        {
            for (int i = 0; i < myOrders.Count; i++)
            {
                if (myOrders[i].Contains(orderdigit))
                {
                    return i;
                }
            }
            return 0;
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate() {
                txtStatus.Text += e.MessageString + Environment.NewLine;
                server.Broadcast(e.TcpClient.Client.RemoteEndPoint + ": " + e.MessageString);
                if ((e.MessageString.ToString().IndexOf("Pizza is Ready") != -1) == true && myOrders.Count!=0)
                {
                    counter++;
                    listBox2.Items.Add(myOrders.ElementAt(IndexHunter(counter.ToString())));
                    myOrders.RemoveAt(IndexHunter(counter.ToString()));
                    showOrderList();
                }

                label3.Text = counter.ToString();
            });
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            txtStatus.Text += "Server starting..." + Environment.NewLine;
            System.Net.IPAddress ip = System.Net.IPAddress.Parse(txtHost.Text);
            try {
                server.Start(ip, Convert.ToInt32(txtPort.Text));
                txtStatus.Text += "Server started" + Environment.NewLine;
            } catch {
                txtStatus.Text += "Server failed to start" + Environment.NewLine;
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (server.IsStarted)
                server.Stop();
        }

        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

        }

        private string toppingsCheck()
        {
            string result = "";
            if (checkBox1.Checked == true)
            {
                result += " ricotta";
            }
            if (checkBox2.Checked == true)
            {
                result += " pineapple";
            }
            if (checkBox3.Checked == true)
            {
                result += " banana";
            }
            if (checkBox3.Checked == true)
            {
                result += " sardines";
            }
            if (checkBox1.Checked == false && checkBox2.Checked == false && checkBox3.Checked == false && checkBox4.Checked == false)
            {
                result += "N/A";
            }
            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                label17.Text = "Please write your name";
            }
            else
            {
                if (comboBox1.SelectedItem == null)
                {
                    label17.Text = "Please select your amount";
                }
                else
                {
                    if (comboBox2.SelectedItem == null)
                    {
                        label17.Text = "Please select a size";
                    }
                    else
                    {
                        if (comboBox3.SelectedItem == null)
                        {
                            label17.Text = "Please select a crust";
                        }
                        else
                        {
                            if (comboBox4.SelectedItem == null)
                            {
                                label17.Text = "Please select a sauce";
                            }
                            else
                            {
                                if (comboBox3.SelectedItem == null)
                                {
                                    label17.Text = "Please select a cheese";
                                }
                                else
                                {
                                    string toppings;
                                    toppings = toppingsCheck();
                                    string Order = "Order #" + orderCount + " " + textBox1.Text + " - " + comboBox1.SelectedItem + " " + comboBox2.SelectedItem + " " + comboBox3.SelectedItem + "-crust w/ " + comboBox4.SelectedItem + " sauce, " + comboBox5.SelectedItem + " cheese, tops:" + toppings;
                                    myOrders.Add(Order);
                                    showOrderList();
                                    orderCount++;
                                    label7.Text = orderCount.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
