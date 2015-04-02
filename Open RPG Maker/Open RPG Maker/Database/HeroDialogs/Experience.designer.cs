namespace ORPG.HeroDialogs
{
    partial class Experience
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageNext = new System.Windows.Forms.TabPage();
            this.expChartNext = new ORPG.HeroDialogs.ExpChart();
            this.tabPageTotal = new System.Windows.Forms.TabPage();
            this.expChartTotal = new ORPG.HeroDialogs.ExpChart();
            this.panelInput = new System.Windows.Forms.Panel();
            this.nudSteep = new System.Windows.Forms.NumericUpDown();
            this.labelSteep = new System.Windows.Forms.Label();
            this.nudBase = new System.Windows.Forms.NumericUpDown();
            this.labelBase = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageNext.SuspendLayout();
            this.tabPageTotal.SuspendLayout();
            this.panelInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSteep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBase)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(437, 364);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(69, 24);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonOk.Location = new System.Drawing.Point(362, 364);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(69, 24);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel.Controls.Add(this.buttonOk, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonCancel, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.tabControl, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.panelInput, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(509, 394);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tableLayoutPanel.SetColumnSpan(this.tabControl, 3);
            this.tabControl.Controls.Add(this.tabPageNext);
            this.tabControl.Controls.Add(this.tabPageTotal);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(3, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(503, 318);
            this.tabControl.TabIndex = 4;
            // 
            // tabPageNext
            // 
            this.tabPageNext.Controls.Add(this.expChartNext);
            this.tabPageNext.Location = new System.Drawing.Point(4, 22);
            this.tabPageNext.Name = "tabPageNext";
            this.tabPageNext.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNext.Size = new System.Drawing.Size(495, 292);
            this.tabPageNext.TabIndex = 0;
            this.tabPageNext.Text = "Next Level";
            this.tabPageNext.UseVisualStyleBackColor = true;
            // 
            // expChartNext
            // 
            this.expChartNext.Color = System.Drawing.Color.Green;
            this.expChartNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.expChartNext.ExpBase = 50;
            this.expChartNext.ExpSteep = 50;
            this.expChartNext.Location = new System.Drawing.Point(3, 3);
            this.expChartNext.Name = "expChartNext";
            this.expChartNext.Size = new System.Drawing.Size(489, 286);
            this.expChartNext.TabIndex = 0;
            this.expChartNext.Total = false;
            // 
            // tabPageTotal
            // 
            this.tabPageTotal.Controls.Add(this.expChartTotal);
            this.tabPageTotal.Location = new System.Drawing.Point(4, 22);
            this.tabPageTotal.Name = "tabPageTotal";
            this.tabPageTotal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTotal.Size = new System.Drawing.Size(495, 292);
            this.tabPageTotal.TabIndex = 1;
            this.tabPageTotal.Text = "Total";
            this.tabPageTotal.UseVisualStyleBackColor = true;
            // 
            // expChartTotal
            // 
            this.expChartTotal.Color = System.Drawing.Color.DarkOrange;
            this.expChartTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.expChartTotal.ExpBase = 50;
            this.expChartTotal.ExpSteep = 50;
            this.expChartTotal.Location = new System.Drawing.Point(3, 3);
            this.expChartTotal.Name = "expChartTotal";
            this.expChartTotal.Size = new System.Drawing.Size(489, 286);
            this.expChartTotal.TabIndex = 1;
            this.expChartTotal.Total = true;
            // 
            // panelInput
            // 
            this.tableLayoutPanel.SetColumnSpan(this.panelInput, 3);
            this.panelInput.Controls.Add(this.nudSteep);
            this.panelInput.Controls.Add(this.labelSteep);
            this.panelInput.Controls.Add(this.nudBase);
            this.panelInput.Controls.Add(this.labelBase);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInput.Location = new System.Drawing.Point(3, 327);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(503, 29);
            this.panelInput.TabIndex = 5;
            // 
            // nudSteep
            // 
            this.nudSteep.Location = new System.Drawing.Point(386, 6);
            this.nudSteep.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudSteep.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSteep.Name = "nudSteep";
            this.nudSteep.Size = new System.Drawing.Size(46, 20);
            this.nudSteep.TabIndex = 3;
            this.nudSteep.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labelSteep
            // 
            this.labelSteep.AutoSize = true;
            this.labelSteep.Location = new System.Drawing.Point(286, 8);
            this.labelSteep.Name = "labelSteep";
            this.labelSteep.Size = new System.Drawing.Size(94, 13);
            this.labelSteep.TabIndex = 2;
            this.labelSteep.Text = "Curve Steepness: ";
            // 
            // nudBase
            // 
            this.nudBase.Location = new System.Drawing.Point(158, 6);
            this.nudBase.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudBase.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBase.Name = "nudBase";
            this.nudBase.Size = new System.Drawing.Size(45, 20);
            this.nudBase.TabIndex = 1;
            this.nudBase.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labelBase
            // 
            this.labelBase.AutoSize = true;
            this.labelBase.Location = new System.Drawing.Point(85, 8);
            this.labelBase.Name = "labelBase";
            this.labelBase.Size = new System.Drawing.Size(67, 13);
            this.labelBase.TabIndex = 0;
            this.labelBase.Text = "Base Value: ";
            // 
            // Experience
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(509, 394);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(515, 420);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(515, 420);
            this.Name = "Experience";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Basic Statistics";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageNext.ResumeLayout(false);
            this.tabPageTotal.ResumeLayout(false);
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSteep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBase)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageTotal;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.NumericUpDown nudBase;
        private System.Windows.Forms.Label labelBase;
        private System.Windows.Forms.NumericUpDown nudSteep;
        private System.Windows.Forms.Label labelSteep;
        private System.Windows.Forms.TabPage tabPageNext;
        private ExpChart expChartNext;
        private ExpChart expChartTotal;

    }
}