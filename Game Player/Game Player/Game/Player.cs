using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Game
{
    public class Player : Character
    {
        const int CENTER_X = (400 - 16) * 4;
        const int CENTER_Y = (300 - 16) * 4;

        private int encounterCount;
        public int EncounterCount
        {
            get { return encounterCount; }
            set { encounterCount = value; }
        }

        public override bool IsPassable(int x, int y, int d)
        {
            int newX = x + (d == 6 ? 1 : d == 4 ? -1 : 0);
            int newY = y + (d == 2 ? 1 : d == 8 ? -1 : 0);

            if (!Globals.GameMap.IsValid(newX, newY))
                return false;

            if (Globals.DEBUG && Input.Held(Keys.Ctrl))
                return true;

            return base.IsPassable(x, y, d);
        }

        void Center(int x, int y)
        {
            int maxX = (Globals.GameMap.Width - 20) * 128;
            int maxY = (Globals.GameMap.Height - 15) * 128;
            Globals.GameMap.DisplayX = (x * 128 - CENTER_X).MinMax(0, maxX);
            Globals.GameMap.DisplayY = (y * 128 - CENTER_Y).MinMax(0, maxY);
        }

        public override void MoveTo(int x, int y)
        {
            base.MoveTo(x, y);
            Center(x, y);
            MakeEncounterCount();
        }

        public override void IncreaseSteps()
        {
            base.IncreaseSteps();

            if (!MoveRouteForcing)
            {
                Globals.GameParty.IncreaseSteps();
                if (Globals.GameParty.Steps % 2 == 0)
                    Globals.GameParty.CheckMapSlipDamage();
            }
        }

        public void MakeEncounterCount()
        {
            if (Globals.GameMap.mapId != 0)
            {
                Random rand = new Random();
                int n = Globals.GameMap.EncouterStep;
                encounterCount = Rand.Next(n) + Rand.Next(n) + 1;
            }
        }

        public void Refresh()
        {
            if (Globals.GameParty.Actors.Length == 0)
            {
                characterName = "";
                characterHue = 0;
                return;
            }

            Actor actor = Globals.GameParty.Actors[0];
            characterName = actor.CharacterName;
            characterHue = actor.CharacterHue;

            opacity = 255;
            blendType = 0;
        }

        public bool CheckEventTriggerHere(int[] triggers)
        {
            bool result = false;

            if (Globals.GameSystem.MapInterpreter.IsRunning)
                return result;

            foreach (Event e in Globals.GameMap.Events.Values)
            {
                if (e != null)
                {
                    if (e.X == X && e.Y == Y && triggers.Includes(e.Trigger))
                    {
                        if (!e.IsJumping && e.IsOverTrigger)
                        {
                            e.Start();
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        public bool CheckEventTriggerThere(int[] triggers)
        {
            bool result = false;

            if (Globals.GameSystem.MapInterpreter.IsRunning)
                return result;

            int newX = x + (direction == 6 ? 1 : direction == 4 ? -1 : 0);
            int newY = y + (direction == 2 ? 1 : direction == 8 ? -1 : 0);

            foreach (Event e in Globals.GameMap.Events.Values)
            {
                if (e != null)
                {
                    if (e.X == newX && e.Y == newY && triggers.Includes(e.Trigger))
                    {
                        if (!e.IsJumping && !e.IsOverTrigger)
                        {
                            e.Start();
                            result = true;
                        }
                    }
                }
            }

            if (!result)
            {
                if (Globals.GameMap.IsCounter(newX, newY))
                {
                    newX += (direction == 6 ? 1 : direction == 4 ? -1 : 0);
                    newY += (direction == 2 ? 1 : direction == 8 ? -1 : 0);

                    foreach (Event e in Globals.GameMap.Events.Values)
                    {
                        if (e.X == newX && e.Y == newY && triggers.Includes(e.Trigger))
                        {
                            if (!e.IsJumping && !e.IsOverTrigger)
                            {
                                e.Start();
                                result = true;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public override bool CheckEventTriggerTouch(int x, int y)
        {
            bool result = false;

            if (Globals.GameSystem.MapInterpreter.IsRunning)
                return result;

            foreach (Event e in Globals.GameMap.Events.Values)
            {
                if (e.X == x && e.Y == y && (new int[] {1, 2}).Includes(e.Trigger))
                {
                    if (!e.IsJumping && !e.IsOverTrigger)
                    {
                        e.Start();
                        result = true;
                    }
                }
            }

            return result;
        }

        public override void Update()
        {
            bool lastMoving = IsMoving;

            if (!(IsMoving || Globals.GameSystem.MapInterpreter.IsRunning ||
                MoveRouteForcing || Globals.GameTemp.messageWindowShowing))
            {
                switch (Input.Dir4())
                {
                    case 2: MoveDown(); break;
                    case 4: MoveLeft(); break;
                    case 6: MoveRight(); break;
                    case 8: MoveUp(); break;
                }
            }

            int lastRealX = realX;
            int lastRealY = realY;

            base.Update();

            if (realY > lastRealY && realY - Globals.GameMap.DisplayY > CENTER_Y)
                Globals.GameMap.ScrollDown(realY - lastRealY);

            if (realX < lastRealX && realX - Globals.GameMap.DisplayX < CENTER_X)
                Globals.GameMap.ScrollLeft(lastRealX - realX);

            if (realX > lastRealX && realX - Globals.GameMap.DisplayX > CENTER_X)
                Globals.GameMap.ScrollRight(realX - lastRealX);

            if (realY < lastRealY && realY - Globals.GameMap.DisplayY < CENTER_Y)
                Globals.GameMap.ScrollUp(lastRealY - realY);

            if (!IsMoving)
            {
                if (lastMoving)
                {
                    bool result = CheckEventTriggerHere(new int[] { 1, 2 });
                    if (!result)
                        if (!(Globals.DEBUG && Input.Held(Keys.Ctrl)))
                            if (encounterCount > 0)
                                encounterCount -= 1;
                }

                if (Input.Triggered(Keys.C))
                {
                    CheckEventTriggerHere(new int[] { 0 });
                    CheckEventTriggerThere(new int[] { 0, 1, 2 });
                }
            }

        }
    }
}
