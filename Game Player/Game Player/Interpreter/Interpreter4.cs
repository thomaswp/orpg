using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    public partial class Interpreter
    {
        //Switch Operation
        private bool Command121()
        {
            for (int i = parameters[0]; i <= parameters[1]; i++ )
                Globals.GameSwitches[i] = parameters[2] == 0;

            Globals.GameMap.NeedRefresh = true;
            return true;
        }

        //Variable Operation
        private bool Command122()
        {
            int value = 0;
            switch ((int)parameters[3])
            {
                case 0: value = parameters[4]; break;
                case 1: value = Globals.GameVariables[parameters[4]]; break;
                case 2: value = parameters[4] + Rand.Next(parameters[5] - parameters[4] + 1); break;
                case 3: value = Globals.GameParty.ItemNumber(parameters[4]); break;
                case 4:
                    Game.Actor actor = Globals.GameActors[parameters[4]];
                    if (actor != null)
                    {
                        switch ((int)parameters[5])
                        {
                            case 0: value = actor.Level; break;
                            case 1: value = actor.Exp; break;
                            case 2: value = actor.Hp; break;
                            case 3: value = actor.Sp; break;
                            case 4: value = actor.MaxHp; break;
                            case 5: value = actor.MaxSp; break;
                            case 6: value = actor.Str; break;
                            case 7: value = actor.Dex; break;
                            case 8: value = actor.Agi; break;
                            case 9: value = actor.Int; break;
                            case 10: value = actor.Atk; break;
                            case 11: value = actor.PDef; break;
                            case 12: value = actor.MDef; break;
                            case 13: value = actor.Eva; break;
                        }
                    }
                    break;
                case 5:
                    Game.Enemy enemy = Globals.GameTroop.Enemies[parameters[4]];
                    if (enemy != null)
                    {
                        switch ((int)parameters[5])
                        {
                            case 0: value = enemy.Hp; break;
                            case 1: value = enemy.Sp; break;
                            case 2: value = enemy.MaxHp; break;
                            case 3: value = enemy.MaxSp; break;
                            case 4: value = enemy.Str; break;
                            case 5: value = enemy.Dex; break;
                            case 6: value = enemy.Agi; break;
                            case 7: value = enemy.Int; break;
                            case 8: value = enemy.Atk; break;
                            case 9: value = enemy.PDef; break;
                            case 10: value = enemy.MDef; break;
                            case 11: value = enemy.Agi; break;
                        }
                    }
                    break;
                case 6:
                    Game.Character character = GetCharacter(parameters[4]);
                    switch ((int)parameters[5])
                    {
                        case 0: value = character.X; break;
                        case 1: value = character.Y; break;
                        case 2: value = character.Direction; break;
                        case 3: value = character.ScreenX(); break;
                        case 4: value = character.ScreenY(); break;
                        case 5: value = character.TerrainTag; break;
                    }
                    break;
                case 7:
                    switch ((int)parameters[4])
                    {
                        case 0: value = Globals.GameMap.MapId; break;
                        case 1: value = Globals.GameParty.Actors.Length; break;
                        case 2: value = Globals.GameParty.Gold; break;
                        case 3: value = Globals.GameParty.Steps; break;
                        case 4: value = Graphics.Playtime.Seconds; break;
                        case 5: value = Globals.GameSystem.Timer / Graphics.FPS; break;
                        case 6: value = Globals.GameSystem.SaveCount; break;
                    }
                    break;
            }

            for (int i = parameters[0]; i <= parameters[1]; i++)
            {
                switch ((int)parameters[2])
                {
                    case 0: Globals.GameVariables[i] = value; break;
                    case 1: Globals.GameVariables[i] += value; break;
                    case 2: Globals.GameVariables[i] -= value; break;
                    case 3: Globals.GameVariables[i] *= value; break;
                    case 4: 
                        if (Globals.GameVariables[i] != 0)
                            Globals.GameVariables[i] /= value; 
                        break;
                    case 5:
                        if (Globals.GameVariables[i] != 0)
                            Globals.GameVariables[i] %= value;
                        break;
                }

                Globals.GameVariables[i] = Globals.GameVariables[i].MinMax(-99999999, 99999999);
            }

            Globals.GameMap.NeedRefresh = true;
            return true;
        }

        //Self Switch Operation
        private bool Command123()
        {
            if (eventId > 0)
            {
                int[] key = new int[] {Globals.GameMap.mapId, eventId, parameters[0]};
                Globals.GameSelfSwitches[key] = (parameters[1] == 0);
            }

            Globals.GameMap.NeedRefresh = true;
            return true;
        }

        //Timer Operation
        private bool Command124()
        {
            if (parameters[0] == 0)
            {
                Globals.GameSystem.Timer = parameters[1] * Graphics.FPS;
                Globals.GameSystem.TimerWorking = true;
            }

            if (parameters[0] == 1)
                Globals.GameSystem.TimerWorking = false;

            return true;
        }

        //Gain Gold
        private bool Command125()
        {
            int value = OperateValue(parameters[0], parameters[1], parameters[2]);
            Globals.GameParty.GainGold(value);
            return true;
        }

        //Gain Item
        private bool Command126()
        {
            int value = OperateValue(parameters[1], parameters[2], parameters[3]);
            Globals.GameParty.GainItem(parameters[0], value);
            return true;
        }

        //Gain Weapon
        private bool Command127()
        {
            int value = OperateValue(parameters[1], parameters[2], parameters[3]);
            Globals.GameParty.GainWeapon(parameters[0], value);
            return true;
        }

        //Gain Armor
        private bool Command128()
        {
            int value = OperateValue(parameters[1], parameters[2], parameters[3]);
            Globals.GameParty.GainArmor(parameters[0], value);
            return true;
        }

        //Change Party
        private bool Command129()
        {
            Game.Actor actor = Globals.GameActors[parameters[0]];

            if (actor != null)
            {
                if (parameters[1] == 0)
                {
                    if (parameters[2] == 1)
                        Globals.GameActors[parameters[0]].Setup(parameters[0]);
                    Globals.GameParty.AddActor(parameters[0]);
                }
                else
                {
                    Globals.GameParty.RemoveActor(parameters[0]);
                }
            }
            return true;
        }

        //Change Windowskin
        private bool Command131()
        {
            Globals.GameSystem.WindowSkinName = parameters[0];
            return true;
        }

        //Change Battle BGM
        private bool Command132()
        {
            Globals.GameSystem.BattleBGM = parameters[0];
            return true;
        }

        //Change Battle ME
        private bool Command133()
        {
            Globals.GameSystem.BattleEndME = parameters[0];
            return true;
        }

        //Change Save Disabled
        private bool Command134()
        {
            Globals.GameSystem.SaveDisabled = (parameters[0] == 0);
            return true;
        }

        //Change Menu Disabled
        private bool Command135()
        {
            Globals.GameSystem.MenuDisabled = (parameters[0] == 0);
            return true;
        }

        //Change Encounter Disabled
        private bool Command136()
        {
            Globals.GameSystem.EncounterDisabled = (parameters[0] == 0);
            Globals.GamePlayer.MakeEncounterCount();
            return true;
        }
    }
}
