using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Client
{
    public partial class Form1 : Form
    {

        static DateTime recv_glob = DateTime.Now;
        string mcu_message = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void sendbutton_Click(object sender, EventArgs e)
        {
            if (myserialPort.IsOpen)
            {
                DateTime daterecv = DateTime.Now;
                myserialPort.Write(SendtextBox.Text);
                CommunicationtextBox.Text += daterecv + "(sended): " + SendtextBox.Text + Environment.NewLine;

            }
            else
            {
                MessageBox.Show("COM port wasn't opened");
            }
            SendtextBox.Text = string.Empty;
        }


        private void myserialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            recv_glob = DateTime.Now;
            mcu_message = myserialPort.ReadExisting();
            this.Invoke(new EventHandler(dispalytext));
        }

        private void dispalytext(object sender, EventArgs e)
        {
            CommunicationtextBox.Text += recv_glob + "(received): " + mcu_message + Environment.NewLine;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            myserialPort.Close();
        }

        private void openCOM_Click(object sender, EventArgs e)
        {
            try
            {
                myserialPort.PortName = COMcomboBox.Text;
                myserialPort.Open();
                MessageBox.Show("COM port was opened");
            }
            catch
            {
                MessageBox.Show("Can't open COM port. " +
                    "Access is denied to the port. " +
                    "-or- " +
                    "The current process, or another process on the system," +
                    " already has the specified COM port open");
            }
        }

        private void closeCOM_Click(object sender, EventArgs e)
        {
            try
            {
                myserialPort.Close();
                MessageBox.Show("COM port was closed");

            }
            catch
            {
                MessageBox.Show("Can't close COM port");
            }
        }

        private void COMcomboBox_DropDown(object sender, EventArgs e)
        {
            string[] portList = SerialPort.GetPortNames();
            COMcomboBox.Items.Clear();
            COMcomboBox.Items.AddRange(portList);

        }

        private void SendtextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                sendbutton.PerformClick();
            }
        }
    }
}
