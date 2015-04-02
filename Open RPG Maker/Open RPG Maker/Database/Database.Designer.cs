namespace ORPG
{
    partial class Database
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabHero = new System.Windows.Forms.TabPage();
            this.tabClass = new System.Windows.Forms.TabPage();
            this.tabSkill = new System.Windows.Forms.TabPage();
            this.tabItem = new System.Windows.Forms.TabPage();
            this.tabWeapon = new System.Windows.Forms.TabPage();
            this.tabArmor = new System.Windows.Forms.TabPage();
            this.tabMonster = new System.Windows.Forms.TabPage();
            this.tabMonsterGroup = new System.Windows.Forms.TabPage();
            this.tabStatusEffects = new System.Windows.Forms.TabPage();
            this.tabAnimation = new System.Windows.Forms.TabPage();
            this.tabTilesets = new System.Windows.Forms.TabPage();
            this.tabCommonEvent = new System.Windows.Forms.TabPage();
            this.tabMisc = new System.Windows.Forms.TabPage();
            this.controlHero = new ORPG.Hero();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabHero.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.Controls.Add(this.buttonOk, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonCancel, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonApply, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControlMain, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(834, 589);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonOk.Location = new System.Drawing.Point(612, 559);
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
            this.buttonCancel.Location = new System.Drawing.Point(687, 559);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(69, 24);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonApply.Location = new System.Drawing.Point(762, 559);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(69, 24);
            this.buttonApply.TabIndex = 4;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            // 
            // tabControlMain
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControlMain, 4);
            this.tabControlMain.Controls.Add(this.tabHero);
            this.tabControlMain.Controls.Add(this.tabClass);
            this.tabControlMain.Controls.Add(this.tabSkill);
            this.tabControlMain.Controls.Add(this.tabItem);
            this.tabControlMain.Controls.Add(this.tabWeapon);
            this.tabControlMain.Controls.Add(this.tabArmor);
            this.tabControlMain.Controls.Add(this.tabMonster);
            this.tabControlMain.Controls.Add(this.tabMonsterGroup);
            this.tabControlMain.Controls.Add(this.tabStatusEffects);
            this.tabControlMain.Controls.Add(this.tabAnimation);
            this.tabControlMain.Controls.Add(this.tabTilesets);
            this.tabControlMain.Controls.Add(this.tabCommonEvent);
            this.tabControlMain.Controls.Add(this.tabMisc);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(3, 3);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(828, 548);
            this.tabControlMain.TabIndex = 5;
            // 
            // tabHero
            // 
            this.tabHero.Controls.Add(this.controlHero);
            this.tabHero.Location = new System.Drawing.Point(4, 22);
            this.tabHero.Name = "tabHero";
            this.tabHero.Padding = new System.Windows.Forms.Padding(3);
            this.tabHero.Size = new System.Drawing.Size(820, 522);
            this.tabHero.TabIndex = 0;
            this.tabHero.Text = "Hero";
            this.tabHero.UseVisualStyleBackColor = true;
            // 
            // tabClass
            // 
            this.tabClass.Location = new System.Drawing.Point(4, 22);
            this.tabClass.Name = "tabClass";
            this.tabClass.Padding = new System.Windows.Forms.Padding(3);
            this.tabClass.Size = new System.Drawing.Size(820, 522);
            this.tabClass.TabIndex = 1;
            this.tabClass.Text = "Class";
            this.tabClass.UseVisualStyleBackColor = true;
            // 
            // tabSkill
            // 
            this.tabSkill.Location = new System.Drawing.Point(4, 22);
            this.tabSkill.Name = "tabSkill";
            this.tabSkill.Padding = new System.Windows.Forms.Padding(3);
            this.tabSkill.Size = new System.Drawing.Size(820, 522);
            this.tabSkill.TabIndex = 2;
            this.tabSkill.Text = "Skill";
            this.tabSkill.UseVisualStyleBackColor = true;
            // 
            // tabItem
            // 
            this.tabItem.Location = new System.Drawing.Point(4, 22);
            this.tabItem.Name = "tabItem";
            this.tabItem.Padding = new System.Windows.Forms.Padding(3);
            this.tabItem.Size = new System.Drawing.Size(820, 522);
            this.tabItem.TabIndex = 3;
            this.tabItem.Text = "Item";
            this.tabItem.UseVisualStyleBackColor = true;
            // 
            // tabWeapon
            // 
            this.tabWeapon.Location = new System.Drawing.Point(4, 22);
            this.tabWeapon.Name = "tabWeapon";
            this.tabWeapon.Padding = new System.Windows.Forms.Padding(3);
            this.tabWeapon.Size = new System.Drawing.Size(820, 522);
            this.tabWeapon.TabIndex = 4;
            this.tabWeapon.Text = "Weapon";
            this.tabWeapon.UseVisualStyleBackColor = true;
            // 
            // tabArmor
            // 
            this.tabArmor.Location = new System.Drawing.Point(4, 22);
            this.tabArmor.Name = "tabArmor";
            this.tabArmor.Padding = new System.Windows.Forms.Padding(3);
            this.tabArmor.Size = new System.Drawing.Size(820, 522);
            this.tabArmor.TabIndex = 5;
            this.tabArmor.Text = "Armor";
            this.tabArmor.UseVisualStyleBackColor = true;
            // 
            // tabMonster
            // 
            this.tabMonster.Location = new System.Drawing.Point(4, 22);
            this.tabMonster.Name = "tabMonster";
            this.tabMonster.Padding = new System.Windows.Forms.Padding(3);
            this.tabMonster.Size = new System.Drawing.Size(820, 522);
            this.tabMonster.TabIndex = 6;
            this.tabMonster.Text = "Monster";
            this.tabMonster.UseVisualStyleBackColor = true;
            // 
            // tabMonsterGroup
            // 
            this.tabMonsterGroup.Location = new System.Drawing.Point(4, 22);
            this.tabMonsterGroup.Name = "tabMonsterGroup";
            this.tabMonsterGroup.Padding = new System.Windows.Forms.Padding(3);
            this.tabMonsterGroup.Size = new System.Drawing.Size(820, 522);
            this.tabMonsterGroup.TabIndex = 7;
            this.tabMonsterGroup.Text = "Monster Group";
            this.tabMonsterGroup.UseVisualStyleBackColor = true;
            // 
            // tabStatusEffects
            // 
            this.tabStatusEffects.Location = new System.Drawing.Point(4, 22);
            this.tabStatusEffects.Name = "tabStatusEffects";
            this.tabStatusEffects.Padding = new System.Windows.Forms.Padding(3);
            this.tabStatusEffects.Size = new System.Drawing.Size(820, 522);
            this.tabStatusEffects.TabIndex = 8;
            this.tabStatusEffects.Text = "Status Effects";
            this.tabStatusEffects.UseVisualStyleBackColor = true;
            // 
            // tabAnimation
            // 
            this.tabAnimation.Location = new System.Drawing.Point(4, 22);
            this.tabAnimation.Name = "tabAnimation";
            this.tabAnimation.Padding = new System.Windows.Forms.Padding(3);
            this.tabAnimation.Size = new System.Drawing.Size(820, 522);
            this.tabAnimation.TabIndex = 9;
            this.tabAnimation.Text = "Animation";
            this.tabAnimation.UseVisualStyleBackColor = true;
            // 
            // tabTilesets
            // 
            this.tabTilesets.Location = new System.Drawing.Point(4, 22);
            this.tabTilesets.Name = "tabTilesets";
            this.tabTilesets.Padding = new System.Windows.Forms.Padding(3);
            this.tabTilesets.Size = new System.Drawing.Size(820, 522);
            this.tabTilesets.TabIndex = 10;
            this.tabTilesets.Text = "Tilesets";
            this.tabTilesets.UseVisualStyleBackColor = true;
            // 
            // tabCommonEvent
            // 
            this.tabCommonEvent.Location = new System.Drawing.Point(4, 22);
            this.tabCommonEvent.Name = "tabCommonEvent";
            this.tabCommonEvent.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommonEvent.Size = new System.Drawing.Size(820, 522);
            this.tabCommonEvent.TabIndex = 11;
            this.tabCommonEvent.Text = "Common Event";
            this.tabCommonEvent.UseVisualStyleBackColor = true;
            // 
            // tabMisc
            // 
            this.tabMisc.Location = new System.Drawing.Point(4, 22);
            this.tabMisc.Name = "tabMisc";
            this.tabMisc.Padding = new System.Windows.Forms.Padding(3);
            this.tabMisc.Size = new System.Drawing.Size(820, 522);
            this.tabMisc.TabIndex = 12;
            this.tabMisc.Text = "Misc";
            this.tabMisc.UseVisualStyleBackColor = true;
            // 
            // controlHero
            // 
            this.controlHero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlHero.Location = new System.Drawing.Point(3, 3);
            this.controlHero.Name = "controlHero";
            this.controlHero.Size = new System.Drawing.Size(814, 516);
            this.controlHero.TabIndex = 0;
            // 
            // Database
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(834, 589);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(840, 615);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(840, 615);
            this.Name = "Database";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabHero.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabHero;
        private System.Windows.Forms.TabPage tabClass;
        private System.Windows.Forms.TabPage tabSkill;
        private System.Windows.Forms.TabPage tabItem;
        private System.Windows.Forms.TabPage tabWeapon;
        private System.Windows.Forms.TabPage tabArmor;
        private System.Windows.Forms.TabPage tabMonster;
        private System.Windows.Forms.TabPage tabMonsterGroup;
        private System.Windows.Forms.TabPage tabStatusEffects;
        private System.Windows.Forms.TabPage tabAnimation;
        private System.Windows.Forms.TabPage tabTilesets;
        private System.Windows.Forms.TabPage tabCommonEvent;
        private System.Windows.Forms.TabPage tabMisc;
        private Hero controlHero;
    }
}