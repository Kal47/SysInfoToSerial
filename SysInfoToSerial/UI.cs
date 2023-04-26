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
        private ToolStripItemCollection StartStop;
        private ToolStripMenuItem ComPortDropDown = new ToolStripMenuItem();

        public ViewModel Config { get; set; }


        public delegate void MyEventHandler(object sender, EventArgs e);

        public UI()
        {
            Thread notifyThread = new Thread(
            delegate ()
            {
                notifyIcon.MouseUp += RightClick;
                notifyIcon.Icon = new Icon("on.ico");
                notifyIcon.Text = "SysInfoToSerial";
                notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                notifyIcon.ContextMenuStrip.Items.Add("Start/Stop", null, this.MenuPause_Click);
                notifyIcon.ContextMenuStrip.Items.Add("Exit", null, this.MenuExit_Click);
                notifyIcon.Visible = true;

                Application.Run();
            }
        );
            notifyThread.Start();
        }

        private void RightClick(object? sender, MouseEventArgs e)
        {       
            ComPortDropDown = new ToolStripMenuItem();
            foreach (String port in Config.AvalableSerialPorts)
            {
                ComPortDropDown.DropDownItems.Add(port, null, (sender, e) => { Config.ActiveSerialPort = port; });
            }
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuPause_Click(object sender, EventArgs e)
        {
            Config.RunSerialPort = !Config.RunSerialPort;
            if (Config.RunSerialPort)
                notifyIcon.Icon = new Icon("on.ico");
            else
                notifyIcon.Icon = new Icon("off.ico");
        }
        

        ~UI()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }
    }
}
