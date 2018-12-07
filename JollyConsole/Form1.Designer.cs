﻿using System.Drawing;
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.mainPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.AutoScroll = true;
            this.mainPanel.Controls.Add(this.label1);
            this.mainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mainPanel.Location = new System.Drawing.Point(12, 12);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(136, 426);
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
                panel0.Size = new System.Drawing.Size(133, 135);
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
                    textBox1.Size = new System.Drawing.Size(122, 20);
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
                execute0.Size = new System.Drawing.Size(122, 23);
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
            this.panel2.Location = new System.Drawing.Point(154, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(634, 342);
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
            this.textBox3.Size = new System.Drawing.Size(628, 336);
            this.textBox3.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(154, 360);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(634, 79);
            this.panel3.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.mainPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.mainPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel mainPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBox3;
    }
}

