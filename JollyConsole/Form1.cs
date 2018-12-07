﻿using System;
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
        List<Macro> macros = new List<Macro>();
        private int macroId = 0;
        List<Panel> panels = new List<Panel>();

        public Form1()
        {
            InitializeComponent();
            InitializeConsole();
            InitializeGUIPanel();
            mainPanel.AutoScroll = true;
        }

        private void InitializeGUIPanel()
        {
            Macro initialMacro = new Macro
            {
                Id = macroId,
                Name = "GIT",
                Position = 0,
                Commands = new List<Command>
                {
                    new Command
                    {
                        Text = "cd ..",
                        Enabled = true,
                        Position = 0
                    },
                    new Command
                    {
                        Text = "cd ..",
                        Enabled = true,
                        Position = 1
                    }
                },
            };
            macroId++;
            macros.Add(initialMacro);

            foreach (Macro macro in macros)
            {
                Button macroName = new Button();
                macroName.Location = new System.Drawing.Point(3, 80);
                macroName.Name = "button" + macro.Id;
                macroName.Size = new System.Drawing.Size(100, 25);
                macroName.TabIndex = 1;
                macroName.Text = macro.Name;
                macroName.UseVisualStyleBackColor = true;
                macroName.Click += new System.EventHandler(this.showHide);
                mainPanel.Controls.Add(macroName);

                Panel panel = new Panel();
                panels.Add(panel);
                panel.Location = new System.Drawing.Point(3, (80 + macroName.Height + 10));
                panel.Name = "panel" + macro.Id;
                panel.Size = new System.Drawing.Size(100, 426);
                panel.TabIndex = 0;

                int textBoxLocation = 5;
                foreach (Command command in macro.Commands)
                {
                    TextBox textBox = new TextBox();
                    textBox.Location = new System.Drawing.Point(0, textBoxLocation);
                    textBox.Name = "textBox" + macro.Commands.IndexOf(command);
                    textBox.Size = new System.Drawing.Size(100, 20);
                    textBox.TabIndex = command.Position;
                    textBox.Text = command.Text;
                    panel.Controls.Add(textBox);
                    textBoxLocation += textBox.Height + 10;
                }

                Button executeButton = new Button();
                executeButton.Location = new System.Drawing.Point(0, 30 * macro.Commands.Count + 10);
                executeButton.Name = "execute" + macro.Id;
                executeButton.Size = new System.Drawing.Size(100, 23);
                executeButton.TabIndex = 1;
                executeButton.Text = "EXECUTE";
                executeButton.UseVisualStyleBackColor = true;
                executeButton.Click += new System.EventHandler(this.execute_click);
                panel.Controls.Add(executeButton);

                mainPanel.Controls.Add(panel);
            }
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
            cmdProcess.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataHandler);
            cmdProcess.StartInfo.RedirectStandardInput = true;
            cmdProcess.StartInfo.RedirectStandardError = true;
            cmdProcess.Start();

            cmdStreamWriter = cmdProcess.StandardInput;
            cmdProcess.BeginOutputReadLine();
            cmdProcess.BeginErrorReadLine();
        }

        private static void SortOutputHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                cmdOutput.Append(Environment.NewLine + outLine.Data);
            }
        }

        private static void ErrorDataHandler(object sendingProcess,
            DataReceivedEventArgs errLine)
        {
            if (!string.IsNullOrEmpty(errLine.Data))
            {
                cmdOutput.Append(Environment.NewLine + errLine.Data);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void showHide(object sender, EventArgs e)
        {
            Button button = ((Button)sender);
            foreach (Panel panel in panels)
            {
                if (panel.Name.Replace("panel", "") == button.Name.Replace("button", "")) {
                    if (panel.Visible)
                    {
                        panel.Hide();
                    }
                    else
                    {
                        panel.Show();
                    }
                    break;
                }
            }
            
        }

        private void execute_click(object sender, EventArgs e)
        {

            Button button = ((Button)sender);
            string result = "";
            foreach (Macro macro in macros)
            {
                if (macro.Id.ToString() == button.Name.Replace("execute", ""))
                {
                    foreach (Command command in macro.Commands)
                    {
                        result += command.Text + " && ";
                    }
                    result = result.Substring(0, result.Length - 4);
                }
                break;
            }
            cmdStreamWriter.WriteLine(result);
            Thread.Sleep(500);
            textBox3.Text = cmdOutput.ToString();
        }
    }
}
