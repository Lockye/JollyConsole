using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace JollyConsole
{
    public partial class Form1 : Form
    {
        private static StringBuilder cmdOutput = null;
        private static int NUMBER_OF_MACROS = 4;
        private static int NUMBER_OF_COMMANDS_PER_MACRO = 4;

        Process cmdProcess;
        StreamWriter cmdStreamWriter;

        private int macroId = 0;

        List<Macro> macros = new List<Macro>();
        List<Panel> panels = new List<Panel>();

        public Form1()
        {
            InitializeComponent();
            InitializeConsole();
            InitializeGUIPanel();
        }

        private void InitializeGUIPanel()
        {
            InitMacros();
        }

        private void InitMacros()
        {
            for(int i = 0; i < NUMBER_OF_MACROS; i++)
            {
                Macro initialMacro = new Macro
                {
                    Id = i,
                    Name = "GIT",
                    Position = i*2
                };

                var listCommands = new List<Command>();
                for (int j = 0; j < NUMBER_OF_COMMANDS_PER_MACRO; j++)
                {
                    Command tempCommand = new Command
                    {
                        Text = "",
                        Enabled = true,
                        Position = j
                    };
                    listCommands.Add(tempCommand);
                }

                initialMacro.Commands = listCommands;
                macros.Add(initialMacro);
            }
        }

        private void CommandTextBoxFocusLost(object sender, EventArgs e)
        {
            TextBox textBox = ((TextBox)sender);
            string[] textBoxNameParts = textBox.Name.Replace("textBox", "").Split(new string[] { "Index" }, StringSplitOptions.None);
            foreach (Macro macro in macros)
            {
                if (macro.Id.ToString() == textBoxNameParts[0])
                {
                    foreach (Command command in macro.Commands)
                    {
                        if (macro.Commands.IndexOf(command).ToString() == textBoxNameParts[1])
                        {
                            command.Text = textBox.Text;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        private void Enter_key_event(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = ((TextBox)sender);
            string[] textBoxNameParts = textBox.Name.Replace("textBox", "").Split(new string[] { "Index" }, StringSplitOptions.None);
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                foreach (Macro macro in macros)
                {
                    if (macro.Id.ToString() == textBoxNameParts[0])
                    {
                        foreach (Command command in macro.Commands)
                        {
                            if (macro.Commands.IndexOf(command).ToString() == textBoxNameParts[1])
                            {
                                command.Text = textBox.Text;
                                break;
                            }
                        }
                        break;
                    }
                }

                foreach (Panel panel in panels)
                {
                    if (panel.Name.Replace("panel", "") == textBoxNameParts[0])
                    {
                        foreach (Control control in panel.Controls)
                        {
                            if (control.Name.Replace("execute", "") == textBoxNameParts[0])
                            {
                                ((Button)control).PerformClick();
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void InitializeConsole()
        {
            cmdOutput = new StringBuilder("");
            cmdProcess = new Process();

            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.StartInfo.RedirectStandardOutput = true;

            cmdProcess.OutputDataReceived += SortOutputHandler;
            cmdProcess.ErrorDataReceived += ErrorDataHandler;
            cmdProcess.StartInfo.RedirectStandardInput = true;
            cmdProcess.StartInfo.RedirectStandardError = true;
            cmdProcess.Start();

            cmdStreamWriter = cmdProcess.StandardInput;
            cmdProcess.BeginOutputReadLine();
            cmdProcess.BeginErrorReadLine();
            Thread.Sleep(100);
            textBox3.Text = cmdOutput.ToString();
            Thread.Sleep(100);
            SetAndPrintDefaultLocation();
        }

        private void SetAndPrintDefaultLocation()
        {
            cmdOutput.Clear();
            cmdStreamWriter.WriteLine("c:&&cd %userprofile%");
            Thread.Sleep(100);
            textBox3.AppendText(Environment.NewLine);
            textBox3.AppendText(Environment.NewLine);
            textBox3.AppendText(GetCurrentLocation());
        }

        private string GetCurrentLocation()
        {
            cmdOutput.Clear();
            cmdStreamWriter.WriteLine("cd");
            Thread.Sleep(100);
            string[] lines = cmdOutput.ToString().Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);

            return $"{lines[2]}>";
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

        private void ChangePanelVisibility(object sender, EventArgs e)
        {
            Button button = ((Button)sender);
            foreach (Panel panel in panels)
            {
                if (panel.Name.Replace("panel", "") == button.Name.Replace("button", ""))
                {
                    if (panel.Visible)
                    {
                        panel.Hide();
                    }
                    else
                    {
                        panel.Show();
                    }
                }
                else
                {
                    panel.Hide();
                }
            }
        }

        private void Execute_click(object sender, EventArgs e)
        {
            Button button = ((Button)sender);
            string result = "";
            foreach (Macro macro in macros)
            {
                if (macro.Id.ToString() == button.Name.Replace("execute", ""))
                {
                    bool allEmpty = true;
                    foreach (Command command in macro.Commands)
                    {
                        allEmpty &= command.Text.Length == 0;
                    }
                    if (allEmpty)
                    {
                        return;
                    }
                    foreach (Command command in macro.Commands)
                    {
                        if (command.Text.Length > 0)
                        {
                            result += command.Text + " && ";
                        }
                    }
                    result = result.Substring(0, result.Length - 4);
                    break;
                }
            }
            cmdOutput.Clear();
            cmdStreamWriter.WriteLine(result);
            Thread.Sleep(2000);
            textBox3.Text = RemoveLastLine();
            textBox3.AppendText(cmdOutput.ToString());
            textBox3.AppendText(Environment.NewLine);
            textBox3.AppendText(GetCurrentLocation());
        }

        private string RemoveLastLine()
        {
            return textBox3.Text.Remove(textBox3.Text.LastIndexOf(Environment.NewLine));
        }

        private void buttonMacro1_Click(object sender, EventArgs e)
        {
            ChangePanelVisibility(sender, e);
        }
    }
}
