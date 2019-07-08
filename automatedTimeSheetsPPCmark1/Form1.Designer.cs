namespace automatedTimeSheetsPPCmark1
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
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEhubIDnum = new System.Windows.Forms.TextBox();
            this.txtEhubPassword = new System.Windows.Forms.TextBox();
            this.btnDeleteDataFromConfigFile = new System.Windows.Forms.Button();
            this.btnSubmitTimeSheets = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSendEmail = new System.Windows.Forms.TextBox();
            this.txtCCEmail = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtOutput
            // 
            this.txtOutput.Enabled = false;
            this.txtOutput.Location = new System.Drawing.Point(17, 120);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(1222, 585);
            this.txtOutput.TabIndex = 0;
            this.txtOutput.Text = "";
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.Yellow;
            this.btnSubmit.Location = new System.Drawing.Point(470, 28);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(206, 70);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "Save Your User Settings";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "PPC ID NUMBER: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "E-HUB PASSWORD: ";
            // 
            // txtEhubIDnum
            // 
            this.txtEhubIDnum.Location = new System.Drawing.Point(159, 13);
            this.txtEhubIDnum.Name = "txtEhubIDnum";
            this.txtEhubIDnum.Size = new System.Drawing.Size(166, 20);
            this.txtEhubIDnum.TabIndex = 6;
            // 
            // txtEhubPassword
            // 
            this.txtEhubPassword.Location = new System.Drawing.Point(159, 41);
            this.txtEhubPassword.Name = "txtEhubPassword";
            this.txtEhubPassword.PasswordChar = '*';
            this.txtEhubPassword.Size = new System.Drawing.Size(166, 20);
            this.txtEhubPassword.TabIndex = 8;
            // 
            // btnDeleteDataFromConfigFile
            // 
            this.btnDeleteDataFromConfigFile.BackColor = System.Drawing.Color.Red;
            this.btnDeleteDataFromConfigFile.Location = new System.Drawing.Point(1033, 712);
            this.btnDeleteDataFromConfigFile.Name = "btnDeleteDataFromConfigFile";
            this.btnDeleteDataFromConfigFile.Size = new System.Drawing.Size(201, 22);
            this.btnDeleteDataFromConfigFile.TabIndex = 9;
            this.btnDeleteDataFromConfigFile.Text = "Delete Saved User Settings";
            this.btnDeleteDataFromConfigFile.UseVisualStyleBackColor = false;
            this.btnDeleteDataFromConfigFile.Click += new System.EventHandler(this.btnDeleteDataFromConfigFile_Click);
            // 
            // btnSubmitTimeSheets
            // 
            this.btnSubmitTimeSheets.BackColor = System.Drawing.Color.Lime;
            this.btnSubmitTimeSheets.Location = new System.Drawing.Point(851, 12);
            this.btnSubmitTimeSheets.Name = "btnSubmitTimeSheets";
            this.btnSubmitTimeSheets.Size = new System.Drawing.Size(383, 102);
            this.btnSubmitTimeSheets.TabIndex = 10;
            this.btnSubmitTimeSheets.Text = "Submit Time Sheets";
            this.btnSubmitTimeSheets.UseVisualStyleBackColor = false;
            this.btnSubmitTimeSheets.Click += new System.EventHandler(this.btnSubmitTimeSheets_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Send Email To: ";
            // 
            // txtSendEmail
            // 
            this.txtSendEmail.Location = new System.Drawing.Point(159, 67);
            this.txtSendEmail.Name = "txtSendEmail";
            this.txtSendEmail.Size = new System.Drawing.Size(291, 20);
            this.txtSendEmail.TabIndex = 12;
            // 
            // txtCCEmail
            // 
            this.txtCCEmail.Location = new System.Drawing.Point(159, 94);
            this.txtCCEmail.Name = "txtCCEmail";
            this.txtCCEmail.Size = new System.Drawing.Size(291, 20);
            this.txtCCEmail.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "CC (Yourself Is Suggested): ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 746);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCCEmail);
            this.Controls.Add(this.txtSendEmail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSubmitTimeSheets);
            this.Controls.Add(this.btnDeleteDataFromConfigFile);
            this.Controls.Add(this.txtEhubPassword);
            this.Controls.Add(this.txtEhubIDnum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtOutput);
            this.Name = "Form1";
            this.Text = "Time Sheet Generator 1.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEhubIDnum;
        private System.Windows.Forms.TextBox txtEhubPassword;
        private System.Windows.Forms.Button btnDeleteDataFromConfigFile;
        private System.Windows.Forms.Button btnSubmitTimeSheets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSendEmail;
        private System.Windows.Forms.TextBox txtCCEmail;
        private System.Windows.Forms.Label label4;
    }
}

