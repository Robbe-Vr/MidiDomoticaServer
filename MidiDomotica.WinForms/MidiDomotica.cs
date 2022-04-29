using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidiDomotica.WinForms
{
    public partial class MidiDomotica : Form
    {
        public MidiDomotica()
        {
            InitializeComponent();

            MidiDomoticaApi.Startup.PasswordUpdate += (sender, e) =>
            {
                WebApiPasswordTextBox.Text = e.NewPassword;
            };
        }

        public void ToggleForm(bool show)
        {
            if (show)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => { this.Show(); }));
                }
                else Show();
            }
            else
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => { this.Hide(); }));
                }
                else Hide();
            }
        }

        private void UpdateWebApiPasswordButton_Click(object sender, EventArgs e)
        {
            MidiDomoticaApi.Program.SetApiPassword(WebApiPasswordTextBox.Text);
        }

        private void ShowPasswordToggle_Click(object sender, EventArgs e)
        {
            WebApiPasswordTextBox.UseSystemPasswordChar = !WebApiPasswordTextBox.UseSystemPasswordChar;
        }
    }
}
