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
            this.onbutton = new System.Windows.Forms.Button();
            this.offbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // myserialPort
            // 
            this.myserialPort.PortName = "COM3";
            // 
            // onbutton
            // 
            this.onbutton.Location = new System.Drawing.Point(247, 119);
            this.onbutton.Name = "onbutton";
            this.onbutton.Size = new System.Drawing.Size(217, 101);
            this.onbutton.TabIndex = 0;
            this.onbutton.Text = "on";
            this.onbutton.UseVisualStyleBackColor = true;
            this.onbutton.Click += new System.EventHandler(this.onbutton_Click);
            // 
            // offbutton
            // 
            this.offbutton.Location = new System.Drawing.Point(247, 245);
            this.offbutton.Name = "offbutton";
            this.offbutton.Size = new System.Drawing.Size(217, 89);
            this.offbutton.TabIndex = 1;
            this.offbutton.Text = "off";
            this.offbutton.UseVisualStyleBackColor = true;
            this.offbutton.Click += new System.EventHandler(this.offbutton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.offbutton);
            this.Controls.Add(this.onbutton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort myserialPort;
        private System.Windows.Forms.Button onbutton;
        private System.Windows.Forms.Button offbutton;
    }
}

