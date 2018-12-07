using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JollyConsole
{
    public partial class Form1 : Form
    {

        private static StringBuilder cmdOutput = null;
        Process cmdProcess;
        StreamWriter cmdStreamWriter;

        public Form1()
        {
            InitializeComponent();
            InitializeConsole();
        }

        void InitializeConsole()
        {
            cmdOutput = new StringBuilder("");
            cmdProcess = new Process();

            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.StartInfo.RedirectStandardOutput = true;

            cmdProcess.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
            cmdProcess.StartInfo.RedirectStandardInput = true;
            cmdProcess.Start();

            cmdStreamWriter = cmdProcess.StandardInput;
            cmdProcess.BeginOutputReadLine();
        }

        private static void SortOutputHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                cmdOutput.Append(Environment.NewLine + outLine.Data);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (panel4.Visible)
            {
                panel4.Hide();
            } else
            {
                panel4.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string action1 = textBox1.Text;
            string action2 = textBox2.Text;
            string result = action1 + " && " + action2 + " && git add . && git commit -m \"Test commit\"";

            cmdStreamWriter.WriteLine(result);
            Thread.Sleep(500);
            textBox3.Text = cmdOutput.ToString();
        }
    }
}
