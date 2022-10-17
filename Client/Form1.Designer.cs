namespace Client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.myserialPort = new System.IO.Ports.SerialPort(this.components);
            this.sendbutton = new System.Windows.Forms.Button();
            this.SendtextBox = new System.Windows.Forms.TextBox();
            this.CommunicationtextBox = new System.Windows.Forms.TextBox();
            this.СommunicationBoxLabel = new System.Windows.Forms.Label();
            this.Sendmessagelabel = new System.Windows.Forms.Label();
            this.COMcomboBox = new System.Windows.Forms.ComboBox();
            this.openCOM = new System.Windows.Forms.Button();
            this.closeCOM = new System.Windows.Forms.Button();
            this.ToggleAutoscroll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // myserialPort
            // 
            this.myserialPort.PortName = "COM4";
            this.myserialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.myserialPort_DataReceived);
            // 
            // sendbutton
            // 
            this.sendbutton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.sendbutton.Location = new System.Drawing.Point(621, 172);
            this.sendbutton.Name = "sendbutton";
            this.sendbutton.Size = new System.Drawing.Size(137, 44);
            this.sendbutton.TabIndex = 0;
            this.sendbutton.Text = "SEND";
            this.sendbutton.UseVisualStyleBackColor = true;
            this.sendbutton.Click += new System.EventHandler(this.sendbutton_Click);
            // 
            // SendtextBox
            // 
            this.SendtextBox.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SendtextBox.Location = new System.Drawing.Point(67, 172);
            this.SendtextBox.Name = "SendtextBox";
            this.SendtextBox.Size = new System.Drawing.Size(505, 44);
            this.SendtextBox.TabIndex = 1;
            this.SendtextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendtextBox_KeyDown);
            // 
            // CommunicationtextBox
            // 
            this.CommunicationtextBox.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CommunicationtextBox.Location = new System.Drawing.Point(67, 260);
            this.CommunicationtextBox.Multiline = true;
            this.CommunicationtextBox.Name = "CommunicationtextBox";
            this.CommunicationtextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CommunicationtextBox.Size = new System.Drawing.Size(691, 199);
            this.CommunicationtextBox.TabIndex = 2;
            this.CommunicationtextBox.TextChanged += new System.EventHandler(this.CommunicationtextBox_TextChanged);
            // 
            // СommunicationBoxLabel
            // 
            this.СommunicationBoxLabel.AutoSize = true;
            this.СommunicationBoxLabel.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.СommunicationBoxLabel.Location = new System.Drawing.Point(62, 229);
            this.СommunicationBoxLabel.Name = "СommunicationBoxLabel";
            this.СommunicationBoxLabel.Size = new System.Drawing.Size(198, 28);
            this.СommunicationBoxLabel.TabIndex = 3;
            this.СommunicationBoxLabel.Text = "СommunicationBox";
            // 
            // Sendmessagelabel
            // 
            this.Sendmessagelabel.AutoSize = true;
            this.Sendmessagelabel.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Sendmessagelabel.Location = new System.Drawing.Point(62, 139);
            this.Sendmessagelabel.Name = "Sendmessagelabel";
            this.Sendmessagelabel.Size = new System.Drawing.Size(147, 28);
            this.Sendmessagelabel.TabIndex = 4;
            this.Sendmessagelabel.Text = "Send message";
            // 
            // COMcomboBox
            // 
            this.COMcomboBox.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.COMcomboBox.FormattingEnabled = true;
            this.COMcomboBox.Location = new System.Drawing.Point(329, 41);
            this.COMcomboBox.Name = "COMcomboBox";
            this.COMcomboBox.Size = new System.Drawing.Size(104, 35);
            this.COMcomboBox.TabIndex = 5;
            this.COMcomboBox.DropDown += new System.EventHandler(this.COMcomboBox_DropDown);
            // 
            // openCOM
            // 
            this.openCOM.Font = new System.Drawing.Font("Cambria", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.openCOM.Location = new System.Drawing.Point(480, 39);
            this.openCOM.Name = "openCOM";
            this.openCOM.Size = new System.Drawing.Size(92, 37);
            this.openCOM.TabIndex = 6;
            this.openCOM.Text = "OPEN";
            this.openCOM.UseVisualStyleBackColor = true;
            this.openCOM.Click += new System.EventHandler(this.openCOM_Click);
            // 
            // closeCOM
            // 
            this.closeCOM.Font = new System.Drawing.Font("Cambria", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.closeCOM.Location = new System.Drawing.Point(633, 39);
            this.closeCOM.Name = "closeCOM";
            this.closeCOM.Size = new System.Drawing.Size(85, 37);
            this.closeCOM.TabIndex = 7;
            this.closeCOM.Text = "CLOSE";
            this.closeCOM.UseVisualStyleBackColor = true;
            this.closeCOM.Click += new System.EventHandler(this.closeCOM_Click);
            // 
            // ToggleAutoscroll
            // 
            this.ToggleAutoscroll.Location = new System.Drawing.Point(297, 229);
            this.ToggleAutoscroll.Name = "ToggleAutoscroll";
            this.ToggleAutoscroll.Size = new System.Drawing.Size(88, 27);
            this.ToggleAutoscroll.TabIndex = 8;
            this.ToggleAutoscroll.Text = "Autoscroll";
            this.ToggleAutoscroll.UseVisualStyleBackColor = true;
            this.ToggleAutoscroll.Click += new System.EventHandler(this.ToggleAutoscroll_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 486);
            this.Controls.Add(this.ToggleAutoscroll);
            this.Controls.Add(this.closeCOM);
            this.Controls.Add(this.openCOM);
            this.Controls.Add(this.COMcomboBox);
            this.Controls.Add(this.Sendmessagelabel);
            this.Controls.Add(this.СommunicationBoxLabel);
            this.Controls.Add(this.CommunicationtextBox);
            this.Controls.Add(this.SendtextBox);
            this.Controls.Add(this.sendbutton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort myserialPort;
        private System.Windows.Forms.Button sendbutton;
        private System.Windows.Forms.TextBox SendtextBox;
        private System.Windows.Forms.TextBox CommunicationtextBox;
        private System.Windows.Forms.Label СommunicationBoxLabel;
        private System.Windows.Forms.Label Sendmessagelabel;
        private System.Windows.Forms.ComboBox COMcomboBox;
        private System.Windows.Forms.Button openCOM;
        private System.Windows.Forms.Button closeCOM;
        private System.Windows.Forms.Button ToggleAutoscroll;
    }
}

