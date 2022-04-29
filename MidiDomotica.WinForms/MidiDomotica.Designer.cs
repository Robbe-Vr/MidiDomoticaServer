
namespace MidiDomotica.WinForms
{
    partial class MidiDomotica
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
            this.UpdateWebApiPasswordButton = new System.Windows.Forms.Button();
            this.WebApiPasswordTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ShowPasswordToggle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UpdateWebApiPasswordButton
            // 
            this.UpdateWebApiPasswordButton.Location = new System.Drawing.Point(3, 72);
            this.UpdateWebApiPasswordButton.Name = "UpdateWebApiPasswordButton";
            this.UpdateWebApiPasswordButton.Size = new System.Drawing.Size(75, 23);
            this.UpdateWebApiPasswordButton.TabIndex = 0;
            this.UpdateWebApiPasswordButton.Text = "Update";
            this.UpdateWebApiPasswordButton.UseVisualStyleBackColor = true;
            this.UpdateWebApiPasswordButton.Click += new System.EventHandler(this.UpdateWebApiPasswordButton_Click);
            // 
            // WebApiPasswordTextBox
            // 
            this.WebApiPasswordTextBox.Location = new System.Drawing.Point(3, 43);
            this.WebApiPasswordTextBox.Name = "WebApiPasswordTextBox";
            this.WebApiPasswordTextBox.Size = new System.Drawing.Size(146, 23);
            this.WebApiPasswordTextBox.TabIndex = 1;
            this.WebApiPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Update Web Api Password";
            // 
            // ShowPasswordToggle
            // 
            this.ShowPasswordToggle.Location = new System.Drawing.Point(155, 43);
            this.ShowPasswordToggle.Name = "ShowPasswordToggle";
            this.ShowPasswordToggle.Size = new System.Drawing.Size(44, 23);
            this.ShowPasswordToggle.TabIndex = 3;
            this.ShowPasswordToggle.Text = "show";
            this.ShowPasswordToggle.UseVisualStyleBackColor = true;
            this.ShowPasswordToggle.Click += new System.EventHandler(this.ShowPasswordToggle_Click);
            // 
            // MidiDomotica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ShowPasswordToggle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.WebApiPasswordTextBox);
            this.Controls.Add(this.UpdateWebApiPasswordButton);
            this.Name = "MidiDomotica";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UpdateWebApiPasswordButton;
        private System.Windows.Forms.TextBox WebApiPasswordTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ShowPasswordToggle;
    }
}

