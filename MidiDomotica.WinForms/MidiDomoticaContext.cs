using Microsoft.Extensions.Hosting;
using MidiDomotica.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SYWCentralLogging;

namespace MidiDomotica.WinForms
{
    class MidiDomoticaContext : ApplicationContext
    {
        private MidiDomotica form;
        private Thread Api;

        private MidiManager manager;

        private NotifyIcon TrayIcon;
        private ContextMenuStrip TrayIconContextMenu;
        private ToolStripMenuItem FormToggleItem;
        private ToolStripMenuItem CloseMenuItem;

        public MidiDomoticaContext(string[] args)
        {
            try
            {
                Logger.SourceAppName = nameof(MidiDomotica);

                if (MidiDomoticaSetup.Init())
                {
                    manager = new MidiManager();

                    Logger.Log("MidiDomotica Core functionality Initialized!");
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error while initializing MidiDomotica Api!\nError: " + e.Message + "\nStackTrace:\n" + e.StackTrace, SeverityLevel.High);
            }

            if (MidiDomoticaSetup.Initialized)
            {
                form = new MidiDomotica();

                MidiDomoticaApi.Program.MidiManager = manager;

                Api = new Thread(new ThreadStart(() =>
                        {
                            Logger.Log("Start initializing MidiDomotica Web Api!");

                            MidiDomoticaApi.Program.Main(args);
                        })
                    );
                Api.Start();
            }
            else
            {
                Logger.Log("Failed to initialize MidiDomotica Api!");
                MessageBox.Show("Setup has failed!\nApplication will close now.", "Setup has failed!", MessageBoxButtons.OK);
                Application.Exit();
                return;
            }

            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

            InitializeComponent();
            TrayIcon.Visible = true;

            Logger.Log("MidiDomotica Application fully initialized!");
        }

        private void InitializeComponent()
        {
            TrayIcon = new NotifyIcon();
            TrayIcon.Text = "MidiDomotica - Api";

            TrayIcon.Icon = Properties.Resources.MidiDomotica;

            TrayIcon.DoubleClick += TrayIcon_DoubleClick;

            TrayIconContextMenu = new ContextMenuStrip();
            FormToggleItem = new ToolStripMenuItem();
            CloseMenuItem = new ToolStripMenuItem();
            TrayIconContextMenu.SuspendLayout();

            // 
            // TrayIconContextMenu
            // 
            this.TrayIconContextMenu.Items.AddRange(new ToolStripItem[] {
                this.FormToggleItem,
                this.CloseMenuItem,
            });
            this.TrayIconContextMenu.Name = "MidiDomotica Api";
            this.TrayIconContextMenu.Size = new Size(153, 70);

            //
            // FormMenuItem
            //
            this.FormToggleItem.Name = "FormToggle";
            this.FormToggleItem.Size = new Size(152, 152);
            this.FormToggleItem.Text = form.Visible ? "Hide" : "Show";
            this.FormToggleItem.Click += new EventHandler(this.FormToggleButton_Click);

            // 
            // CloseMenuItem
            // 
            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Size = new Size(152, 22);
            this.CloseMenuItem.Text = "Close MidiDomotica Api";
            this.CloseMenuItem.Click += new EventHandler(this.CloseMenuItem_Click);

            TrayIconContextMenu.ResumeLayout(false);
            TrayIcon.ContextMenuStrip = TrayIconContextMenu;
        }

        private void FormToggleButton_Click(object sender, EventArgs e)
        {
            ToggleForm();
        }
        private void ToggleForm()
        {
            if (form != null && !form.IsDisposed && form.Visible)
            {
                form.ToggleForm(false);
                this.FormToggleItem.Text = "Show";
            }
            else
            {
                if (form == null || form.IsDisposed)
                {
                    form = null;
                    form = new MidiDomotica();
                }
                form.ToggleForm(true);
                this.FormToggleItem.Text = "Hide";
            }
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close MidiDomotica Api?",
                    "Close MidiDomotica Api?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            form.Close();
            form.Dispose();
            form = null;

            MidiDomoticaApi.Program.WebHost.StopAsync().Wait();

            TrayIcon.Visible = false;
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            ToggleForm();
        }
    }
}
