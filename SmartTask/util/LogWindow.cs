using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartTask
{
    public partial class LogWindow : Form
    {
        public delegate void OutputProxy(string message);

        public LogWindow()
        {
            // http://www.cnblogs.com/txw1958/archive/2012/08/21/csharp-crossthread-widget.html
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        public int Counter { get; set; }

        /// <summary>
        /// Output Log Message
        /// </summary>
        /// <param name="message"></param>
        public void Output(string message)
        {
            //if (this.InvokeRequired)
            //{
            //    this.txtLogOut.BeginInvoke(new OutputProxy(Output), message);
            //    this.txtLogOut.Invoke(new OutputProxy(Output), message);
            //}
            //else
            {
                if (Counter >= 100)
                {
                    // Clear Log
                    this.txtLogOut.Text = "";
                    Counter = 0;
                }

                Counter++;
                this.txtLogOut.AppendText("[" + Counter + "]" + message + System.Environment.NewLine);
            }
        }

        private void LogWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.LogWin = null;
        }
    }
}
