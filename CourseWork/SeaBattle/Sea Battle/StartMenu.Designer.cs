namespace Sea_Battle
{
    partial class StartMenu
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
            this.btnmanman = new System.Windows.Forms.Button();
            this.comboBoxCOMports = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnmanman
            // 
            this.btnmanman.Location = new System.Drawing.Point(320, 218);
            this.btnmanman.Name = "btnmanman";
            this.btnmanman.Size = new System.Drawing.Size(148, 69);
            this.btnmanman.TabIndex = 0;
            this.btnmanman.Text = "Man vs Man";
            this.btnmanman.UseVisualStyleBackColor = true;
            this.btnmanman.Click += new System.EventHandler(this.btnmanman_Click);
            // 
            // comboBoxCOMports
            // 
            this.comboBoxCOMports.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.comboBoxCOMports.FormattingEnabled = true;
            this.comboBoxCOMports.Location = new System.Drawing.Point(320, 98);
            this.comboBoxCOMports.Name = "comboBoxCOMports";
            this.comboBoxCOMports.Size = new System.Drawing.Size(151, 49);
            this.comboBoxCOMports.TabIndex = 1;
            this.comboBoxCOMports.DropDown += new System.EventHandler(this.comboBoxCOMports_DropDown);
            // 
            // StartMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboBoxCOMports);
            this.Controls.Add(this.btnmanman);
            this.Name = "StartMenu";
            this.Text = "StartMenu";
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnmanman;
        private ComboBox comboBoxCOMports;
    }
}