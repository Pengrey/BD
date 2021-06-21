
namespace Trabalho_form
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.text_UserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.text_Password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Submit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // text_UserName
            // 
            this.text_UserName.Location = new System.Drawing.Point(135, 63);
            this.text_UserName.Name = "text_UserName";
            this.text_UserName.Size = new System.Drawing.Size(229, 23);
            this.text_UserName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "UserName";
            // 
            // text_Password
            // 
            this.text_Password.Location = new System.Drawing.Point(135, 109);
            this.text_Password.Name = "text_Password";
            this.text_Password.PasswordChar = '*';
            this.text_Password.Size = new System.Drawing.Size(228, 23);
            this.text_Password.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // btn_Submit
            // 
            this.btn_Submit.Location = new System.Drawing.Point(178, 160);
            this.btn_Submit.Name = "btn_Submit";
            this.btn_Submit.Size = new System.Drawing.Size(75, 23);
            this.btn_Submit.TabIndex = 4;
            this.btn_Submit.Text = "Login";
            this.btn_Submit.Click += new System.EventHandler(this.btn_Submit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 230);
            this.Controls.Add(this.btn_Submit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.text_Password);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.text_UserName);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text_UserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Submit;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.TextBox text_Password;
    }
}

