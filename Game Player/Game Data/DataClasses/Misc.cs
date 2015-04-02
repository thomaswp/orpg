using System;
using System.Collections.Generic;
using System.Text;
using Game_Player;

namespace DataClasses
{
    [Serializable()]
    public class Misc
    {
        public int magicNumber = 0;
        public int[] partyMembers = {1};
        public string[] elements = {null, ""};
        public string[] switches = {null, ""};
        public string[] variables = {null, ""};
        public string windowSkinName = "";
        public string titleName = "";
        public string gameoverName = "";
        public string battleTransition = "";
        public AudioFile titleBgm = new AudioFile();
        public AudioFile battleBgm = new AudioFile();
        public AudioFile battleEndMe = new AudioFile();
        public AudioFile gameoverMe = new AudioFile();
        public AudioFile cursorSe = new AudioFile("", 80, 100);
        public AudioFile decisionSe = new AudioFile("", 80, 100);
        public AudioFile cancelSe = new AudioFile("", 80, 100);
        public AudioFile buzzerSe = new AudioFile("", 80, 100);
        public AudioFile equipSe = new AudioFile("", 80, 100);
        public AudioFile shopSe = new AudioFile("", 80, 100);
        public AudioFile saveSe = new AudioFile("", 80, 100);
        public AudioFile loadSe = new AudioFile("", 80, 100);
        public AudioFile battleStartSe = new AudioFile("", 80, 100);
        public AudioFile escapeSe = new AudioFile("", 80, 100);
        public AudioFile actorCollapseSe = new AudioFile("", 80, 100);
        public AudioFile enemySollapseSe = new AudioFile("", 80, 100);
        public Words words = new Words();
        public TestBattler[] testBattlers = { };
        public int testTroopId = 1;
        public int startMapId = 1;
        public int startX = 0;
        public int startY = 0;
        public string battlebackName = "";
        public string battlerName = "";
        public int battlerHue = 0;
        public int editMapId = 1;

        public Misc DeepClone()
        {
            Misc s = (Misc)this.MemberwiseClone();
            s.elements = (string[])this.elements.Clone();
            s.switches = (string[])this.switches.Clone();
            s.variables = (string[])this.variables.Clone();
            s.words = (Words)this.words.Clone();
            s.testBattlers = (TestBattler[])this.testBattlers.DeepClone();

            AudioFile[] afs = {
                                  this.titleBgm,
                                  this.battleBgm,
                                  this.battleEndMe,
                                  this.gameoverMe,
                                  this.cursorSe,
                                  this.decisionSe,
                                  this.cancelSe,
                                  this.buzzerSe,
                                  this.equipSe,
                                  this.shopSe,
                                  this.saveSe,
                                  this.loadSe,
                                  this.battleStartSe,
                                  this.escapeSe,
                                  this.actorCollapseSe,
                                  this.enemySollapseSe
                              };
            afs = (AudioFile[])afs.DeepClone();
            s.titleBgm = afs[0];
            s.battleBgm = afs[1];
            s.battleEndMe = afs[2];
            s.gameoverMe = afs[3];
            s.cursorSe = afs[4];
            s.decisionSe = afs[5];
            s.cancelSe = afs[6];
            s.buzzerSe = afs[7];
            s.equipSe = afs[8];
            s.shopSe = afs[9];
            s.saveSe = afs[10];
            s.loadSe = afs[11];
            s.battleStartSe = afs[12];
            s.escapeSe = afs[13];
            s.actorCollapseSe = afs[14];
            s.enemySollapseSe = afs[15];

            return s;
        }

        [Serializable()]
        public class TestBattler : ICloneable
        {
            public int actorId = 1;
            public int level = 1;
            public int weaponId = 0;
            public int armor1Id = 0;
            public int armor2Id = 0;
            public int armor3Id = 0;
            public int armor4Id = 0;

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }

        [Serializable()]
        public class Words : ICloneable
        {
            public string gold = "";
            public string hp = "";
            public string sp = "";
            public string str = "";
            public string dex = "";
            public string agi = "";
            public string intel = "";
            public string atk = "";
            public string pdef = "";
            public string mdef = "";
            public string weapon = "";
            public string armor1 = "";
            public string armor2 = "";
            public string armor3 = "";
            public string armor4 = "";
            public string attack = "";
            public string skill = "";
            public string guard = "";
            public string item = "";
            public string equip = "";

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }
    }
}
