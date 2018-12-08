using System.Drawing;
using System.Windows.Forms;

namespace JollyConsole
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.AutoScroll = true;
            this.mainPanel.Controls.Add(this.label1);
            this.mainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mainPanel.Location = new System.Drawing.Point(12, 38);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(136, 400);
            this.mainPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 78);
            this.label1.TabIndex = 0;
            this.label1.Text = "MACROS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            for (int i = 0; i < NUMBER_OF_MACROS; i++)
            {
                // 
                // button0
                // 
                var button0 = new Button();
                button0.Location = new System.Drawing.Point(3, 81);
                button0.Name = "button" + i;
                button0.Size = new System.Drawing.Size(125, 23);
                button0.TabIndex = i * 2;
                button0.Text = "Macro" + i;
                button0.UseVisualStyleBackColor = true;
                button0.Click += new System.EventHandler(buttonMacro1_Click);
                this.mainPanel.Controls.Add(button0);
                // 
                // panel0
                // 
                var panel0 = new FlowLayoutPanel();
                panel0.SuspendLayout();
                panel0.Location = new System.Drawing.Point(3, 110);
                panel0.Name = "panel" + i;
                panel0.Size = new System.Drawing.Size(130, 135);
                panel0.TabIndex = i * 2 + 1;
                if (i > 0)
                {
                    panel0.Hide();
                }
                panels.Add(panel0);

                for (int j = 0; j < NUMBER_OF_COMMANDS_PER_MACRO; j++)
                {
                    // 
                    // textBox1
                    // 
                    var textBox1 = new TextBox();
                    textBox1.Location = new System.Drawing.Point(3, 3);
                    textBox1.Name = "textBox" + i + "Index" + j;
                    textBox1.Size = new System.Drawing.Size(119, 20);
                    textBox1.TabIndex = j;
                    textBox1.KeyPress += Enter_key_event;
                    textBox1.LostFocus += CommandTextBoxFocusLost;

                    panel0.Controls.Add(textBox1);
                }

                // 
                // execute0
                // 
                var execute0 = new Button();
                execute0.Location = new System.Drawing.Point(3, 107);
                execute0.Name = "execute" + i;
                execute0.Size = new System.Drawing.Size(119, 23);
                execute0.TabIndex = NUMBER_OF_COMMANDS_PER_MACRO;
                execute0.Text = "Execute" + i;
                execute0.UseVisualStyleBackColor = true;
                execute0.Click += Execute_click;

                panel0.Controls.Add(execute0);
                panel0.ResumeLayout(false);
                panel0.PerformLayout();

                this.mainPanel.Controls.Add(panel0);
            }
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.textBox3);
            this.panel2.Location = new System.Drawing.Point(154, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(634, 400);
            this.panel2.TabIndex = 1;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Black;
            this.textBox3.ForeColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(3, 3);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(628, 394);
            this.textBox3.TabIndex = 0;
            this.textBox3.KeyPress += CheckConsolePressedKey;
            this.textBox3.KeyDown += CheckConsoleDownKey;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.importToolStripMenuItem.Text = "Import...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.mainPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel mainPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBox3;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem importToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
    }
}

