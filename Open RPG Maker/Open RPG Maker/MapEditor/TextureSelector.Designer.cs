namespace ORPG
{
    partial class TextureSelector
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
            this.pictureBoxTexture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexture)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxTexture
            // 
            this.pictureBoxTexture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTexture.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxTexture.Name = "pictureBoxTexture";
            this.pictureBoxTexture.Padding = new System.Windows.Forms.Padding(1);
            this.pictureBoxTexture.Size = new System.Drawing.Size(256, 600);
            this.pictureBoxTexture.TabIndex = 0;
            this.pictureBoxTexture.TabStop = false;
            this.pictureBoxTexture.Visible = false;
            this.pictureBoxTexture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxTexture_MouseMove);
            this.pictureBoxTexture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxTexture_MouseDown);
            this.pictureBoxTexture.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxTexture_Paint);
            this.pictureBoxTexture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxTexture_MouseUp);
            // 
            // TextureSelector
            // 
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.pictureBoxTexture);
            this.Size = new System.Drawing.Size(256, 600);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxTexture;
    }
}
