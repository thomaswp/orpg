using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Sprites
{
    //Some issues.. animation proc and bushlevel/belndtype(?) see comments
    public class Character : Sprite
    {
        //renamed to prevent same-name error
        public Game.Character Char { get; set; }

        private int tileId = 0;
        private string characterName = "";
        private int characterHue = 0;
        private int ch = 0, cw = 0;

        public Character(Viewport viewport) : this(viewport, null) { }
        public Character(Viewport viewport, Game.Character character) : base(viewport)
        {
            Char = character;
            Update();
        }

        public override void Update()
        {
            base.Update();

            if (Char == null)
                return;

            if (tileId != Char.TileId ||
                characterName != Char.CharacterName ||
                characterHue != Char.CharacterHue)
            {
                tileId = Char.TileId;
                characterName = Char.CharacterName;
                characterHue = Char.CharacterHue;


                if (tileId >= 384)
                {
                    this.Bitmap = Cache.LoadTile(Globals.GameMap.TilesetName,
                        tileId, characterHue);
                    this.BmpSourceRect = new Rect(0, 0, 32, 32);
                    this.OX = 16;
                    this.OY = 32;
                }
                else
                {
                    this.Bitmap = Cache.LoadCharacter(characterName,
                        characterHue);
                    cw = bitmap.Width / 4;
                    ch = bitmap.Height / 4;
                    this.OX = cw / 2;
                    this.OY = ch;
                }
            }

            this.Visible = !Char.Transparent;

            if (tileId == 0)
            {
                int sx = Char.Pattern * cw;
                int sy = (Char.Direction - 2) / 2 * ch;
                this.BmpSourceRect = new Rect(sx, sy, cw, ch);
            }

            this.X = Char.ScreenX();
            this.Y = Char.ScreenY();
            this.Z = Char.ScreenZ();

            this.Opactiy = Char.Opacity;
            this.BlendType = Char.BlendType;
            this.BushDepth = Char.BushDepth;

            if (Char.AnimationId != 0)
            {
                DataClasses.Animation animation = Data.Animations[Char.AnimationId];
                //proc animation call..?
                Char.AnimationId = 0;
            }
        }
    }
}
