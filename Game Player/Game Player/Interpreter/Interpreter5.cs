using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player
{
    public partial class Interpreter
    {
        //Teleport
        private bool Command201()
        {
            if (Globals.GameTemp.inBattle)
                return true;

            if (Globals.GameTemp.playerTransferring ||
                Globals.GameTemp.messageWindowShowing ||
                Globals.GameTemp.transitionProcessing)
                return false;

            Globals.GameTemp.playerTransferring = true;

            if (parameters[0] == 0)
            {
                Globals.GameTemp.playerNewMapId = parameters[1];
                Globals.GameTemp.playerNewX = parameters[2];
                Globals.GameTemp.playerNewY = parameters[3];
                Globals.GameTemp.playerNewDirection = parameters[4];
            }
            else
            {
                Globals.GameTemp.playerNewMapId = Globals.GameVariables[parameters[1]];
                Globals.GameTemp.playerNewX = Globals.GameVariables[parameters[2]];
                Globals.GameTemp.playerNewY = Globals.GameVariables[parameters[3]];
                Globals.GameTemp.playerNewDirection = Globals.GameVariables[parameters[4]];
            }

            index++;

            if (parameters[5] == 0)
            {
                Graphics.Freeze();
                Globals.GameTemp.transitionProcessing = true;
                Globals.GameTemp.transitionName = "";
            }

            return false;
        }
       
        //Change Event Location
        private bool Command202()
        {
            if (Globals.GameTemp.inBattle) return true;

            Game.Character character = GetCharacter(parameters[0]);

            if (character == null) return true;

            if (parameters[1] == 0)
                character.MoveTo(parameters[2], parameters[3]);
            else if (parameters[1] == 1)
                character.MoveTo(Globals.GameVariables[parameters[2]],
                    Globals.GameVariables[parameters[3]]);
            else
            {
                int oldX = character.X;
                int oldY = character.Y;
                Game.Character character2 = GetCharacter(parameters[2]);
                if (character2 != null)
                {
                    character.MoveTo(character2.X, character2.Y);
                    character2.MoveTo(oldX, oldY);
                }
            }

            switch ((int)parameters[4])
            {
                case 8: character.TurnUp(); break;
                case 6: character.TurnRight(); break;
                case 2: character.TurnDown(); break;
                case 4: character.TurnLeft(); break;
            }

            return true;
        }

        //Scroll Map
        private bool Command203()
        {
            if (Globals.GameTemp.inBattle) return true;
            if (Globals.GameMap.IsScrolling) return false;

            Globals.GameMap.StartScroll(parameters[0], parameters[1], parameters[2]);
            return true;
        }

        //Change Map Settings
        private bool Command204()
        {
            switch ((int)parameters[0])
            {
                case 0:
                    Globals.GameMap.PanoramaName = parameters[1];
                    Globals.GameMap.PanoramaHue = parameters[2];
                    break;
                case 1:
                    Globals.GameMap.FogName = parameters[1];
                    Globals.GameMap.FogHue = parameters[2];
                    Globals.GameMap.FogOpacity = parameters[3];
                    Globals.GameMap.FogBlendType = parameters[4];
                    Globals.GameMap.FogZoom = parameters[5];
                    Globals.GameMap.FogSx = parameters[6];
                    Globals.GameMap.FogSy = parameters[7];
                    break;
                case 2:
                    Globals.GameMap.BattlebackName = parameters[1];
                    Globals.GameTemp.battlebackName = parameters[1];
                    break;
            }

            return true;
        }

        //Tint Fog
        private bool Command205()
        {
            Globals.GameMap.StartFogToneChange((Tone)parameters[0].Child, parameters[1] * 2); //done by 2x frames, not seconds
            return true;
        }

        //Change Fog Opacity
        private bool Command206()
        {
            Globals.GameMap.StartFogOpacityChange(parameters[0], parameters[1] * 2); //see above
            return true;
        }

        //Show Animation
        private bool Command207()
        {
            Game.Character character = GetCharacter(parameters[0]);
            if (character == null) return true;

            character.AnimationId = parameters[1];
            return true;
        }

        //Change Hero Transperancy
        private bool Command208()
        {
            Globals.GamePlayer.Transparent = (parameters[0] == 0);
            return true;
        }


        //Move Event
        private bool Command209()
        {
            Game.Character character = GetCharacter(parameters[0]);

            if (character == null)
                return true;

            character.ForceMoveRoute((DataClasses.MoveRoute)parameters[1].Child);
            return true;
        }

        //Procede With Movement
        private bool Command210()
        {
            if (!Globals.GameTemp.inBattle)
                moveRouteWaiting = true;

            return true;
        }

        //Prepare Transition
        private bool Command221()
        {
            if (Globals.GameTemp.messageWindowShowing) return false;

            Graphics.Freeze();
            return true;
        }

        //Execute Transition
        private bool Command222()
        {
            if (Globals.GameTemp.transitionProcessing) return false;

            Globals.GameTemp.transitionProcessing = true;
            Globals.GameTemp.transitionName = parameters[0];

            index++; //??
            return false;
        }

        //Tint Screen
        private bool Command223()
        {
            Globals.GameScreen.StartToneChange((Tone)parameters[0].Child, parameters[1] * 2); //^
            return true;
        }

        //Flash Screen
        private bool Command224()
        {
            Globals.GameScreen.StartFlash((Color)parameters[0].Child, parameters[1] * 2); //^
            return true;
        }

        //Shake Screen
        private bool Command225()
        {
            Globals.GameScreen.StartShake(parameters[0], parameters[1], parameters[2] * 2); //^
            return true;
        }

        //Show Picture
        private bool Command231()
        {
            int number = parameters[0] + (Globals.GameTemp.inBattle ? 50 : 0);

            int x, y;
            if (parameters[3] == 0)
            {
                x = parameters[4];
                y = parameters[5];
            }
            else
            {
                x = Globals.GameVariables[parameters[4]];
                y = Globals.GameVariables[parameters[5]];
            }

            Globals.GameScreen.Pictures[number].Show(parameters[1], parameters[2],
                x, y, parameters[6], parameters[7], parameters[8], parameters[9]);

            return true;
        }

        //Move Picture
        private bool Command232()
        {
            int number = parameters[0] + (Globals.GameTemp.inBattle ? 50 : 0);

            int x, y;
            if (parameters[3] == 0)
            {
                x = parameters[4];
                y = parameters[5];
            }
            else
            {
                x = Globals.GameVariables[parameters[4]];
                y = Globals.GameVariables[parameters[5]];
            }

            Globals.GameScreen.Pictures[number].Move(parameters[1] * 2, parameters[2], //^
                x, y, parameters[6], parameters[7], parameters[8], parameters[9]);

            return true;
        }

        //Rotate Picture
        private bool Command233()
        {
            int number = parameters[0] + (Globals.GameTemp.inBattle ? 50 : 0);
            Globals.GameScreen.Pictures[number].Roate(parameters[1]);

            return true;
        }

        //Tint Picture
        private bool Command234()
        {
            int number = parameters[0] + (Globals.GameTemp.inBattle ? 50 : 0);
            Globals.GameScreen.Pictures[number].StartToneChange((Tone)parameters[1].Child,
                parameters[2] * 2); //^

            return true;
        }

        private bool Command235()
        {
            int number = parameters[0] + (Globals.GameTemp.inBattle ? 50 : 0);
            Globals.GameScreen.Pictures[number].Erase();

            return true;
        }

        //Weather Effect
        private bool Command236()
        {
            Globals.GameScreen.Weather(parameters[0], parameters[1], parameters[2]);

            return true;
        }

        //Play BGM
        private bool Command241()
        {
            Audio.BGM.Play((AudioFile)parameters[0]);
            return true;
        }

        //Fade BGM
        private bool Command242() 
        {
            Audio.BGM.Fade(parameters[0]);
            return true;
        }

        //Play BGS
        private bool Command245()
        {
            Audio.BGS.Play((AudioFile)parameters[0]);
            return true;
        }

        //Fade BGS
        private bool Command246()
        {
            Audio.BGS.Fade(parameters[0]);
            return true;
        }

        //Memorize BGM/BGS
        private bool Command247()
        {
            //Add Memorize BGM/S
            return true;
        }

        //Restore BGM/BGS
        private bool Command248()
        {
            //Add restore
            return true;
        }

        //Play ME
        private bool Command249()
        {
            Audio.ME.Play((AudioFile)parameters[0]);
            return true;
        }

        //Play SE
        private bool Command250()
        {
            Audio.SE.Play((AudioFile)parameters[0]);
            return true;
        }

        //Stop SE
        private bool Command251()
        {
            Audio.SE.Stop();
            return true;
        }

        //Stop ME       This command doesn't actually exist... so I'll need to add it...
        private bool Command252()
        {
            Audio.ME.Stop();
            return true;
        }
    }
}
