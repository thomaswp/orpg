using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player
{
    public partial class Interpreter
    {
        public bool ExecuteCommand()
        {
            if (index >= list.Length - 1)
            {
                CommandEnd();
                return true;
            }

            parameters = list[index].parameters;

            switch (list[index].code)
            {
                case 101: return Command101();
                case 102: return Command102();
                case 402: return Command402();
                case 403: return Command403();
                case 103: return Command103();
                case 104: return Command104();
                case 105: return Command105();
                case 106: return Command106();
                case 111: return Command111();
                case 411: return Command411();
                case 112: return Command112();
                case 113: return Command113();
                case 413: return Command413();
                case 115: return Command115();
                case 116: return Command116();
                case 117: return Command117();
                case 118: return Command118();
                case 119: return Command119();
                case 121: return Command121();
                case 122: return Command122();
                case 123: return Command123();
                case 124: return Command124();
                case 125: return Command125();
                case 126: return Command126();
                case 127: return Command127();
                case 128: return Command128();
                case 129: return Command129();
                case 131: return Command131();
                case 132: return Command132();
                case 133: return Command133();
                case 134: return Command134();
                case 135: return Command135();
                case 136: return Command136();
                case 201: return Command201();
                case 202: return Command202();
                case 203: return Command203();
                case 204: return Command204();
                case 205: return Command205();
                case 206: return Command206();
                case 207: return Command207();
                case 208: return Command208();
                case 209: return Command209();
                case 210: return Command210();
                case 221: return Command221();
                case 222: return Command222();
                case 223: return Command223();
                case 224: return Command224();
                case 225: return Command225();
                case 231: return Command231();
                case 232: return Command232();
                case 233: return Command233();
                case 234: return Command234();
                case 235: return Command235();
                case 236: return Command236();
                case 241: return Command241();
                case 242: return Command242();
                case 251: return Command251();
                case 252: return Command252();
            }

            return true;
        }

        public void CommandEnd()
        {
            list = null;

            if (main && eventId > 0)
                Globals.GameMap.Events[eventId].Unlock();
        }


        public bool CommandSkip()
        {
            int indent = list[index].indent;

            while (true)
            {
                if (list[index + 1].indent == indent)
                    return true;

                index++;
            }
        }


        public Game.Character GetCharacter(DataClasses.Parameter parameter)
        {
            Dictionary<int, Game.Event> events = null;

            switch ((int)parameter)
            {
                case -1: return Globals.GamePlayer;
                case 0:
                    events = Globals.GameMap.Events;
                    return events == null ? null : events[eventId];
                default:
                    events = Globals.GameMap.Events;
                    return events == null ? null : events[parameter];

            }
        }

        public int OperateValue(int operation, int operandType, int operand)
        {
            int value = 0;

            if (operandType == 0)
                value = operand;
            else
                value = Globals.GameVariables[operand];

            if (operation == 1)
                value = -value;

            return value;
        }
    }
}
