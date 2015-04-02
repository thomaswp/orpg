namespace HueTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonRed = new System.Windows.Forms.Button();
            this.buttonGreen = new System.Windows.Forms.Button();
            this.buttonBlue = new System.Windows.Forms.Button();
            this.buttonAlpha = new System.Windows.Forms.Button();
            this.nudHue = new System.Windows.Forms.NumericUpDown();
            this.labelColors = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(127, 182);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // buttonRed
            // 
            this.buttonRed.Location = new System.Drawing.Point(181, 154);
            this.buttonRed.Name = "buttonRed";
            this.buttonRed.Size = new System.Drawing.Size(75, 23);
            this.buttonRed.TabIndex = 1;
            this.buttonRed.Text = "Red";
            this.buttonRed.UseVisualStyleBackColor = true;
            // 
            // buttonGreen
            // 
            this.buttonGreen.Location = new System.Drawing.Point(181, 183);
            this.buttonGreen.Name = "buttonGreen";
            this.buttonGreen.Size = new System.Drawing.Size(75, 23);
            this.buttonGreen.TabIndex = 2;
            this.buttonGreen.Text = "Green";
            this.buttonGreen.UseVisualStyleBackColor = true;
            // 
            // buttonBlue
            // 
            this.buttonBlue.Location = new System.Drawing.Point(181, 212);
            this.buttonBlue.Name = "buttonBlue";
            this.buttonBlue.Size = new System.Drawing.Size(75, 23);
            this.buttonBlue.TabIndex = 3;
            this.buttonBlue.Text = "Blue";
            this.buttonBlue.UseVisualStyleBackColor = true;
            // 
            // buttonAlpha
            // 
            this.buttonAlpha.Location = new System.Drawing.Point(181, 241);
            this.buttonAlpha.Name = "buttonAlpha";
            this.buttonAlpha.Size = new System.Drawing.Size(75, 23);
            this.buttonAlpha.TabIndex = 4;
            this.buttonAlpha.Text = "Alpha";
            this.buttonAlpha.UseVisualStyleBackColor = true;
            // 
            // nudHue
            // 
            this.nudHue.Location = new System.Drawing.Point(181, 128);
            this.nudHue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudHue.Name = "nudHue";
            this.nudHue.Size = new System.Drawing.Size(75, 20);
            this.nudHue.TabIndex = 5;
            this.nudHue.ValueChanged += new System.EventHandler(this.nudHue_ValueChanged);
            // 
            // labelColors
            // 
            this.labelColors.AutoSize = true;
            this.labelColors.Location = new System.Drawing.Point(163, 12);
            this.labelColors.Name = "labelColors";
            this.labelColors.Size = new System.Drawing.Size(0, 13);
            this.labelColors.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 268);
            this.Controls.Add(this.labelColors);
            this.Controls.Add(this.nudHue);
            this.Controls.Add(this.buttonAlpha);
            this.Controls.Add(this.buttonBlue);
            this.Controls.Add(this.buttonGreen);
            this.Controls.Add(this.buttonRed);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonRed;
        private System.Windows.Forms.Button buttonGreen;
        private System.Windows.Forms.Button buttonBlue;
        private System.Windows.Forms.Button buttonAlpha;
        private System.Windows.Forms.NumericUpDown nudHue;
        private System.Windows.Forms.Label labelColors;
    }
}

