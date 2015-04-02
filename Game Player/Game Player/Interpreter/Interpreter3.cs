using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    public partial class Interpreter
    {
        //Show Message
        private bool Command101()
        {
            if (Globals.GameTemp.messageText != "")
                return false;

            messageWaiting = true;
            Globals.GameTemp.messageProc = delegate() { messageWaiting = false; };

            Globals.GameTemp.messageText = list[index].parameters[0] + "\n";
            int lineCount = 1;

            while (true)
            {
                if (list[index + 1].code == 401)
                {
                    Globals.GameTemp.messageText += list[index + 1].parameters[0] + "\n";
                    lineCount++;
                }
                else
                {
                    if (list[index + 1].code == 102)
                    {
                        if (list[index + 1].parameters[0].Size <= 4 - lineCount)
                        {
                            index++;
                            Globals.GameTemp.choiceStart = lineCount;
                            SetupChoices(list[index].parameters);
                        }
                    }
                    else if (list[index+1].code == 103)
                    {
                        if (lineCount < 4)
                        {
                            index++;
                            Globals.GameTemp.numInputStart = lineCount;
                            Globals.GameTemp.numInputVariableId = list[index].parameters[0];
                            Globals.GameTemp.numInputDigitsMax = list[index].parameters[1];
                        }
                    }
                    return true;
                }
                index++;
            }
        }

        //Show Choices
        private bool Command102()
        {
            if (Globals.GameTemp.messageText != "")
                return false;

            messageWaiting = true;
            Globals.GameTemp.messageProc = delegate() { messageWaiting = false; };

            Globals.GameTemp.messageText = "";
            Globals.GameTemp.choiceStart = 0;
            SetupChoices(parameters);

            return true;
        }

        //Check Choice Branches
        private bool Command402()
        {
            if (CurrentBranch == parameters[0])
            {
                branch.Remove(list[index].indent);
                return true;
            }

            return CommandSkip();
        }

        //Check Choice Cancelation
        private bool Command403()
        {
            if (CurrentBranch == 4)
            {
                branch.Remove(list[index].indent);
                return true;
            }

            return CommandSkip();
        }

        //Input Number
        private bool Command103()
        {
            if (Globals.GameTemp.messageText != "")
                return false;

            messageWaiting = false;
            Globals.GameTemp.messageProc = delegate() { messageWaiting = false; };

            Globals.GameTemp.messageText = "";
            Globals.GameTemp.numInputStart = 0;
            Globals.GameTemp.numInputVariableId = parameters[0];
            Globals.GameTemp.numInputDigitsMax = parameters[1];

            return true;
        }

        //Message Position
        private bool Command104()
        {
            if (Globals.GameTemp.messageWindowShowing)
                return false;

            Globals.GameSystem.MessagePosition = parameters[0];
            Globals.GameSystem.MessageFrame = parameters[1];

            return true;
        }

        //Key Input Processing
        private bool Command105()
        {
            buttonInputVariableId = parameters[0];
            index++;

            return false;
        }

        //Wait
        private bool Command106()
        {
            waitCount = parameters[0] * 2;

            return true;
        }

        //Conditional Branch
        private bool Command111()
        {
            bool result = false;

            switch ((int)parameters[0])
            {
                case 0:
                    result = (Globals.GameSwitches[parameters[1]] == (parameters[2] == 0));
                    break;

                case 1:
                    int value1 = Globals.GameVariables[parameters[1]];

                    int value2 = 0;
                    if (parameters[2] == 0)
                        value2 = parameters[3];
                    else
                        value2 = Globals.GameVariables[parameters[3]];

                    switch ((int)parameters[4])
                    {
                        case 0: result = (value1 == value2); break;
                        case 1: result = (value1 >= value2); break;
                        case 2: result = (value1 <= value2); break;
                        case 3: result = (value1 > value2); break;
                        case 4: result = (value1 < value2); break;
                        case 5: result = (value1 != value2); break;
                    }

                    break;

                case 2:
                    if (eventId > 0)
                    {
                        int[] key = new int[] { Globals.GameMap.mapId, eventId, parameters[1] };
                        if (parameters[2] == 0)
                            result = (Globals.GameSelfSwitches[key] == true);
                        else
                            result = (Globals.GameSelfSwitches[key] != true);
                    }
                    break;

                case 3:
                    if (Globals.GameSystem.TimerWorking)
                    {
                        int sec = Graphics.Playtime.Seconds;
                        if (parameters[2] == 0)
                            result = (sec >= parameters[2]);
                        else
                            result = (sec <= parameters[2]);
                    }
                    break;

                case 4:
                    Game.Actor actor = Globals.GameActors[parameters[1]];
                    if (actor != null)
                    {
                        switch ((int)parameters[2])
                        {
                            case 0: result = (Globals.GameParty.Actors.Includes(actor)); break;
                            case 1: result = (actor.Name == parameters[3]); break;
                            case 2: result = (actor.IsSkillLearned(parameters[3])); break;
                            case 3: result = (actor.WeaponId == parameters[3]); break;
                            case 4: result = (actor.Armor1Id == parameters[3] || 
                                              actor.Armor2Id == parameters[3] || 
                                              actor.Armor3Id == parameters[3]); break;
                            case 5: result = (actor.HasState(parameters[3])); break;
                        }
                    }
                    break;

                case 5:
                    Game.Enemy enemy = Globals.GameTroop.Enemies[parameters[1]];
                    if (enemy != null)
                    {
                        switch ((int)parameters[2])
                        {
                            case 0: result = (enemy.Exists); break;
                            case 1: result = (enemy.HasState(parameters[3])); break;
                        }
                    }
                    break;

                case 6:
                    Game.Character character = GetCharacter(parameters[1]);
                    if (character != null)
                        result = (character.Direction == parameters[2]);
                    break;

                case 7:
                    if (parameters[2] == 0)
                        result = (Globals.GameParty.Gold >= parameters[1]);
                    else
                        result = (Globals.GameParty.Gold <= parameters[1]);
                    break;

                case 8: result = (Globals.GameParty.ItemNumber(parameters[1]) > 0); break;
                case 9: result = (Globals.GameParty.WeaponNumber(parameters[1]) > 0); break;
                case 10: result = (Globals.GameParty.ArmorNumber(parameters[1]) > 0); break;
                case 11: result = (Input.Held(parameters[1])); break;
                case 12: result = (bool)Code.Eval(parameters[1]); break;
            }

            CurrentBranch = result ? 1 : 0; //Modded var as int/bool to int w/ 0=false and 1=true

            if (CurrentBranch == 1) //1=true
            {
                branch.Remove(list[index].indent);
                return true;
            }

            return CommandSkip();
        }

        //Else Case Handling
        private bool Command411()
        {
            if (CurrentBranch == 0) //0=false
            {
                branch.Remove(list[index].indent);
                return true;
            }

            return CommandSkip();
        }

        //Loop
        private bool Command112()
        {
            return true;
        }

        //End Loop Data
        private bool Command413()
        {
            int indent = list[index].indent;

            while (true)
            {
                index--;
                if (list[index].indent == indent)
                    return true;
            }
        }

        //End Loop
        private bool Command113()
        {
            int indent = list[index].indent;
            int tempIndex = index;

            while (true)
            {
                tempIndex++;

                if (tempIndex >= list.Length - 1)
                    return true;

                if (list[tempIndex].code == 413 && list[tempIndex].indent < indent)
                {
                    index = tempIndex;
                    return true;
                }
            }
        }

        //End Event Processing
        private bool Command115()
        {
            CommandEnd();
            return true;
        }

        //Erase Event
        private bool Command116()
        {
            if (eventId > 0)
                Globals.GameMap.Events[eventId].Erase();

            index++;
            return false;
        }

        //Call Common Event
        private bool Command117()
        {
            DataClasses.CommonEvent commonEvent = Data.CommonEvents[parameters[0]];

            if (commonEvent != null)
            {
                childInterpreter = new Interpreter(depth + 1);
                childInterpreter.Setup(commonEvent.list, eventId);
            }

            return true;
        }

        //Label
        private bool Command118()
        {
            return true;
        }

        //Jump to Label
        private bool Command119()
        {
            string label_name = parameters[0];
            int tempIndex = 0;

            while (true)
            {
                if (tempIndex >= list.Length - 1)
                    return true;

                if (list[tempIndex].code == 118 && list[tempIndex].parameters[0] == label_name)
                {
                    index = tempIndex;
                    return true;
                }

                tempIndex++;
            }
        }
    }
}
