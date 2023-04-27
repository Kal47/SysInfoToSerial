using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysInfoToSerial
{
    internal class UI
    {
        private NotifyIcon notifyIcon = new NotifyIcon();
        private ContextMenuStrip MainDropdown;
        private ToolStripItemCollection StartStop;
        private ToolStripMenuItem ComPortDropDown = new ToolStripMenuItem();
        private ContextMenuStrip ComPortDropDownStrip = new ContextMenuStrip();

        private ToolStripMenuItem WebServerEnable = new ToolStripMenuItem();
        private ToolStripMenuItem ComPortEnable = new ToolStripMenuItem();
        public ViewModel Config { get; set; }

        public delegate void MyEventHandler(object sender, EventArgs e);

        public UI()
        {
            Config = new ViewModel()
            {
                ActiveSerialPort = "COM4",
                RunSerialPort = true,
                RunWebSocketServer = true
            };

            Thread notifyThread = new Thread(
            delegate ()
            {
                notifyIcon.MouseUp += RightClick;
                notifyIcon.Icon = new Icon("On.ico");
                notifyIcon.Text = "SysInfoToSerial";

                MainDropdown = new ContextMenuStrip();
                notifyIcon.ContextMenuStrip = MainDropdown;


                WebServerEnable = new ToolStripMenuItem("Web Socket Server", new Icon("Green.ico").ToBitmap(), this.WebServerEnable_Click);
                ComPortEnable = new ToolStripMenuItem("Serial Port", new Icon("Green.ico").ToBitmap(), this.ComPortEnable_Click);
                ComPortDropDown = new ToolStripMenuItem("Serial Port Selcetion"); //populated on right click

                MainDropdown.Items.Add(WebServerEnable);
                MainDropdown.Items.Add(ComPortEnable);
                MainDropdown.Items.Add(ComPortDropDown);
                MainDropdown.Items.Add("Exit", null, this.MenuExit_Click);

                notifyIcon.ContextMenuStrip = MainDropdown;
                notifyIcon.Visible = true;

                Application.Run();
            }
        );
            notifyThread.Start();
        }

        private void PopulatePortList()
        {
            Bitmap check;
            ComPortDropDownStrip = new ContextMenuStrip();
            foreach (String port in Config.AvalableSerialPorts)
            {
                if (port == Config.ActiveSerialPort)
                    check = new Icon("Green.ico").ToBitmap();
                else
                    check = null;

                ComPortDropDownStrip.Items.Add(port, check, (sender, e) => { Config.ActiveSerialPort = port; });
                
                    
            }
            ComPortDropDown.DropDown = ComPortDropDownStrip;
        }

        private void RightClick(object? sender, MouseEventArgs e)
        {
            Console.WriteLine("right Click");
            PopulatePortList();
        }

        private void ComPortEnable_Click(object? sender, EventArgs e)
        {
            Config.RunSerialPort = !Config.RunSerialPort;
            if (Config.RunSerialPort)
                ComPortEnable.Image = new Icon("Green.ico").ToBitmap();
            else
                ComPortEnable.Image = new Icon("Red.ico").ToBitmap();
        }

        private void WebServerEnable_Click(object sender, EventArgs e)
        {
            Config.RunWebSocketServer = !Config.RunWebSocketServer;
            if (Config.RunWebSocketServer)
                WebServerEnable.Image = new Icon("Green.ico").ToBitmap();
            else
                WebServerEnable.Image = new Icon("Red.ico").ToBitmap();
        }
        private void MenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }      

        ~UI()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }
    }
}
