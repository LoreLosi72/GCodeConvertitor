
namespace Elaborato_EdS_2021
{
    partial class FormTesto
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.EscitoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.Testotxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AltezzatextBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.FlyAltezzatextBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ProfonditatextBox3 = new System.Windows.Forms.TextBox();
            this.GeneraGcodebutton1 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.TESTO = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.TESTO.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.DarkOrange;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EscitoolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(514, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // EscitoolStripMenuItem
            // 
            this.EscitoolStripMenuItem.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EscitoolStripMenuItem.Name = "EscitoolStripMenuItem";
            this.EscitoolStripMenuItem.Size = new System.Drawing.Size(55, 29);
            this.EscitoolStripMenuItem.Text = "Esci";
            this.EscitoolStripMenuItem.Click += new System.EventHandler(this.EscitoolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Orange;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(119, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "TESTO DA CONVERTIRE:";
            // 
            // Testotxt
            // 
            this.Testotxt.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Testotxt.Location = new System.Drawing.Point(26, 103);
            this.Testotxt.Multiline = true;
            this.Testotxt.Name = "Testotxt";
            this.Testotxt.Size = new System.Drawing.Size(445, 50);
            this.Testotxt.TabIndex = 2;
            this.Testotxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(61, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 30);
            this.label2.TabIndex = 3;
            this.label2.Text = "ALTEZZA TESTO:";
            // 
            // AltezzatextBox1
            // 
            this.AltezzatextBox1.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AltezzatextBox1.Location = new System.Drawing.Point(300, 207);
            this.AltezzatextBox1.Multiline = true;
            this.AltezzatextBox1.Name = "AltezzatextBox1";
            this.AltezzatextBox1.Size = new System.Drawing.Size(117, 50);
            this.AltezzatextBox1.TabIndex = 4;
            this.AltezzatextBox1.Text = "4";
            this.AltezzatextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(73, 326);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 30);
            this.label3.TabIndex = 5;
            this.label3.Text = "FLY ALTEZZA:";
            // 
            // FlyAltezzatextBox2
            // 
            this.FlyAltezzatextBox2.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FlyAltezzatextBox2.Location = new System.Drawing.Point(300, 315);
            this.FlyAltezzatextBox2.Multiline = true;
            this.FlyAltezzatextBox2.Name = "FlyAltezzatextBox2";
            this.FlyAltezzatextBox2.Size = new System.Drawing.Size(117, 50);
            this.FlyAltezzatextBox2.TabIndex = 6;
            this.FlyAltezzatextBox2.Text = "5";
            this.FlyAltezzatextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(21, 423);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(249, 30);
            this.label4.TabIndex = 7;
            this.label4.Text = "PROFONDITA DI TAGLIO:";
            // 
            // ProfonditatextBox3
            // 
            this.ProfonditatextBox3.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProfonditatextBox3.Location = new System.Drawing.Point(300, 412);
            this.ProfonditatextBox3.Multiline = true;
            this.ProfonditatextBox3.Name = "ProfonditatextBox3";
            this.ProfonditatextBox3.Size = new System.Drawing.Size(117, 48);
            this.ProfonditatextBox3.TabIndex = 8;
            this.ProfonditatextBox3.Text = "-1";
            this.ProfonditatextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // GeneraGcodebutton1
            // 
            this.GeneraGcodebutton1.BackColor = System.Drawing.Color.DarkOrange;
            this.GeneraGcodebutton1.Location = new System.Drawing.Point(124, 513);
            this.GeneraGcodebutton1.Name = "GeneraGcodebutton1";
            this.GeneraGcodebutton1.Size = new System.Drawing.Size(228, 69);
            this.GeneraGcodebutton1.TabIndex = 9;
            this.GeneraGcodebutton1.Text = "Genera Gcode";
            this.GeneraGcodebutton1.UseVisualStyleBackColor = false;
            this.GeneraGcodebutton1.Click += new System.EventHandler(this.GeneraGcodebutton1_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "TestoGCode.nc";
            this.saveFileDialog1.Filter = "G-Code Files(*.CNC;*.NC;*.TAP;*.TXT)|*.CNC;*.NC;*.TAP;*.TXT|All files (*.*)|*.*";
            // 
            // TESTO
            // 
            this.TESTO.BackColor = System.Drawing.Color.Orange;
            this.TESTO.Controls.Add(this.label1);
            this.TESTO.Controls.Add(this.GeneraGcodebutton1);
            this.TESTO.Controls.Add(this.Testotxt);
            this.TESTO.Controls.Add(this.ProfonditatextBox3);
            this.TESTO.Controls.Add(this.label2);
            this.TESTO.Controls.Add(this.label4);
            this.TESTO.Controls.Add(this.AltezzatextBox1);
            this.TESTO.Controls.Add(this.FlyAltezzatextBox2);
            this.TESTO.Controls.Add(this.label3);
            this.TESTO.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TESTO.Location = new System.Drawing.Point(13, 46);
            this.TESTO.Name = "TESTO";
            this.TESTO.Size = new System.Drawing.Size(490, 620);
            this.TESTO.TabIndex = 10;
            this.TESTO.TabStop = false;
            this.TESTO.Text = "TESTO";
            // 
            // FormTesto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Orange;
            this.ClientSize = new System.Drawing.Size(514, 680);
            this.Controls.Add(this.TESTO);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTesto";
            this.Text = "FormTesto";
            this.Load += new System.EventHandler(this.FormTesto_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.TESTO.ResumeLayout(false);
            this.TESTO.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem EscitoolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Testotxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AltezzatextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FlyAltezzatextBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ProfonditatextBox3;
        private System.Windows.Forms.Button GeneraGcodebutton1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox TESTO;
    }
}