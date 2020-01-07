using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SerialPortLib;
using System.IO.Ports;

namespace ReadQRCodeScanner
{
    public partial class Form1 : Form
    {
        private SerialPortInput serialPort;
        private String valueQR = String.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fillPortList();
            lblStatus.Text = "Disconnect";
        }

        private void fillPortList()
        {
            try
            {
                foreach (String ports in SerialPort.GetPortNames())
                {
                    cListPort.Items.Add(ports);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            closeForm();
            System.Windows.Forms.Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeForm();
        }

        private void closeForm()
        {
            try
            {
                if (serialPort.IsConnected)
                {
                    serialPort.Disconnect();
                }
            }
            catch (Exception e)
            {

            }
            this.Dispose();
        }

        private void bConnect_Click(object sender, EventArgs e)
        {
            connectPort();
        }

        private void connectPort()
        {
            String SP = null;         
            try
            {
                SP = cListPort.SelectedItem.ToString();
                if (SP != "")
                {
                    serialPort = new SerialPortInput();
                    serialPort.SetPort(SP, 9600);
                    serialPort.MessageReceived += SerialPort_MessageReceived;
                    serialPort.Connect();
                    lblStatus.Text = "Connect";
                }
                else
                {
                    MessageBox.Show("Please select COM Port !");
                }
            }
            catch (Exception e)
            {

            }
            
        }

        private void SerialPort_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            
            valueQR = Encoding.ASCII.GetString(args.Data);
            Invoke(new EventHandler(resultScan));
        }

        private void resultScan(object sender, EventArgs e)
        {
            txtValue.Text = "";
            valueQR = valueQR.Replace("\r", "");
            txtValue.Text = valueQR;
        }
    }
}
