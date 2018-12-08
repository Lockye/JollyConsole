using Newtonsoft.Json;
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

        private static int NUMBER_OF_MACROS = 4;
        private static int NUMBER_OF_COMMANDS_PER_MACRO = 4;
        private static StringBuilder cmdOutput;
        private static string CurrentConsoleCommand = "";

        private const string Separator = "---Command Completed---";
        // Has to be something that won't occur in normal output.  
        volatile bool finished;

        Process cmdProcess;
        StreamWriter cmdStreamWriter;

        private int macroId = 0;

        private readonly List<Macro> macros = new List<Macro>();
        private readonly List<Panel> panels = new List<Panel>();
        private readonly List<Button> buttons = new List<Button>();

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
            for (int i = 0; i < NUMBER_OF_MACROS; i++)
            {
                Macro initialMacro = new Macro
                {
                    Id = i,
                    Name = "Macro" + i,
                    Position = i * 2
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
            var textBox = ((TextBox)sender);
            if (textBox == null)
                return;

            var textBoxNameParts = textBox.Name.Replace("textBox", "").Split(new[] { "Index" }, StringSplitOptions.None);
            foreach (var macro in macros)
            {
                if (macro.Id.ToString() != textBoxNameParts[0])
                    continue;

                foreach (var command in macro.Commands)
                {
                    if (macro.Commands.IndexOf(command).ToString() != textBoxNameParts[1])
                        continue;

                    command.Text = textBox.Text;
                    break;
                }

                break;
            }
        }

        private void CheckConsolePressedKey(object sender, KeyPressEventArgs e)
        {
            bool isEnter = e.KeyChar == Convert.ToChar(Keys.Return);
            bool isBackspace = e.KeyChar == Convert.ToChar(Keys.Back);
            int caretPositionFromEnd = textBox3.TextLength - textBox3.SelectionStart;
            if (isEnter)
            {
                e.Handled = true;
                SendToCmd(CurrentConsoleCommand);
                CurrentConsoleCommand = "";
            }
            else if (isBackspace)
            {
                if (caretPositionFromEnd >= CurrentConsoleCommand.Length)
                {
                    e.Handled = true;
                    return;
                }

                if (CurrentConsoleCommand.Length > 0)
                {

                    CurrentConsoleCommand =
                        CurrentConsoleCommand.Remove(CurrentConsoleCommand.Length - caretPositionFromEnd - 1, 1);
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                CurrentConsoleCommand =
                    CurrentConsoleCommand.Insert(CurrentConsoleCommand.Length - caretPositionFromEnd,
                        e.KeyChar.ToString());
            }
        }

        private void CheckConsoleDownKey(object sender, KeyEventArgs e)
        {
            if (e.Modifiers != Keys.Control)
                return;

            try
            {
                var macroIndex = GetLastConfiguredMacroIndex(e.KeyCode);
                AddCommandToMacroChain(CurrentConsoleCommand, macroIndex);
            }
            catch (KeyNotFoundException)
            {
            }
        }

        private int GetLastConfiguredMacroIndex(Keys keyCode)
        {
            Dictionary<Keys, int> dict = new Dictionary<Keys, int>
            {
                [Keys.D0] = 0,
                [Keys.NumPad0] = 0,
                [Keys.D1] = 1,
                [Keys.NumPad1] = 1,
                [Keys.D2] = 2,
                [Keys.NumPad2] = 2,
                [Keys.D3] = 3,
                [Keys.NumPad3] = 3
            };

            return dict[keyCode];
        }

        private void AddCommandToMacroChain(string currentConsoleCommand, int macroIndex)
        {
            string[] commands = currentConsoleCommand.Split(new[] { " && " }, StringSplitOptions.None);
            try
            {
                foreach (Command command in macros[macroIndex].Commands)
                {
                    command.Text = "";
                }

                foreach (Control control in panels[macroIndex].Controls)
                {
                    if (control.Name.StartsWith("textBox" + macroIndex))
                    {
                        control.Text = "";
                    }
                }

                for (int i = 0; i < commands.Length; i++)
                {
                    macros[macroIndex].Commands[i].Text = commands[i];
                    foreach (Control control in panels[macroIndex].Controls)
                    {
                        if (control.Name == "textBox" + macros[macroIndex].Id + "Index" + i)
                        {
                            control.Text = commands[i];
                            break;
                        }
                    }
                }
            }
            catch (KeyNotFoundException)
            {
            }
        }

        private void Space_key_event(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == Convert.ToChar(Keys.Space))
            //{
            //    var buttonSender = (Button) sender;
            //    var tempPanel = panels.Find(panel => panel.Name.Replace("panel", "") == buttonSender.Name.Replace("button", ""));
            //    var control = tempPanel.Controls.Find("execute")
            //}
        }

        private void Enter_key_event(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = ((TextBox)sender);
            string[] textBoxNameParts = textBox.Name.Replace("textBox", "").Split(new[] { "Index" }, StringSplitOptions.None);

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
            cmdProcess = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                }
            };


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
            CurrentConsoleCommand = "";
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

        private static string RemoveLastLine(string str)
        {
            return str.Remove(str.LastIndexOf(Environment.NewLine, StringComparison.Ordinal));
        }

        private void ButtonMacro_Click(object sender, EventArgs e)
        {
            ChangePanelVisibility(sender, e);
        }

        private void ChangePanelVisibility(object sender, EventArgs e)
        {
            var button = ((Button)sender);
            foreach (var panel in panels)
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

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JSON |*.json";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                string json = sr.ReadToEnd();
                sr.Close();

                macros.Clear();
                Macro[] macrosFromJson = JsonConvert.DeserializeObject<Macro[]>(json);
                foreach (Macro macro in macrosFromJson)
                {
                    macros.Add(macro);
                }

                for (var i = 0; i < macros.Count; i++)
                {
                    var macro = macros[i];

                    buttons[i].Text = macro.Name;

                    foreach (var command in macro.Commands)
                    {
                        foreach (Control control in panels[macro.Id].Controls)
                        {
                            if ("textBox" + macro.Id + "Index" + macro.Commands.IndexOf(command) == control.Name)
                            {
                                control.Text = command.Text;
                            }
                        }
                    }
                }
            }
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save template";
            saveFileDialog1.DefaultExt = "json";
            saveFileDialog1.Filter = "JSON |*.json";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(JsonConvert.SerializeObject(macros, Formatting.Indented));
                }
            }
        }

        private void RightClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var dlg = new MacroNameDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            var buttonClicked = (Button) sender;
            var newMacroName = dlg.textBox1.Text;
            buttonClicked.Text = newMacroName;

            var macroClickedId = buttonClicked.Name.Replace("button", "");
            foreach (var macro in macros)
            {
                if (macro.Id.ToString() != macroClickedId)
                    continue;

                macro.Name = newMacroName;
                break;
            }
        }
    }
}