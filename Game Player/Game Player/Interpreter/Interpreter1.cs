using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player
{
    public partial class Interpreter
    {

        public bool IsRunning { get { return list != null; } }

        private DataClasses.EventCommand[] _list;
        private DataClasses.EventCommand[] list 
        {
            get { return _list; }
            set { _list = value; }
        }
        private int index;
        private int mapId;
        private int depth;
        private int eventId;
        private int buttonInputVariableId;
        private int waitCount;
        private int loopCount;
        private bool main;
        private bool messageWaiting;
        private bool moveRouteWaiting;
        private Interpreter childInterpreter;
        private Dictionary<int, int> branch;
        private DataClasses.Parameter parameters;
        private delegate void MessageProc(int n);

        public Interpreter() : this(0, false) { }
        public Interpreter(int depth) : this(depth, false) { }
        public Interpreter(int depth, bool main)
        {
            this.depth = depth;
            this.main = main;

            if (depth > 100)
            {
                MsgBox.Show("The common event call exceeded the upper boundary.");
                throw new Exception("The common event call exceeded the upper boundary.");
            }

            Clear();
        }

        public void Clear()
        {
            mapId = 0;
            eventId = 0;
            messageWaiting = false;
            moveRouteWaiting = false;
            buttonInputVariableId = 0;
            waitCount = 0;
            childInterpreter = null;
            branch = new Dictionary<int, int>();
        }

        public void Setup(DataClasses.EventCommand[] list, int eventId) 
        {
            Clear();
            mapId = Globals.GameMap.mapId;
            this.eventId = eventId;
            this.list = list;
            index = 0;
            branch.Clear();
        }

        public void SetupStartingEvent()
        {
            if (Globals.GameMap.NeedRefresh)
                Globals.GameMap.Refresh();

            if (Globals.GameTemp.commonEventId > 0)
            {
                Setup(Data.CommonEvents[Globals.GameTemp.commonEventId].list, 0);
                Globals.GameTemp.commonEventId = 0;
                return;
            }

            foreach (Game.Event evnt in Globals.GameMap.Events.Values)
            {
                if (evnt.Starting)
                {
                    if (evnt.Trigger < 3)
                    {
                        evnt.ClearStarting();
                        evnt.Lock();
                    }

                    Setup(evnt.List, evnt.Id);
                    return;
                }
            }

            foreach (DataClasses.CommonEvent commonEvent in Data.CommonEvents)
            {
                if (commonEvent != null) //replaces .compact
                {
                    if (commonEvent.trigger == 1 && Globals.GameSwitches[commonEvent.switchId] == true)
                    {
                        Setup(commonEvent.list, 0);
                        return;
                    }
                }
            }
        }

        public void Update()
        {
            loopCount = 0;
            while (true)
            {
                loopCount++;
                if (loopCount > 100)
                {
                    Graphics.Update();
                    loopCount = 0;
                }

                if (Globals.GameMap.mapId != mapId)
                    eventId = 0;

                if (childInterpreter != null)
                {
                    childInterpreter.Update();
                    if (!childInterpreter.IsRunning)
                        childInterpreter = null;
                    if (childInterpreter != null)
                        return;
                }

                if (messageWaiting)
                    return;

                if (moveRouteWaiting)
                {
                    if (Globals.GamePlayer.MoveRouteForcing)
                        return;

                    foreach (Game.Event evnt in Globals.GameMap.Events.Values)
                    {
                        if (evnt.MoveRouteForcing)
                            return;
                    }

                    moveRouteWaiting = false;
                }

                if (buttonInputVariableId > 0)
                {
                    InputButton();
                    return;
                }

                if (waitCount > 0)
                {
                    waitCount--;
                    return;
                }

                if (Globals.GameTemp.forcingBattler != null)
                    return;

                if (Globals.GameTemp.battleCalling ||
                    Globals.GameTemp.shopCalling ||
                    Globals.GameTemp.nameCalling ||
                    Globals.GameTemp.menuCalling ||
                    Globals.GameTemp.saveCalling ||
                    Globals.GameTemp.gameover)
                    return;

                if (list == null)
                {
                    if (main)
                        SetupStartingEvent();

                    if (list == null)
                        return;
                }

                if (ExecuteCommand() == false)
                    return;

                index++;
            }
        }

        private void InputButton()
        {
            int n = 0;

            //no the same buttons... needs changing in Input
            for (int i = 0; i <= 20; i++)
                if (Input.Triggered((Keys)i))
                    n = i;

            if (n > 0)
            {
                Globals.GameVariables[buttonInputVariableId] = n;
                Globals.GameMap.NeedRefresh = true;
                buttonInputVariableId = 0;
            }
        }

        private void SetupChoices(DataClasses.Parameter parameters)
        {
            DataClasses.Parameter choices = parameters[0];

            Globals.GameTemp.choiceMax = choices.Size;

            foreach (DataClasses.Parameter text in choices.Children)
                Globals.GameTemp.messageText += text + "\n";

            Globals.GameTemp.choiceCancelType = parameters[1];

            int currentIndent = list[index].indent;
            Globals.GameTemp.choiceProc = delegate(int n) { branch[currentIndent] = n; };
        }

        private IEnumerable<Game.Actor> IterateActor(DataClasses.Parameter parameter)
        {
            if (parameter == 0)
            {
                foreach (Game.Actor actor in Globals.GameParty.Actors)
                    yield return actor;
            }
            else
            {
                Game.Actor actor = Globals.GameActors[parameter];
                if (actor != null)
                    yield return actor;
            }
        }

        private IEnumerable<Game.Enemy> IterateEnemy(DataClasses.Parameter parameter)
        {
            if (parameter == -1)
            {
                foreach (Game.Enemy enemy in Globals.GameTroop.Enemies)
                    yield return enemy;
            }
            else
            {
                Game.Enemy enemy = Globals.GameTroop.Enemies[parameter];
                if (enemy != null)
                    yield return enemy;
            }
        }

        private IEnumerable<Game.Battler> IterateBattler(DataClasses.Parameter parameter1, DataClasses.Parameter parameter2)
        {
            if (parameter1 == 0)
            {
                foreach (Game.Enemy enemy in IterateEnemy(parameter2))
                    yield return enemy;
            }
            else
            {
                if (parameter2 == -1)
                {
                    foreach (Game.Actor actor in Globals.GameParty.Actors)
                        yield return actor;
                }
                else
                {
                    Game.Actor actor = Globals.GameParty.Actors[parameter2];
                    if (actor != null)
                        yield return actor;
                }
            }
        }

        private int CurrentBranch
        {
            get
            {
                int b;
                if (!branch.TryGetValue(list[index].indent, out b))
                    b = -1;

                return b;
            }
            set
            {
                if (branch.ContainsKey(list[index].indent))
                    branch[list[index].indent] = value;
                else
                    branch.Add(list[index].indent, value);
            }
        }
    }
}
