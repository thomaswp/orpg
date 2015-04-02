using System;
using System.Text;
using System.IO;

namespace Game_Player.Game
{
    public class System
    {
#region Properties
        Interpreter mapInterpreter = new Interpreter(0, true);
        public Interpreter MapInterpreter
        {
            get { return mapInterpreter; }
        }

        Interpreter battleInterpreter = new Interpreter(0, false);
        public Interpreter BattleInterpreter
        {
            get { return battleInterpreter; }
        }

        int timer;
        public int Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        bool timerWorking;
        public bool TimerWorking
        {
            get { return timerWorking; }
            set { timerWorking = value; }
        }

        bool saveDisabled;
        public bool SaveDisabled
        {
            get { return saveDisabled; }
            set { saveDisabled = value; }
        }

        bool menuDisabled;
        public bool MenuDisabled
        {
            get { return menuDisabled; }
            set { menuDisabled = value; }
        }

        bool encounterDisabled;
        public bool EncounterDisabled
        {
            get { return encounterDisabled; }
            set { encounterDisabled = value; }
        }

        int messagePosition = 2;
        public int MessagePosition
        {
            get { return messagePosition; }
            set { messagePosition = value; }
        }

        int messageFrame;
        public int MessageFrame
        {
            get { return messageFrame; }
            set { messageFrame = value; }
        }

        int saveCount;
        public int SaveCount
        {
            get { return saveCount; }
            set { saveCount = value; }
        }

        int magicNumber;
        public int MagicNumber
        {
            get { return magicNumber; }
            set { magicNumber = value; }
        }

        string windowskinName;
        public string WindowSkinName
        {
            get 
            { 
                if (windowskinName == String.Empty || windowskinName == null) 
                { return Data.Misc.windowSkinName; } 
                else { return windowskinName; } 
            }
            set { windowskinName = value; }
        }

        private AudioFile battleBGM;
        public AudioFile BattleBGM
        {
            get { return battleBGM == null ? Data.Misc.battleBgm : battleBGM; }
            set { battleBGM = value; }
        }

        private AudioFile battleEndME;
        public AudioFile BattleEndME
        {
            get { return battleEndME == null ? Data.Misc.battleEndMe : battleEndME; }
            set { battleEndME = value; }
        }
#endregion

       public System()
        {
            mapInterpreter = new Interpreter(0, true);
            battleInterpreter = new Interpreter(0, false);
            timer = 0;
            timerWorking = false;
            saveDisabled = false;
            menuDisabled = false;
            encounterDisabled = false;
            messagePosition = 2;
            messageFrame = 0;
            saveCount = 0;
            magicNumber = 0;
            windowskinName = "";
        }

        public void GameStart()
        {
            Graphics.FontFace = "Arial";
            Graphics.FontSize = 16;
            Paths.Load(Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\");
            Code.Assemblies.Add("Game Player.exe");
            Code.Assemblies.Add("Game Data.dll");
            Globals.Scene = new Scenes.Title();
        }

        public bool Update()
        {
            if (TimerWorking)
                Timer -= 1;

            Input.Update();
            Audio.Update();
            Graphics.Update();
            if (Globals.Scene != null)
            {
                Globals.Scene.Update();
                return true;
            }
            return Graphics.Transitioning;
        }

 
    }
}
