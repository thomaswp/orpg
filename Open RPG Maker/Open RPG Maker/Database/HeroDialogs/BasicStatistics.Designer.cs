namespace ORPG.HeroDialogs
{
    partial class BasicStatistics
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabHP = new System.Windows.Forms.TabPage();
            this.statHP = new ORPG.Stat();
            this.tabSP = new System.Windows.Forms.TabPage();
            this.statSP = new ORPG.Stat();
            this.tabStr = new System.Windows.Forms.TabPage();
            this.statStr = new ORPG.Stat();
            this.tabDex = new System.Windows.Forms.TabPage();
            this.statDex = new ORPG.Stat();
            this.tabAgi = new System.Windows.Forms.TabPage();
            this.statAgi = new ORPG.Stat();
            this.tabInt = new System.Windows.Forms.TabPage();
            this.statInt = new ORPG.Stat();
            this.tableLayoutPanel.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabHP.SuspendLayout();
            this.tabSP.SuspendLayout();
            this.tabStr.SuspendLayout();
            this.tabDex.SuspendLayout();
            this.tabAgi.SuspendLayout();
            this.tabInt.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel.Controls.Add(this.buttonOk, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonCancel, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.tabControlMain, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel1";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(544, 344);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonOk.Location = new System.Drawing.Point(397, 314);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(69, 24);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(472, 314);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(69, 24);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // tabControlMain
            // 
            this.tableLayoutPanel.SetColumnSpan(this.tabControlMain, 3);
            this.tabControlMain.Controls.Add(this.tabHP);
            this.tabControlMain.Controls.Add(this.tabSP);
            this.tabControlMain.Controls.Add(this.tabStr);
            this.tabControlMain.Controls.Add(this.tabDex);
            this.tabControlMain.Controls.Add(this.tabAgi);
            this.tabControlMain.Controls.Add(this.tabInt);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(3, 3);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(538, 303);
            this.tabControlMain.TabIndex = 4;
            // 
            // tabHP
            // 
            this.tabHP.Controls.Add(this.statHP);
            this.tabHP.Location = new System.Drawing.Point(4, 22);
            this.tabHP.Name = "tabHP";
            this.tabHP.Padding = new System.Windows.Forms.Padding(3);
            this.tabHP.Size = new System.Drawing.Size(530, 277);
            this.tabHP.TabIndex = 0;
            this.tabHP.Text = "Max HP";
            this.tabHP.UseVisualStyleBackColor = true;
            // 
            // statHP
            // 
            this.statHP.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(60)))), ((int)(((byte)(120)))));
            this.statHP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statHP.Level = 1;
            this.statHP.Location = new System.Drawing.Point(3, 3);
            this.statHP.Name = "statHP";
            this.statHP.Size = new System.Drawing.Size(524, 271);
            this.statHP.TabIndex = 1;
            this.statHP.Values = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            // 
            // tabSP
            // 
            this.tabSP.Controls.Add(this.statSP);
            this.tabSP.Location = new System.Drawing.Point(4, 22);
            this.tabSP.Name = "tabSP";
            this.tabSP.Padding = new System.Windows.Forms.Padding(3);
            this.tabSP.Size = new System.Drawing.Size(530, 277);
            this.tabSP.TabIndex = 1;
            this.tabSP.Text = "Max SP";
            this.tabSP.UseVisualStyleBackColor = true;
            // 
            // statSP
            // 
            this.statSP.Color = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(120)))), ((int)(((byte)(200)))));
            this.statSP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statSP.Level = 1;
            this.statSP.Location = new System.Drawing.Point(3, 3);
            this.statSP.Name = "statSP";
            this.statSP.Size = new System.Drawing.Size(524, 271);
            this.statSP.TabIndex = 1;
            this.statSP.Values = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            // 
            // tabStr
            // 
            this.tabStr.Controls.Add(this.statStr);
            this.tabStr.Location = new System.Drawing.Point(4, 22);
            this.tabStr.Name = "tabStr";
            this.tabStr.Padding = new System.Windows.Forms.Padding(3);
            this.tabStr.Size = new System.Drawing.Size(530, 277);
            this.tabStr.TabIndex = 2;
            this.tabStr.Text = "Strength";
            this.tabStr.UseVisualStyleBackColor = true;
            // 
            // statStr
            // 
            this.statStr.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(120)))), ((int)(((byte)(60)))));
            this.statStr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statStr.Level = 1;
            this.statStr.Location = new System.Drawing.Point(3, 3);
            this.statStr.Name = "statStr";
            this.statStr.Size = new System.Drawing.Size(524, 271);
            this.statStr.TabIndex = 1;
            this.statStr.Values = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            // 
            // tabDex
            // 
            this.tabDex.Controls.Add(this.statDex);
            this.tabDex.Location = new System.Drawing.Point(4, 22);
            this.tabDex.Name = "tabDex";
            this.tabDex.Padding = new System.Windows.Forms.Padding(3);
            this.tabDex.Size = new System.Drawing.Size(530, 277);
            this.tabDex.TabIndex = 3;
            this.tabDex.Text = "Dexterity";
            this.tabDex.UseVisualStyleBackColor = true;
            // 
            // statDex
            // 
            this.statDex.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(200)))), ((int)(((byte)(60)))));
            this.statDex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statDex.Level = 1;
            this.statDex.Location = new System.Drawing.Point(3, 3);
            this.statDex.Name = "statDex";
            this.statDex.Size = new System.Drawing.Size(524, 271);
            this.statDex.TabIndex = 1;
            this.statDex.Values = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            // 
            // tabAgi
            // 
            this.tabAgi.Controls.Add(this.statAgi);
            this.tabAgi.Location = new System.Drawing.Point(4, 22);
            this.tabAgi.Name = "tabAgi";
            this.tabAgi.Padding = new System.Windows.Forms.Padding(3);
            this.tabAgi.Size = new System.Drawing.Size(530, 277);
            this.tabAgi.TabIndex = 4;
            this.tabAgi.Text = "Agility";
            this.tabAgi.UseVisualStyleBackColor = true;
            // 
            // statAgi
            // 
            this.statAgi.Color = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(200)))), ((int)(((byte)(120)))));
            this.statAgi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statAgi.Level = 1;
            this.statAgi.Location = new System.Drawing.Point(3, 3);
            this.statAgi.Name = "statAgi";
            this.statAgi.Size = new System.Drawing.Size(524, 271);
            this.statAgi.TabIndex = 0;
            this.statAgi.Values = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            // 
            // tabInt
            // 
            this.tabInt.Controls.Add(this.statInt);
            this.tabInt.Location = new System.Drawing.Point(4, 22);
            this.tabInt.Name = "tabInt";
            this.tabInt.Padding = new System.Windows.Forms.Padding(3);
            this.tabInt.Size = new System.Drawing.Size(530, 277);
            this.tabInt.TabIndex = 5;
            this.tabInt.Text = "Intelligence";
            this.tabInt.UseVisualStyleBackColor = true;
            // 
            // statInt
            // 
            this.statInt.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(60)))), ((int)(((byte)(200)))));
            this.statInt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statInt.Level = 1;
            this.statInt.Location = new System.Drawing.Point(3, 3);
            this.statInt.Name = "statInt";
            this.statInt.Size = new System.Drawing.Size(524, 271);
            this.statInt.TabIndex = 1;
            this.statInt.Values = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            // 
            // BasicStatistics
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(544, 344);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(550, 370);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(550, 370);
            this.Name = "BasicStatistics";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Basic Statistics";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabHP.ResumeLayout(false);
            this.tabSP.ResumeLayout(false);
            this.tabStr.ResumeLayout(false);
            this.tabDex.ResumeLayout(false);
            this.tabAgi.ResumeLayout(false);
            this.tabInt.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabHP;
        private System.Windows.Forms.TabPage tabSP;
        private System.Windows.Forms.TabPage tabStr;
        private System.Windows.Forms.TabPage tabDex;
        private System.Windows.Forms.TabPage tabAgi;
        private System.Windows.Forms.TabPage tabInt;
        public ORPG.Stat statAgi;
        public ORPG.Stat statHP;
        public ORPG.Stat statSP;
        public ORPG.Stat statStr;
        public ORPG.Stat statDex;
        public ORPG.Stat statInt;
    }
}