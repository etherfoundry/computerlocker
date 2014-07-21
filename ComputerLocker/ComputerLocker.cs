using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace ComputerLocker
{
    

    public partial class frmMain : Form
    {
        [DllImport("user32")]
        public static extern void LockWorkStation();

        public bool SessionLocked = false;

        public frmMain()
        {
            Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            InitializeComponent();
        }

        private bool checkLockCondition()
        {
            bool result = false;
            if (this.SessionLocked)
            {
                return false;
            }

            if (DateTime.Now.Hour > 1 && DateTime.Now.Hour < 6 && DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
            {
                result = true;
            }

            return result;
        }
        
        private void lockPc()
        {
            LockWorkStation();
        }

        private void tmrCheckLock_Tick(object sender, EventArgs e)
        {
            if (checkLockCondition())
            {
                lockPc();
            }
        }

        private void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if(e.Reason == SessionSwitchReason.SessionUnlock)
            {
                this.SessionLocked = false;
                if(checkLockCondition())
                {
                    lockPc();
                }
            }
            if(e.Reason == SessionSwitchReason.SessionLock)
            {
                this.SessionLocked = true;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
