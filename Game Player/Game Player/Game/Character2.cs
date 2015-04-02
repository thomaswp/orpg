using System;
using System.Collections.Generic;
using System.Text;
using DataClasses;

namespace Game_Player.Game
{
    public abstract partial class Character
    {
        public virtual void Update()
        {
            if (IsJumping)
                UpdateJump();
            else if (IsMoving)
                UpdateMove();
            else
                UpdateStop();

            if (animeCount > 18 - moveSpeed * 2)
            {
                if (!stepAnime && stopCount > 0)
                    pattern = originalPattern;
                else
                    pattern = (pattern + 1) % 4;

                animeCount = 0;
            }

            if (waitCount > 0)
            {
                waitCount = -1;
                return;
            }

            if (moveRouteForcing)
            {
                MoveTypeCustom();
                return;
            }

            if (starting || IsLocked)
                return;

            if (stopCount > (40 - moveFrequency * 2) * (6 - moveFrequency))
            {
                switch (moveType)
                {
                    case 1: MoveTypeRandom(); break;
                    case 2: MoveTypeTowardPlayer(); break;
                    case 3: MoveTypeCustom(); break;
                        
                }
            }
        }

        public void UpdateJump() 
        {
            jumpCount--;

            realX = (realX * jumpCount + x * 128) / (jumpCount + 1);
            realY = (realY * jumpCount + y * 128) / (jumpCount + 1);
        }

        public void UpdateMove() 
        {
            int distance = (int)Math.Pow(2, moveSpeed);

            if (y * 128 > realY)
                realY = Math.Min(realY + distance, y * 128);

            if (x * 128 < realX)
                realX = Math.Max(realX - distance, x * 128);

            if (x * 128 > realX)
                realX = Math.Min(realX + distance, x * 128);

            if (y * 128 < realY)
                realY = Math.Max(realY - distance, y * 128);

            if (walkAnime)
                animeCount += 1.5;

            if (stepAnime)
                animeCount++;
        }

        public void UpdateStop() 
        {
            if (stepAnime)
                animeCount++;
            else if (pattern != originalPattern)
                animeCount += 1.5;

            if (!(starting || IsLocked))
                stopCount++;
        }

        public void MoveTypeRandom()
        {
            int r = Rand.Next(6);

            switch (r)
            {
                case 4: MoveForward(); break;
                case 5: stopCount = 0; break;
                default: MoveRandom(); break;
            }
        }

        public void MoveTypeTowardPlayer()
        {
            int sx = x - Globals.GamePlayer.X;
            int sy = y - Globals.GamePlayer.Y;

            int absSx = sx > 0 ? sx : -sx;
            int absSy = sy > 0 ? sy : -sy;

            if (sx + sy >= 20)
            {
                MoveRandom();
                return;
            }

            int rand = Rand.Next(6);
            switch (rand)
            {
                case 4: MoveRandom(); break;
                case 5: MoveForward(); break;
                default: MoveTowardPlayer(); break;
            }
        }

        public void MoveTypeCustom()
        {
            if (IsJumping || IsMoving)
                return;

            while (moveRouteIndex < moveRoute.list.Length)
            {
                MoveCommand command = moveRoute.list[moveRouteIndex];

                if (command.code == 0)
                {
                    if (moveRoute.repeat)
                        moveRouteIndex = 0;

                    if (!moveRoute.repeat)
                    {
                        if (moveRouteForcing && !moveRoute.repeat)
                        {
                            moveRouteForcing = false;

                            moveRoute = originalMoveRoute;
                            moveRouteIndex = originalMoveRouteIndex;
                            originalMoveRoute = null;
                        }

                        stopCount = 0;
                    }
                    return;
                }

                if (command.code <= 14)
                {
                    switch (command.code)
                    {
                        case 1: MoveDown(); break;
                        case 2: MoveLeft(); break;
                        case 3: MoveRight(); break;
                        case 4: MoveUp(); break;
                        case 5: MoveLowerLeft(); break;
                        case 6: MoveLowerRight(); break;
                        case 7: MoveUpperLeft(); break;
                        case 8: MoveUpperRight(); break;
                        case 9: MoveRandom(); break;
                        case 10: MoveTowardPlayer(); break;
                        case 11: MoveAwayFromPlayer(); break;
                        case 12: MoveForward(); break;
                        case 13: MoveBackward(); break;
                        case 14: Jump(int.Parse(command.parameters[0]), int.Parse(command.parameters[1])); break;
                    }

                    if (!moveRoute.skippable && !IsMoving && !IsJumping)
                        return;

                    moveRouteIndex++;
                    return;
                }

                if (command.code == 15)
                {
                    waitCount = int.Parse(command.parameters[0]) * 2 - 1;
                    moveRouteIndex++;
                    return;
                }

                if (command.code >= 16 && command.code <= 2)
                {
                    switch (command.code)
                    {
                        case 16: TurnDown(); break;
                        case 17: TurnLeft(); break;
                        case 18: TurnRight(); break;
                        case 19: TurnUp(); break;
                        case 20: TurnRight90(); break;
                        case 21: TurnLeft90(); break;
                        case 22: Turn180(); break;
                        case 23: TurnRightOrLeft90(); break;
                        case 24: TurnRandom(); break;
                        case 25: TurnTowardPlayer(); break;
                        case 26: TurnAwayFromPlayer(); break;
                    }

                    moveRouteIndex++;
                    return;
                }

                if (command.code >= 27)
                {
                    switch (command.code)
                    {
                        case 27:
                            Globals.GameSwitches[int.Parse(command.parameters[0])] = true;
                            Globals.GameMap.NeedRefresh = true;
                            break;
                        case 28:
                            Globals.GameSwitches[int.Parse(command.parameters[0])] = false;
                            Globals.GameMap.NeedRefresh = true;
                            break;
                        case 29: moveSpeed = int.Parse(command.parameters[0]); break;
                        case 30: moveFrequency = int.Parse(command.parameters[0]); break;
                        case 31: walkAnime = true; break;
                        case 32: walkAnime = false; break;
                        case 33: stepAnime = true; break;
                        case 34: stepAnime = false; break;
                        case 35: directionFix = true; break;
                        case 36: directionFix = false; break;
                        case 37: through = true; break;
                        case 38: through = false; break;
                        case 39: alwaysOnTop = true; break;
                        case 40: alwaysOnTop = false; break;
                        case 41:
                            tileId = 0;
                            characterName = command.parameters[0];
                            characterHue = int.Parse(command.parameters[1]);

                            if (originalDirection != int.Parse(command.parameters[2]))
                            {
                                direction = int.Parse(command.parameters[2]);
                                originalDirection = direction;
                                prelockDirection = 0;
                            }

                            if (originalPattern != int.Parse(command.parameters[2]))
                            {
                                pattern = int.Parse(command.parameters[3]);
                                originalPattern = pattern;
                            }

                            break;

                        case 42: opacity = int.Parse(command.parameters[0]); break;
                        case 43: blendType = int.Parse(command.parameters[0]); break;
                        case 44: Audio.SE.Play(command.parameters[0]); break;
                        case 45: break;//EVAL
                    }

                    moveRouteIndex++;
                }
            }
        }

        public virtual void IncreaseSteps()
        {
            stopCount = 0;
        }
    }
}
