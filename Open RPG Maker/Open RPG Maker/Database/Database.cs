using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Game_Player;
using DataClasses;

namespace ORPG
{

    public partial class Database : Form
    {
        //public enum Tabs
        //{
        //    Hero = 0, Class = 1, Skill = 2, Item = 3, Weapon = 4, Armor = 5, Monster = 6,
        //    MonsterGroup = 7, StatusEffect = 8, Animation = 9, Tileset = 10, CommonEvent = 11,
        //    Misc = 12
        //}

        public DataArray<Actor> actors;
        public DataArray<Class> classes;
        public DataArray<Skill> skills;
        public DataArray<Item> items;
        public DataArray<Weapon> weapons;
        public DataArray<Armor> armors;
        public DataArray<Enemy> enemies;
        public DataArray<Troop> troops;
        public DataArray<State> states;
        public DataArray<Animation> animations;
        public DataArray<Tileset> tilesets;
        public DataArray<CommonEvent> commonEvents;
        public Misc misc;

        IDBTab[] dataTabs;

        public Database()
        {
            InitializeComponent();
            InitializeEvents();
        }

        void InitializeEvents()
        {
            this.Load += new EventHandler(TabChanged);
            //this.Load += new EventHandler(LoadTabs);
            this.tabControlMain.Selected += new TabControlEventHandler(TabChanged);
            this.buttonOk.Click += new EventHandler(CloseOK);
            this.buttonApply.Click += new EventHandler(Apply);
        }

        void InitializeData()
        {
            dataTabs = new IDBTab[]
            {
                this.controlHero,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            };

            actors = Data.Actors.DeepClone();
            classes = Data.Classes.DeepClone();
            skills = Data.Skills.DeepClone();
            items = Data.Items.DeepClone();
            weapons = Data.Weapons.DeepClone();
            armors = Data.Armors.DeepClone();
            enemies = Data.Enemies.DeepClone();
            troops = Data.Troops.DeepClone();
            states = Data.States.DeepClone();
            animations = Data.Animations.DeepClone();
            tilesets = Data.Tilesets.DeepClone();
            commonEvents = Data.CommonEvents.DeepClone();
            misc = Data.Misc.DeepClone();
        }

        public new DialogResult ShowDialog()
        {
            InitializeData();
            return base.ShowDialog();
        }

        public void CloseOK(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Save();
            Close();
        }

        public void CloseCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        void Apply(object sender, EventArgs e)
        {
            Save();
        }

        void TabChanged(object sender, EventArgs e)
        {
            if (dataTabs[this.tabControlMain.SelectedIndex] != null)
                dataTabs[this.tabControlMain.SelectedIndex].RefreshData();
        }

        //void LoadTabs(object sender, EventArgs e)
        //{
        //    foreach (IDBTab tab in dataTabs)
        //        if (tab != null)
        //            tab.LoadData();
        //}

        void Save()
        {
            Data.Actors = actors.DeepClone();
        }
    }
}