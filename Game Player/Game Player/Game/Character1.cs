using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataClasses;

namespace Game_Player.Game
{
    public abstract partial class Character
    {
        protected int id;
        public int Id { get { return id; } }

        protected int x;
        public int X { get { return x; } }

        protected int y;
        public int Y { get { return y; } }

        protected int realX;
        public int RealX { get { return realX; } }

        protected int realY;
        public int RealY { get { return realY; } }

        protected int tileId;
        public int TileId { get { return tileId; } }

        protected string characterName;
        public string CharacterName { get { return characterName; } }

        protected int characterHue;
        public int CharacterHue { get { return characterHue; } }

        protected int opacity;
        public int Opacity { get { return opacity; } }

        protected int blendType;
        public int BlendType { get { return blendType; } }

        protected int direction;
        public int Direction { get { return direction; } }

        protected int pattern;
        public int Pattern { get { return pattern; } }

        protected bool moveRouteForcing;
        public bool MoveRouteForcing { get { return moveRouteForcing; } }

        protected bool through;
        public bool Through { get { return through; } }

        protected int animationId;
        public int AnimationId
        {
            get { return animationId; }
            set { animationId = value; }
        }

        protected bool transparent;
        public bool Transparent
        {
            get { return transparent; }
            set { transparent = value; }
        }

        public bool IsMoving
        {
            get { return RealX != X * 128 || RealY != Y * 128; }
        }

        public bool IsJumping 
        { 
            get { return jumpCount > 0; } 
        }

        public bool IsLocked
        {
            get { return locked; }
        }
        
        public int BushDepth
        {
            get
            {
                if (tileId > 0 || alwaysOnTop)
                    return 0;

                if (jumpCount == 0 && Globals.GameMap.IsBush(x, y))
                    return 12;
                else
                    return 0;
            }
        }

        public int TerrainTag
        {
            get { return Globals.GameMap.TerrainTag(x, y); }
        }

        protected int originalDirection, originalPattern, moveType, moveSpeed, moveFrequency,
            moveRouteIndex, originalMoveRouteIndex, stopCount, jumpCount, jumpPeak,
            waitCount, prelockDirection;
        protected double animeCount;
        protected bool walkAnime, stepAnime, directionFix, alwaysOnTop, locked, starting;
        protected MoveRoute moveRoute, originalMoveRoute;

        public Character()
        {
            characterName = "";
            opacity = 255;
            direction = 2;
            originalDirection = 2;
            moveSpeed = 4;
            moveFrequency = 6;
            walkAnime = true;
        }

        public void Straighten()
        {
            if (walkAnime || stepAnime)
                pattern = 0;

            animeCount = 0;
            prelockDirection = 0;
        }

        public void ForceMoveRoute(MoveRoute moveRoute)
        {
            if (originalMoveRoute == null)
            {
                originalMoveRoute = this.moveRoute;
                originalMoveRouteIndex = moveRouteIndex;
            }

            this.moveRoute = moveRoute;
            moveRouteIndex = 0;

            moveRouteForcing = true;
            prelockDirection = 0;
            waitCount = 0;
            MoveTypeCustom();
        }

        public virtual bool IsPassable(int x, int y, int d)
        {
            if (this is Player)
            { }

            int newX = x + (d == 6 ? 1 : d == 4 ? -1 : 0);
            int newY = y + (d == 2 ? 1 : d == 8 ? -1 : 0);

            if (!Globals.GameMap.IsValid(newX, newY))
                return false;

            if (through)
                return true;

            if (!Globals.GameMap.IsPassable(x, y, d, this))
                return false;

            if (!Globals.GameMap.IsPassable(newX, newY, 10 - d))
                return false;

            foreach(Event evnt in Globals.GameMap.Events.Values)
            {
                if (evnt != null)
                {
                    if (evnt.x == newX && evnt.y == newY)
                    {
                        if (!evnt.through)
                        {
                            if (this != Globals.GamePlayer)
                                return false;

                            if (evnt.characterName != "")
                                return false;
                        }
                    }
                }
            }

            if (Globals.GamePlayer.X == newX && Globals.GamePlayer.Y == newY)
                if (!Globals.GamePlayer.through)
                    if (characterName != "")
                        return false;

            return true;
        }

        public void Lock()
        {
            if (locked)
                return;

            prelockDirection = direction;
            TurnTowardPlayer();
            locked = true;
        }

        public void Unlock()
        {
            if (!locked)
                return;

            locked = false;

            if (!directionFix)
                if (prelockDirection != 0)
                    direction = prelockDirection;
        }

        public virtual void MoveTo(int x, int y)
        {
            this.x = x % Globals.GameMap.Width;
            this.y = y % Globals.GameMap.Height;
            realX = x * 128;
            realY = y * 128;
            prelockDirection = 0;
        }

        public int ScreenX()
        {
            return (realX - Globals.GameMap.DisplayX + 3) / 4 + 16;
        }

        public int ScreenY()
        {
            int y = (realY - Globals.GameMap.DisplayY + 3) / 4 + 32;

            int n;
            if (jumpCount >= jumpPeak)
                n = jumpCount - jumpPeak;
            else
                n = jumpPeak - jumpCount;

            return y - (jumpPeak * jumpPeak - n * n) / 2;
        }

        public int ScreenZ() { return ScreenZ(0); }
        public int ScreenZ(int height)
        {
            if (alwaysOnTop)
                return 999;

            int z = (realY - Globals.GameMap.DisplayY + 3) / 4 + 32;

            if (tileId > 0)
                return z + Globals.GameMap.Priorities[tileId] * 32;
            else
                return z + ((height > 32) ? 31 : 0);
        }
    }
}
