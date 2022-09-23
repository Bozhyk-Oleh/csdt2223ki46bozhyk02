using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try
            {
                myserialPort.Open();
            }catch
            {
                MessageBox.Show("Cant open COM port");
            }
        }

        private void onbutton_Click(object sender, EventArgs e)
        {
            if (myserialPort.IsOpen)
            {
                myserialPort.Write("1");
            }

        }

        private void offbutton_Click(object sender, EventArgs e)
        {
            if (myserialPort.IsOpen)
            {
                myserialPort.Write("0");
            }

        }
    }
}
