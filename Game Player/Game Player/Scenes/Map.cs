using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Scenes
{
    public class Map : Scene
    {
        Spriteset.Map spriteset;
        Windows.Message messageWindow;

        public Map()
        {
            Graphics.Transition();

            spriteset = new Spriteset.Map();
            messageWindow = new Windows.Message();
        }
        
        public override void End()
        {
            spriteset.Dispose();
            messageWindow.Dispose();

            if (Globals.Scene is Scenes.Title)
                Graphics.Transition();
        }

        public override void Update()
        {
            while (true)
            {
                Globals.GameMap.Update();
                Globals.GameSystem.MapInterpreter.Update();
                Globals.GamePlayer.Update();

                Globals.GameScreen.Update();

                if (!Globals.GameTemp.playerTransferring)
                    break;

                TransferPlayer();

                if (Globals.GameTemp.transitionProcessing)
                    break;
            }

            spriteset.Update();
            messageWindow.Update();

            if (Globals.GameTemp.gameover)
            {
                Globals.Scene = new Scenes.Gameover();
                return;
            }

            if (Globals.GameTemp.toTitle)
            {
                Globals.Scene = new Scenes.Title();
                return;
            }

            if (Globals.GameTemp.transitionProcessing)
            {
                Globals.GameTemp.transitionProcessing = false;

                if (Globals.GameTemp.transitionName == "")
                    Graphics.Transition(20);
                else
                    Graphics.Transition(40, @"Graphics/Transitions/" +
                        Globals.GameTemp.transitionName);
            }

            if (Globals.GameTemp.messageWindowShowing)
                return;

            if (Globals.GamePlayer.EncounterCount == 0 &&
                (Globals.GameMap.EncounterList != null && Globals.GameMap.EncounterList.Length > 0))
            {
                if (!(Globals.GameSystem.MapInterpreter.IsRunning || Globals.GameSystem.EncounterDisabled))
                {
                    int n = Rand.Next(Globals.GameMap.EncounterList.Length);
                    int troopId = Globals.GameMap.EncounterList[n];

                    if (Data.Troops[troopId] != null)
                    {
                        Globals.GameTemp.battleCalling = true;
                        Globals.GameTemp.battleTroopId = troopId;
                        Globals.GameTemp.battleCanEscape = true;
                        Globals.GameTemp.battleCanLose = false;
                        //set Globals.GameTemp.BattleProc to null...
                    }
                }
            }

            if (Input.Triggered(Keys.B))
            {
                if (!(Globals.GameSystem.MapInterpreter.IsRunning ||
                    Globals.GameSystem.MenuDisabled))
                {
                    Globals.GameTemp.menuCalling = true;
                    Globals.GameTemp.menuBeep = true;
                }
            }

            if (Globals.DEBUG && Input.Held(Keys.F9))
                Globals.GameTemp.debugCalling = true;

            if (!Globals.GamePlayer.IsMoving)
            {
                if (Globals.GameTemp.battleCalling)
                    CallBattle();
                else if (Globals.GameTemp.shopCalling)
                    CallShop();
                else if (Globals.GameTemp.nameCalling)
                    CallName();
                else if (Globals.GameTemp.menuCalling)
                    CallMenu();
                else if (Globals.GameTemp.saveCalling)
                    CallSave();
                else if (Globals.GameTemp.debugCalling)
                    CallDebug();
            }
        }

        public void CallBattle()
        {
            Globals.GameTemp.battleCalling = false;

            Globals.GameTemp.menuCalling = false;
            Globals.GameTemp.menuBeep = false;

            Globals.GamePlayer.MakeEncounterCount();

            Globals.GameTemp.mapBgm = Audio.BGM.Playing;
            Audio.BGM.Stop();

            Audio.SE.Play(Data.Misc.battleStartSe);
            Audio.BGM.Play(Data.Misc.battleBgm);

            Globals.GamePlayer.Straighten();

            Globals.Scene = new Scenes.Battle();
        }

        public void CallShop()
        {
            Globals.GameTemp.shopCalling = false;
            Globals.GamePlayer.Straighten();
            Globals.Scene = new Scenes.Shop();
        }

        public void CallName()
        {
            Globals.GameTemp.nameCalling = false;
            Globals.GamePlayer.Straighten();
            Globals.Scene = new Scenes.Name();
        }

        public void CallMenu()
        {
            Globals.GameTemp.menuCalling = false;

            if (Globals.GameTemp.menuBeep)
            {
                Audio.SE.Play(Data.Misc.decisionSe);
                Globals.GameTemp.menuBeep = false;
            }

            Globals.GamePlayer.Straighten();
            Globals.Scene = new Scenes.Menu();
        }

        public void CallSave()
        {
            Globals.GamePlayer.Straighten();
            Globals.Scene = new Scenes.Save();
        }

        public void CallDebug()
        {
            Globals.GameTemp.debugCalling = false;
            Audio.SE.Play(Data.Misc.decisionSe);
            Globals.GamePlayer.Straighten();
            Globals.Scene = new Scenes.Debug();
        }

        public void TransferPlayer()
        {
            Globals.GameTemp.playerTransferring = false;

            if (Globals.GameMap.mapId != Globals.GameTemp.playerNewMapId)
                Globals.GameMap.Setup(Globals.GameTemp.playerNewMapId);

            Globals.GamePlayer.MoveTo(Globals.GameTemp.playerNewX, Globals.GameTemp.playerNewY);

            switch (Globals.GameTemp.playerNewDirection)
            {
                case 2: Globals.GamePlayer.TurnDown(); break;
                case 4: Globals.GamePlayer.TurnLeft(); break;
                case 6: Globals.GamePlayer.TurnRight(); break;
                case 8: Globals.GamePlayer.TurnUp(); break;
            }

            Globals.GamePlayer.Straighten();

            Globals.GameMap.Update();

            spriteset.Dispose();
            spriteset = new Spriteset.Map();

            if (Globals.GameTemp.transitionProcessing)
            {
                Globals.GameTemp.transitionProcessing = false;
                Graphics.Transition(20);
            }

            Globals.GameMap.Autoplay();

            Input.Update();
        }
    }
}
