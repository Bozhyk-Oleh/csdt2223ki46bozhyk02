using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Sea_Battle
{
    public partial class StartMenu : Form
    {
        private SerialPort myserialPort = new SerialPort();
        public StartMenu()
        {
            InitializeComponent();
        }

        private void btnmanman_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxCOMports.Text != "")
                    myserialPort.PortName = comboBoxCOMports.Text;
                else
                {
                    MessageBox.Show("Choose a COM port");
                    return;
                }
                myserialPort.Open();
                myserialPort.Close();
                this.Hide();
                ArrangeShips pl2arrangeships = new ArrangeShips(myserialPort);
                pl2arrangeships.Show();
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

        private void comboBoxCOMports_DropDown(object sender, EventArgs e)
        {
            string[] portList = SerialPort.GetPortNames();
            comboBoxCOMports.Items.Clear();
            comboBoxCOMports.Items.AddRange(portList);
        }

    }
}
