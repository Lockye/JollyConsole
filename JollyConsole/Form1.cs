using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace JollyConsole
{
    public partial class Form1 : Form
    {
        private static StringBuilder cmdOutput = null;
        private static string CurrentConsoleCommand = "";

        const string Separator = "---Command Completed---";
        // Has to be something that won't occur in normal output.  

        volatile bool finished = false;

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

        private void CheckConsolePressedKey(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = ((TextBox)sender);
            string[] textBoxNameParts = textBox.Name.Replace("textBox", "").Split(new string[] { "Index" }, StringSplitOptions.None);
            bool isEnter = e.KeyChar == Convert.ToChar(Keys.Return);
            bool isBackspace = e.KeyChar == Convert.ToChar(Keys.Back);
            if (isEnter)
            {
                e.Handled = true;
                SendToCmd(CurrentConsoleCommand);
                CurrentConsoleCommand = "";
            } else if (isBackspace)
            {
                if (CurrentConsoleCommand.Length > 0)
                {
                    CurrentConsoleCommand = CurrentConsoleCommand.Remove(CurrentConsoleCommand.Length - 1);
                } else
                {
                    e.Handled = true;
                }
            } else
            {
                CurrentConsoleCommand += e.KeyChar;
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

            SendToCmd(result);
        }

        private void SendToCmd(string result)
        {
            finished = false;
            cmdOutput.Clear();
            cmdStreamWriter.WriteLine(result);
            cmdStreamWriter.WriteLine("@echo " + Separator);

            while (!finished)
            {
                Thread.Sleep(100);
                if (cmdOutput.ToString().IndexOf(Separator, StringComparison.Ordinal) >= 0)
                {
                    finished = true;
                }
            }
            var outputString = RemoveLastLine(textBox3.Text);
            outputString += cmdOutput.ToString();
            outputString = RemoveLastLine(outputString);
            outputString = RemoveLastLine(outputString);
            outputString += Environment.NewLine;
            outputString += GetCurrentLocation();

            textBox3.Text = "";
            textBox3.AppendText(outputString);
        }

        private string RemoveLastLine(string str)
        {
            return str.Remove(str.LastIndexOf(Environment.NewLine));
        }

        private void buttonMacro1_Click(object sender, EventArgs e)
        {
            ChangePanelVisibility(sender, e);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JSON |*.json";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                string json = sr.ReadToEnd();
                sr.Close();

                macros.Clear();
                Macro[] macrosFromJson = JsonConvert.DeserializeObject<Macro[]>(json);
                foreach (Macro macro in macrosFromJson)
                {
                    macros.Add(macro);
                }

                foreach (Macro macro in macros)
                {
                    // add logic for Id, Name...
                    foreach (Command command in macro.Commands)
                    {
                        foreach (Control control in panels[macro.Id].Controls)
                        {
                            if ("textBox" + macro.Id + "Index" + macro.Commands.IndexOf(command) == control.Name)
                            {
                                ((TextBox) control).Text = command.Text;
                            }
                        }
                    }
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";  
            saveFileDialog1.Title = "Save template";
            saveFileDialog1.DefaultExt = "json";
            saveFileDialog1.Filter = "JSON |*.json";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(JsonConvert.SerializeObject(macros, Formatting.Indented));
                    }
                }
            }
        }
    }
}
