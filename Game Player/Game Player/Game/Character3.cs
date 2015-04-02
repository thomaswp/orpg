using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Game
{
    public abstract partial class Character
    {
        public void MoveDown() { MoveDown(true); }
        public void MoveDown(bool turnEnabled)
        {
            if (turnEnabled)
                TurnDown();

            if (IsPassable(x, y, 2))
            {
                TurnDown();
                y++;
                IncreaseSteps();
            }
            else
                CheckEventTriggerTouch(x, y + 1);
        }

        public void MoveLeft() { MoveLeft(true); }
        public void MoveLeft(bool turnEnabled)
        {
            if (turnEnabled)
                TurnLeft();

            if (IsPassable(x, y, 4))
            {
                TurnLeft();
                x--;
                IncreaseSteps();
            }
            else
                CheckEventTriggerTouch(x - 1, y);
        }

        public void MoveRight() { MoveRight(true); }
        public void MoveRight(bool turnEnabled)
        {
            if (turnEnabled)
                TurnRight();

            if (IsPassable(x, y, 6))
            {
                TurnRight();
                x++;
                IncreaseSteps();
            }
            else
                CheckEventTriggerTouch(x + 1, y);
        }

        public void MoveUp() { MoveUp(true); }
        public void MoveUp(bool turnEnabled)
        {
            if (turnEnabled)
                TurnUp();

            if (IsPassable(x, y, 8))
            {
                TurnUp();
                y--;
                IncreaseSteps();
            }
            else
                CheckEventTriggerTouch(x, y - 1);
        }

        public void MoveLowerLeft()
        {
            if (!directionFix)
                direction = (direction == 6 ? 4 : direction == 8 ? 2 : direction);

            if ((IsPassable(x, y, 2) && IsPassable(x, y + 1, 4)) ||
                (IsPassable(x, y, 4) && IsPassable(x - 1, y, 2)))
            {
                x--;
                y++;
                IncreaseSteps();
            }
        }

        public void MoveLowerRight()
        {
            if (!directionFix)
                direction = (direction == 4 ? 6 : direction == 8 ? 2 : direction);

            if ((IsPassable(x, y, 2) && IsPassable(x, y + 1, 6)) ||
                (IsPassable(x, y, 6) && IsPassable(x + 1, y, 2)))
            {
                x++;
                y++;
                IncreaseSteps();
            }
        }

        public void MoveUpperLeft()
        {
            if (!directionFix)
                direction = (direction == 6 ? 4 : direction == 2 ? 8 : direction);

            if ((IsPassable(x, y, 8) && IsPassable(x, y - 1, 4)) ||
                (IsPassable(x, y, 4) && IsPassable(x - 1, y, 8)))
            {
                x--;
                y--;
                IncreaseSteps();
            }
        }

        public void MoveUpperRight()
        {
            if (!directionFix)
                direction = (direction == 4 ? 6 : direction == 2 ? 8 : direction);

            if ((IsPassable(x, y, 8) && IsPassable(x, y - 1, 6)) ||
                (IsPassable(x, y, 6) && IsPassable(x + 1, y, 8)))
            {
                x++;
                y--;
                IncreaseSteps();
            }
        }

        public void MoveRandom()
        {
            int rand = Rand.Next(4);
            switch (rand)
            {
                case 0: MoveDown(); break;
                case 1: MoveLeft(); break;
                case 2: MoveRight(); break;
                case 3: MoveUp(); break;
            }
        }

        public void MoveTowardPlayer()
        {
            int sx = x - Globals.GamePlayer.X;
            int sy = y - Globals.GamePlayer.Y;

            if (sx == 0 && sy == 0)
                return;

            int absSx = Math.Abs(sx);
            int absSy = Math.Abs(sy);

            if (absSx == absSy)
            {
                if (Rand.Next(2) == 0)
                    absSx++;
                else
                    absSy++;
            }

            if (absSx > absSy)
            {
                if (sx > 0)
                    MoveLeft();
                else
                    MoveRight();

                if (!IsMoving && sy != 0)
                {
                    if (sy > 0)
                        MoveUp();
                    else
                        MoveDown();
                }
            }
            else
            {
                if (sy > 0)
                    MoveUp();
                else
                    MoveDown();

                if (!IsMoving && sx != 0)
                {
                    if (sx > 0)
                        MoveLeft();
                    else
                        MoveRight();
                }
            }
        }

        public void MoveAwayFromPlayer()
        {
            int sx = x - Globals.GamePlayer.X;
            int sy = y - Globals.GamePlayer.Y;

            if (sx == 0 && sy == 0)
                return;

            int absSx = Math.Abs(sx);
            int absSy = Math.Abs(sy);

            if (absSx == absSy)
            {
                if (Rand.Next(2) == 0)
                    absSx++;
                else
                    absSy++;
            }

            if (absSx > absSy)
            {
                if (sx > 0)
                    MoveRight();
                else
                    MoveLeft();

                if (!IsMoving && sy != 0)
                {
                    if (sy > 0)
                        MoveDown();
                    else
                        MoveUp();
                }
            }
            else
            {
                if (sy > 0)
                    MoveDown();
                else
                    MoveUp();

                if (!IsMoving && sx != 0)
                {
                    if (sx > 0)
                        MoveRight();
                    else
                        MoveLeft();
                }
            }
        }

        public void MoveForward()
        {
            switch (direction)
            {
                case 2: MoveDown(); break;
                case 4: MoveLeft(); break;
                case 6: MoveRight(); break;
                case 8: MoveUp(); break;
            }
        }

        public void MoveBackward()
        {
            bool lastDirectionFix = directionFix;
            directionFix = true;

            switch (direction)
            {
                case 2: MoveUp(); break;
                case 4: MoveRight(); break;
                case 6: MoveLeft(); break;
                case 8: MoveDown(); break;
            }

            directionFix = lastDirectionFix;
        }

        public void Jump(int xPlus, int yPlus)
        {
            if (xPlus != 0 || yPlus != 0)
            {
                if (Math.Abs(xPlus) > Math.Abs(yPlus))
                {
                    if (xPlus < 0)
                        TurnLeft();
                    else
                        TurnRight();
                }
                else
                {
                    if (yPlus < 0)
                        TurnUp();
                    else
                        TurnDown();
                }
            }

            int newX = x + xPlus;
            int newY = y + yPlus;

            if ((xPlus == 0 && yPlus == 0) || IsPassable(newX, newY, 0))
            {
                Straighten();

                x = newX;
                y = newY;

                int distance = (int)(Math.Sqrt(xPlus * xPlus + yPlus * yPlus) + 0.5);

                jumpPeak = 10 + distance - moveSpeed;
                jumpCount = jumpPeak * 2;

                stopCount = 0;
            }
        }

        public void TurnDown()
        {
            if (!directionFix)
            {
                direction = 2;
                stopCount = 0;
            }
        }

        public void TurnLeft()
        {
            if (!directionFix)
            {
                direction = 4;
                stopCount = 0;
            }
        }

        public void TurnRight()
        {
            if (!directionFix)
            {
                direction = 6;
                stopCount = 0;
            }
        }

        public void TurnUp()
        {
            if (!directionFix)
            {
                direction = 8;
                stopCount = 0;
            }
        }

        public void TurnRight90()
        {
            switch (direction)
            {
                case 2: TurnLeft(); break;
                case 4: TurnUp(); break;
                case 6: TurnDown(); break;
                case 8: TurnRight(); break;
            }
        }

        public void TurnLeft90()
        {
            switch (direction)
            {
                case 2: TurnRight(); break;
                case 4: TurnDown(); break;
                case 6: TurnUp(); break;
                case 8: TurnLeft(); break;
            }
        }

        public void Turn180()
        {
            switch (direction)
            {
                case 2: TurnUp(); break;
                case 4: TurnRight(); break;
                case 6: TurnLeft(); break;
                case 8: TurnDown(); break;
            }
        }

        public void TurnRightOrLeft90()
        {
            if (Rand.Next(2) == 0)
                TurnRight90();
            else
                TurnLeft90();
        }

        public void TurnRandom()
        {
            int rand = Rand.Next(4);
            switch (rand)
            {
                case 0: TurnUp(); break;
                case 1: TurnRight(); break;
                case 2: TurnLeft(); break;
                case 3: TurnDown(); break;
            }
        }

        public void TurnTowardPlayer()
        {
            int sx = x - Globals.GamePlayer.X;
            int sy = y - Globals.GamePlayer.Y;

            if (sx == 0 && sy == 0)
                return;

            if (Math.Abs(sx) > Math.Abs(sy))
            {
                if (sx > 0)
                    TurnLeft();
                else
                    TurnRight();
            }
            else
            {
                if (sy > 0)
                    TurnUp();
                else
                    TurnDown();
            }
        }

        public void TurnAwayFromPlayer()
        {
            int sx = x - Globals.GamePlayer.X;
            int sy = y - Globals.GamePlayer.Y;

            if (sx == 0 && sy == 0)
                return;

            if (Math.Abs(sx) > Math.Abs(sy))
            {
                if (sx > 0)
                    TurnRight();
                else
                    TurnLeft();
            }
            else
            {
                if (sy > 0)
                    TurnDown();
                else
                    TurnUp();
            }
        }

        public abstract bool CheckEventTriggerTouch(int x, int y);
    }
}
