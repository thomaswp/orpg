namespace ORPG
{
    partial class MapEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xnaHost = new ORPG.XNA.HostControl();
            this.SuspendLayout();
            // 
            // xnaHost
            // 
            this.xnaHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xnaHost.Location = new System.Drawing.Point(0, 0);
            this.xnaHost.Name = "xnaHost";
            this.xnaHost.Size = new System.Drawing.Size(617, 469);
            this.xnaHost.TabIndex = 0;
            this.xnaHost.Text = "hostControl1";
            this.xnaHost.MouseLeave += new System.EventHandler(this.xnaHost_MouseLeave);
            this.xnaHost.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMap_MouseMove);
            this.xnaHost.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMap_MouseDown);
            this.xnaHost.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMap_MouseUp);
            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.Controls.Add(this.xnaHost);
            this.Name = "MapEditor";
            this.Size = new System.Drawing.Size(617, 469);
            this.ResumeLayout(false);

        }

        #endregion

        private ORPG.XNA.HostControl xnaHost;



    }
}
