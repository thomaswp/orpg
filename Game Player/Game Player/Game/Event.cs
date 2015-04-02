using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Game
{
    public class Event : Character
    {
        protected int trigger;
        public int Trigger { get { return trigger; } }

        protected DataClasses.EventCommand[] list;
        public DataClasses.EventCommand[] List { get { return list; } }

        //protected bool starting;
        public bool Starting { get { return starting; } }

        public bool IsOverTrigger
        {
            get
            {
                if (characterName != "" && !through)
                    return false;

                if (!Globals.GameMap.IsPassable(x, y, 0))
                    return false;

                return true;
            }
        }

        int mapId;
        DataClasses.Event evnt;
        bool erased;
        DataClasses.Event.Page page;
        Interpreter interpreter;

        public Event(int mapId, DataClasses.Event e) : base()
        {
            this.mapId = mapId;
            this.evnt = e;
            id = e.id;
            erased = false;
            starting = false;
            through = true;

            MoveTo(evnt.x, evnt.y);
            Refresh();
        }

        public void ClearStarting()
        {
            starting = false;
        }

        public void Start()
        {
            if (list.Length > 1)
                starting = true;
        }

        public void Erase()
        {
            erased = true;
            Refresh();
        }

        public void Refresh()
        {
            DataClasses.Event.Page newPage = null;

            if (!erased)
            {
                foreach (DataClasses.Event.Page p in evnt.pages.Reverse())
                {
                    DataClasses.Event.Page.Condition c = p.condition;
                    //changed because c# doesn't have a 'next' keyword
                    bool changePage = true;

                    if (c.switch1Valid)
                        if (Globals.GameSwitches[c.switch1Id] == false)
                            changePage = false;

                    if (c.switch2Valid)
                        if (Globals.GameSwitches[c.switch2Id] == false)
                            changePage = false;

                    if (c.variableValid)
                        if (Globals.GameVariables[c.variableId] < c.variableValue)
                            changePage = false;

                    if (c.selfSwitchValid)
                    {
                        int[] key = new int[] { mapId, evnt.id, c.selfSwitchCh };

                        if (Globals.GameSelfSwitches[key] == false)
                            changePage = false;
                    }

                    if (changePage)
                    {
                        newPage = p;
                        break;
                    }
                }
            }

            if (newPage == this.page)
                return;

            page = newPage;

            ClearStarting();

            if (page == null)
            {
                tileId = 0;
                characterName = "";
                characterHue = 0;
                moveType = 0;
                through = true;
                trigger = -1; //originally null...
                list = null;
                interpreter = null;

                return;
            }

            tileId = page.graphic.tileId;
            characterName = page.graphic.characterName;
            characterHue = page.graphic.characterHue;
            if (originalDirection != page.graphic.direction)
            {
                direction = page.graphic.direction;
                originalDirection = direction;
                prelockDirection = 0;
            }
            if (originalPattern != page.graphic.pattern)
            {
                pattern = page.graphic.pattern;
                originalPattern = pattern;
            }
            opacity = page.graphic.opacity;
            blendType = page.graphic.blendType;
            moveType = page.moveType;
            moveSpeed = page.moveSpeed;
            moveFrequency = page.moveFrequency;
            moveRoute = page.moveRoute;
            moveRouteIndex = 0;
            moveRouteForcing = false;
            walkAnime = page.walkAnime;
            stepAnime = page.stepAnime;
            directionFix = page.directionFix;
            through = page.through;
            alwaysOnTop = page.alwaysOnTop;
            trigger = page.trigger;
            list = page.list;
            interpreter = null;

            if (trigger == 4)
                interpreter = new Interpreter();

            CheckEventTriggerAuto();
        }

        public override bool CheckEventTriggerTouch(int x, int y)
        {
            //return statements changed to bool's to allow override
            if (Globals.GameSystem.MapInterpreter.IsRunning)
                return false;

            if (trigger == 2 && x == Globals.GamePlayer.X && y == Globals.GamePlayer.Y)
                if (!IsJumping && !IsOverTrigger)
                    Start();

            return true;
        }

        public void CheckEventTriggerAuto()
        {
            if (trigger == 2 && x == Globals.GamePlayer.X && y == Globals.GamePlayer.Y)
                if (!IsJumping && IsOverTrigger)
                    Start();

            if (trigger == 3)
                Start();
        }

        public override void Update()
        {
            base.Update();

            CheckEventTriggerAuto();

            if (interpreter != null)
            {
                if (!interpreter.IsRunning)
                {
                    interpreter.Setup(list, evnt.id);

                    interpreter.Update();
                }
            }
        }
    }
}
