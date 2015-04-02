//Just changed index to work right for DataArray<>... so things are buggy
//Also work on getting class and equips to select right

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Game_Player;
using DataClasses;
using ORPG.HeroDialogs;

namespace ORPG
{
    public partial class Hero : UserControl, IDBTab
    {
        public Database Database
        {
            get 
            {
                Control control = this;
                while (typeof(Database) != control.GetType())
                    control = control.Parent;
                return (Database)control;
            }
        }

        //temporary actors to work with until changes are confirmed
        public DataArray<Actor> Actors
        {
            get { return Database.actors; }
            set { Database.actors = value; }
        }

        //confirm dialog for deletions
        Dialogs.Confirm confirmDialog = new Dialogs.Confirm();

        //dialog to edit hero stats
        BasicStatistics statDialog = new BasicStatistics();

        //dialog to edit exp growth
        Experience expDialog = new Experience();

        //currently selected index of the listbox
        //this is also the index of the hero being edited
        int selectedIndex = 0;

        //normally when a heros stats are edited it will actomatically
        //update the hero, but internal mechanics need this to turn off
        Boolean dontAuto = false;

        List<int> weaponIds = new List<int>(), shieldIds, helmetIds, armorIds, accessoryIds;

        int[] classIds = new int[] {1, 2, 3};

        /// <summary>
        /// Initializes the control and its components.
        /// </summary>
        public Hero()
        {
            //needed call for the form
            InitializeComponent();
            //creates labels for the picture boxes holdign stats
            PictureLabels();
            //sets up events for this control
            EventHandlers();

            dontAuto = true;
            this.comboBoxClass.DataSource = classIds;
            dontAuto = false;
        }

        //the labels
        Label labelHp = new Label();
        Label labelSp = new Label();
        Label labelStr = new Label();
        Label labelDex = new Label();
        Label labelAgi = new Label();
        Label labelInt = new Label();

        /// <summary>
        /// Creates the labels on top of the picture boxes
        /// </summary>
        void PictureLabels()
        {
            //the margin of the text within the picture
            const int MARGIN = 3;

            //create each one
            labelHp.Text = "Max HP";
            labelHp.BackColor = System.Drawing.Color.Transparent;
            labelHp.Parent = this.pictureBoxHp;
            labelHp.Location = new System.Drawing.Point(MARGIN, MARGIN);

            labelSp.Text = "Max SP";
            labelSp.BackColor = System.Drawing.Color.Transparent;
            labelSp.Parent = this.pictureBoxSp;
            labelSp.Location = new System.Drawing.Point(MARGIN, MARGIN);

            labelStr.Text = "Max Str";
            labelStr.BackColor = System.Drawing.Color.Transparent;
            labelStr.Parent = this.pictureBoxStr;
            labelStr.Location = new System.Drawing.Point(MARGIN, MARGIN);

            labelDex.Text = "Max Dex";
            labelDex.BackColor = System.Drawing.Color.Transparent;
            labelDex.Parent = this.pictureBoxDex;
            labelDex.Location = new System.Drawing.Point(MARGIN, MARGIN);

            labelAgi.Text = "Max Agi";
            labelAgi.BackColor = System.Drawing.Color.Transparent;
            labelAgi.Parent = this.pictureBoxAgi;
            labelAgi.Location = new System.Drawing.Point(MARGIN, MARGIN);

            labelInt.Text = "Max Int";
            labelInt.BackColor = System.Drawing.Color.Transparent;
            labelInt.Parent = this.pictureBoxInt;
            labelInt.Location = new System.Drawing.Point(MARGIN, MARGIN);
        }

        //the reason for the overloading is that the method needs to be
        //called by events and by code
        void UpdateHero(object sender, EventArgs e) { UpdateHero(); }

        void UpdateHero()
        {
            //return if the method shouldn't be auto-updating
            if (dontAuto)
                return;

            //turn off auto update
            dontAuto = true;

            //get the actor we're editing
            Actor actor = Actors[selectedIndex + 1];

            //update various stats
            actor.name = this.textBoxName.Text;
            actor.id = this.selectedIndex + 1;
            this.listBoxHeroes.Items[selectedIndex] = actor.id.ToString("000: ") + actor.name;

            actor.classId = this.comboBoxClass.SelectedIndex.MinMax(1, 999);

            //make sure initial level is not greater than final
            if (this.numericUpDownInitLvl.Value > this.numericUpDownMaxLvl.Value)
                this.numericUpDownMaxLvl.Value = this.numericUpDownInitLvl.Value;
            actor.initialLevel = (int)this.numericUpDownInitLvl.Value;
            actor.finalLevel = (int)this.numericUpDownMaxLvl.Value;

            if (this.comboBoxWeapon.SelectedIndex >= 0)
                actor.weaponId = weaponIds[this.comboBoxWeapon.SelectedIndex];
            else
                actor.weaponId = 0;
            actor.armor1Id = this.comboBoxShield.SelectedIndex;
            actor.armor2Id = this.comboBoxHelmet.SelectedIndex;
            actor.armor3Id = this.comboBoxArmor.SelectedIndex;
            actor.armor4Id = this.comboBoxAccessory.SelectedIndex;

            actor.weaponFix = this.checkBoxWeaponL.Checked;
            actor.armor1Fix = this.checkBoxShieldL.Checked;
            actor.armor2Fix = this.checkBoxHelmetL.Checked;
            actor.armor3Fix = this.checkBoxArmorL.Checked;
            actor.armor4Fix = this.checkBoxAccessoryL.Checked;

            //turn auto update back on
            dontAuto = false;
        }

        //same reasons for overloading
        void HeroChanged(object sender, EventArgs e) { HeroChanged(); }

        void HeroChanged()
        {
            if (dontAuto)
                return;

            if (Actors == null)
                return;

            //if there are not heroes, don't edit them
            if (this.listBoxHeroes.SelectedIndex >= 0)
                selectedIndex = this.listBoxHeroes.SelectedIndex;

            dontAuto = true;

            Actor actor = Actors[selectedIndex + 1];

            this.textBoxName.Text = actor.name;

            this.numericUpDownInitLvl.Value = actor.initialLevel;
            this.numericUpDownMaxLvl.Value = actor.finalLevel;

            weaponIds.Clear();
            comboBoxWeapon.Items.Clear();
            foreach (Weapon weapon in Database.weapons)
                if (Database.classes[actor.classId].weaponSet.Includes(weapon.id))
                {
                    weaponIds.Add(weapon.id);
                    comboBoxWeapon.Items.Add(weapon.id.ToString("000") + ": " + weapon.name);
                }

            SetComboBoxIndex(this.comboBoxWeapon, weaponIds.IndexOf(actor.weaponId));

            SetComboBoxIndex(this.comboBoxShield, actor.armor1Id);
            SetComboBoxIndex(this.comboBoxHelmet, actor.armor2Id);
            SetComboBoxIndex(this.comboBoxArmor, actor.armor3Id);
            SetComboBoxIndex(this.comboBoxAccessory, actor.armor4Id);

            this.checkBoxWeaponL.Checked = actor.weaponFix;
            this.checkBoxShieldL.Checked = actor.armor1Fix;
            this.checkBoxHelmetL.Checked = actor.armor2Fix;
            this.checkBoxArmorL.Checked = actor.armor3Fix;
            this.checkBoxAccessoryL.Checked = actor.armor4Fix;

            for (int i = 0; i < actor.parameters.GetLength(0); i++)
                for (int j = 1; j < actor.parameters.GetLength(1); j++)
                {
                    statDialog.Values[i][j - 1] = actor.parameters[i, j];
                }
            this.pictureBoxHp.Image = statDialog.statHP.Image;
            this.pictureBoxSp.Image = statDialog.statSP.Image;
            this.pictureBoxStr.Image = statDialog.statStr.Image;
            this.pictureBoxDex.Image = statDialog.statDex.Image;
            this.pictureBoxAgi.Image = statDialog.statAgi.Image;
            this.pictureBoxInt.Image = statDialog.statInt.Image;

            this.buttonExpCurve.Text = 
                "Base: " + actor.expBasis + 
                "   Inflation: " + actor.expInflation;

            dontAuto = false;
        }

        //special method for setting an item to a combo box's value
        void SetComboBoxIndex(ComboBox cb, int index)
        {
            if (index > cb.Items.Count - 1 || index < 0)
            {
                cb.SelectedIndex = -1;
                cb.Text = "";
            }
            else
            {
                cb.SelectedIndex = index;
                cb.Text = cb.SelectedItem.ToString();
            }
        }

        //various events
        void EventHandlers()
        {
            this.buttonAddHero.Click += new EventHandler(AddHero);
            this.buttonRemoveHero.Click += new EventHandler(RemoveHero);

            this.pictureBoxHp.DoubleClick += new EventHandler(StatClick);
            this.pictureBoxSp.DoubleClick += new EventHandler(StatClick);
            this.pictureBoxStr.DoubleClick += new EventHandler(StatClick);
            this.pictureBoxDex.DoubleClick += new EventHandler(StatClick);
            this.pictureBoxAgi.DoubleClick += new EventHandler(StatClick);
            this.pictureBoxInt.DoubleClick += new EventHandler(StatClick);

            this.listBoxHeroes.SelectedIndexChanged += new EventHandler(HeroChanged);

            this.checkBoxAccessoryL.CheckedChanged += new EventHandler(UpdateHero);
            this.checkBoxArmorL.CheckedChanged += new EventHandler(UpdateHero);
            this.checkBoxHelmetL.CheckedChanged += new EventHandler(UpdateHero);
            this.checkBoxShieldL.CheckedChanged += new EventHandler(UpdateHero);
            this.checkBoxWeaponL.CheckedChanged += new EventHandler(UpdateHero);

            this.comboBoxAccessory.SelectedIndexChanged += new EventHandler(UpdateHero);
            this.comboBoxArmor.SelectedIndexChanged += new EventHandler(UpdateHero);
            this.comboBoxClass.SelectedIndexChanged += new EventHandler(UpdateHero);
            this.comboBoxClass.SelectedIndexChanged += new EventHandler(HeroChanged);
            this.comboBoxHelmet.SelectedIndexChanged += new EventHandler(UpdateHero);
            this.comboBoxShield.SelectedIndexChanged += new EventHandler(UpdateHero);
            this.comboBoxWeapon.SelectedIndexChanged += new EventHandler(UpdateHero);

            this.numericUpDownInitLvl.ValueChanged += new EventHandler(UpdateHero);
            this.numericUpDownMaxLvl.ValueChanged += new EventHandler(UpdateHero);

            this.textBoxName.TextChanged += new EventHandler(UpdateHero);

            this.buttonExpCurve.Click += new EventHandler(buttonExpCurve_Click);

            this.Load += new EventHandler(LoadData);
        }

        void LoadData(object sender, EventArgs e)
        {
            GenerateListBox();
        }

        void buttonExpCurve_Click(object sender, EventArgs e)
        {
            Actor actor = Actors[selectedIndex + 1];

            //set the dialog to the actor
            expDialog.ExpBase = actor.expBasis;
            expDialog.ExpSteep = actor.expInflation;

            //if ok, set the actor to the dialog
            if (expDialog.ShowDialog() == DialogResult.OK)
            {
                actor.expBasis = expDialog.ExpBase;
                actor.expInflation = expDialog.ExpSteep;
            }

            //update the button text
            this.buttonExpCurve.Text =
                "Base: " + actor.expBasis +
                "   Inflation: " + actor.expInflation;
        }

        //called when a stat box is double clicked
        void StatClick(object sender, EventArgs e)
        {
            //open the stat dialog to the appropriate stat's tab
            if (sender.Equals(this.pictureBoxHp))
                statDialog.SelectedTab = 0;
            else if (sender.Equals(this.pictureBoxSp))
                statDialog.SelectedTab = 1;
            else if (sender.Equals(this.pictureBoxStr))
                statDialog.SelectedTab = 2;
            else if (sender.Equals(this.pictureBoxDex))
                statDialog.SelectedTab = 3;
            else if (sender.Equals(this.pictureBoxAgi))
                statDialog.SelectedTab = 4;
            else if (sender.Equals(this.pictureBoxInt))
                statDialog.SelectedTab = 5;

            //get the modified actor
            Actor actor = Actors[selectedIndex + 1];

            //set the params to that of the actors
            for (int i = 0; i < actor.parameters.GetLength(0); i++)
                for (int j = 1; j < actor.parameters.GetLength(1); j++)
                {
                    statDialog.Values[i][j - 1] = actor.parameters[i, j];
                }

            //show the dialog
            //if the dialog was successful, set the actor's stats to those of the dialog
            if (statDialog.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < actor.parameters.GetLength(0); i++)
                    for (int j = 1; j < actor.parameters.GetLength(1); j++)
                    {
                        actor.parameters[i, j] = statDialog.Values[i][j - 1];
                    }

                //and update the images
                this.pictureBoxHp.Image = statDialog.statHP.Image;
                this.pictureBoxSp.Image = statDialog.statSP.Image;
                this.pictureBoxStr.Image = statDialog.statStr.Image;
                this.pictureBoxDex.Image = statDialog.statDex.Image;
                this.pictureBoxAgi.Image = statDialog.statAgi.Image;
                this.pictureBoxInt.Image = statDialog.statInt.Image;
            }
                //otherwise reset the dialog
            else
                for (int i = 0; i < actor.parameters.GetLength(0); i++)
                    for (int j = 1; j < actor.parameters.GetLength(1); j++)
                    {
                        statDialog.Values[i][j - 1] = actor.parameters[i, j];
                    }
        }

        /// <summary>
        /// Called when the tab is changed to this tab.
        /// </summary>
        public void RefreshData()
        {
            HeroChanged();
        }

        ///// <summary>
        ///// Loads the control
        ///// </summary>
        //public void LoadData()
        //{
        //    //clone the actors manually (to avoid any possible confusion)
        //    Actors = Data.Actors.DeepClone();
        //    GenerateListBox();
        //    //selected index
        //    this.listBoxHeroes.SelectedIndex = 0;
        //}

        /// <summary>
        /// Fills the list box with the heros and their ids.
        /// </summary>
        public void GenerateListBox()
        {
            this.listBoxHeroes.Items.Clear();
            foreach (Actor actor in Actors)
                this.listBoxHeroes.Items.Add(actor.id.ToString("000: ") + actor.name);
        }

        //adds a hero to the list
        void AddHero(object o, EventArgs e)
        {
            //create teh actor and add it
            Actor a = new Actor();
            a.id = this.listBoxHeroes.Items.Count;
            Actors.Add(a);
            //save the index and generate a new list box
            int index = this.listBoxHeroes.SelectedIndex;
            GenerateListBox();
            this.listBoxHeroes.SelectedIndex = index;
        }

        //removes the currently selected hero
        void RemoveHero(object o, EventArgs e)
        {
            int index = this.listBoxHeroes.SelectedIndex + 1;
            if (index < 1 || index > Actors.Length)
                return;

            Actors[index] = new Actor();
            Actors[index].id = index;
            this.listBoxHeroes.Items[index - 1] = Actors[index].id.ToString("000:");
            HeroChanged();

            ////can't delete the last one (this makes some code obsolete.. check it out)
            //if (actors.Length == 1)
            //    return;
            ////I honestly am not sure if this is needed or not... little scared to remove it
            //if (!actors[actors.Length - 1].Equals(new Actor()))
            //{
            //    //confirm dialog
            //    confirmDialog.ShowDialog("Are you sure you want to delete this Hero?");
            //    //if confirmed...
            //    if (confirmDialog.DialogResult == DialogResult.OK)
            //    {
            //        //dont auto (just in case...)
            //        dontAuto = true;

            //        //get the selected index
            //        int index = selectedIndex;
            //        //push the actors up, replacing the deleted one
            //        for (int i = index; i < actors.Length - 1; i++)
            //        {
            //            actors[i] = actors[i + 1];
            //            actors[i].id--;
            //        }
            //        //resize the array
            //        actors.Resize(actors.Length - 1);

            //        //regenerate the list box
            //        GenerateListBox();

            //        dontAuto = false;

            //        //change the index if needed
            //        if (this.listBoxHeroes.Items.Count <= index)
            //            this.listBoxHeroes.SelectedIndex = index - 1;
            //        else
            //            this.listBoxHeroes.SelectedIndex = index;
            //    }
            //}
        }
    }
}
