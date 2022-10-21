using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace Client
{
    public partial class Form1 : Form
    {

        static DateTime recv_glob = DateTime.Now;
        string mcu_message = string.Empty;
        bool Autoscroll = false;

        MySqlConnection mySqlConnection;
        string mycommandsql = string.Empty;
        string myConnectionString = "Database=test; Data Source=192.168.0.103;User Id=root;Password=gahava01";
        public Form1()
        {
            InitializeComponent();
            mySqlConnection = new MySqlConnection(myConnectionString);
        }

        private void sendbutton_Click(object sender, EventArgs e)
        {
            if (myserialPort.IsOpen)
            {
                if (SendtextBox.Text.Length <= 30)
                {
                    mySqlConnection.Open();
                    if (mySqlConnection.Ping())
                    {
                        DateTime datesent = DateTime.Now;
                    myserialPort.Write(SendtextBox.Text);
                    CommunicationtextBox.Text += datesent + "(sended): " + SendtextBox.Text + Environment.NewLine;
                    
                    mycommandsql = "INSERT INTO test.data (datain, timein) " +
                   "VALUES ('" + SendtextBox.Text + "'," +
                   " '" + datesent.Year + "-" + datesent.Month + "-" + datesent.Day + " " + datesent.TimeOfDay + "');";

                    
                   
                        // mySqlConnection.Open();
                        MySqlCommand commandsql = new MySqlCommand(mycommandsql, mySqlConnection);
                        commandsql.ExecuteNonQuery();
                        mySqlConnection.Close();

                    }
                    else
                    {
                        MessageBox.Show("Can't open db");
                    }
                }
                else
                {
                    MessageBox.Show("Message must be less than 31 characters");
                }

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
            mcu_message = myserialPort.ReadLine();
            this.Invoke(new EventHandler(dispalytext));
        }

        private void dispalytext(object sender, EventArgs e)
        {
            CommunicationtextBox.Text += recv_glob + "(received): " + mcu_message + Environment.NewLine;
            int id_data;
            string mySelectQuery = "SELECT max(id_data) FROM test.data;";
            mySqlConnection.Open();
            MySqlDataReader myReader;
            MySqlCommand readsql = new MySqlCommand(mySelectQuery, mySqlConnection);
            myReader = readsql.ExecuteReader();
            myReader.Read();
            id_data = myReader.GetInt16(0);
            myReader.Close();
            mycommandsql = "UPDATE test.`data` SET dataout = '" + mcu_message + "'," +
             " timeout = '" + recv_glob.Year + "-" + recv_glob.Month + "-" + recv_glob.Day + " " + recv_glob.TimeOfDay + "' " +
             "WHERE (id_data = '" + id_data + "');";

            MySqlCommand commandsql = new MySqlCommand(mycommandsql, mySqlConnection);
            commandsql.ExecuteNonQuery();
            mySqlConnection.Close();
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

        private void ToggleAutoscroll_Click(object sender, EventArgs e)
        {
            Autoscroll = !Autoscroll;
            if (Autoscroll)
            {
                ToggleAutoscroll.BackColor = Color.Aqua;
            }
            else
            {
                ToggleAutoscroll.BackColor = Color.Transparent;
            }
        }

        private void CommunicationtextBox_TextChanged(object sender, EventArgs e)
        {
            if (Autoscroll)
            {
                CommunicationtextBox.SelectionStart = CommunicationtextBox.Text.Length;
                CommunicationtextBox.ScrollToCaret();
            }
        }
    }
}
