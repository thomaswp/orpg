using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_Player;

namespace DataClasses
{
    public class Sprite : Game_Player.Sprite
    {
        private static Animation[] animations = new Animation[] { };
        private static Dictionary<Bitmap, int> referenceCount = new Dictionary<Bitmap, int>();

        private int whitenDuration = 0;
        private int appearDuration = 0;
        private int escapeDuration = 0;
        private int collapseDuration = 0;
        private int damageDuration = 0;
        private int animationDuration = 0;
        private int loopAnimationIndex = 0;
        private int blinkCount = 0;

        private Animation animation;
        private Animation loopAnimation;

        private bool blink = false;
        private bool animationHit;
        private bool oddFrame = false;

        private Sprite damageSprite;

        private Sprite[] animationSprites;
        private Sprite[] loopAnimationSprites;

        public bool IsBlinking { get { return blink; } }

        public Sprite() : this(null) { }
        public Sprite(Viewport viewport)
            : base(viewport) { }


        public override void Dispose()
        {
            base.Dispose();
            DisposeDamage();
            DisposeAnimation();
            DisposeLoopAnimation();
            base.Dispose();
        }

        public void Whiten()
        {
            this.blendType = 0;
            this.color = Colors.White;
            this.opactiy = 255;

            whitenDuration = 16;
            appearDuration = 0;
            escapeDuration = 0;
            collapseDuration = 0;
        }

        public void Appear()
        {
            this.blendType = 0;
            this.color = Colors.White;
            this.opactiy = 0;

            appearDuration = 16;
            whitenDuration = 0;
            escapeDuration = 0;
            collapseDuration = 0;
        }

        public void Escape()
        {
            this.blendType = 0;
            this.color = Colors.White;

            escapeDuration = 32;
            whitenDuration = 0;
            appearDuration = 0;
            collapseDuration = 0;
        }

        public void Collapse()
        {
            this.blendType = 1;
            this.color = new Color(255, 64, 64, 255);
            this.opactiy = 255;

            collapseDuration = 48;
            whitenDuration = 0;
            appearDuration = 0;
            escapeDuration = 0;
        }

        public void Damage(string value, bool critical)
        {
            DisposeDamage();

            bool isNumeric;
            int numericValue;
            try
            {
                numericValue = int.Parse(value);
                value = Math.Abs(numericValue).ToString();
                isNumeric = true;
            }
            catch
            {
                numericValue = -1;
                isNumeric = false;
            }

            Bitmap bitmap = new Bitmap(160, 48);
            bitmap.FontName = "Arial Black";
            bitmap.FontSize = 32;
            bitmap.FontColor = new Color(0, 0, 0);

            bitmap.DrawText(-1, 12 - 1, 160, 36, value, FontAligns.Center);
            bitmap.DrawText(1, 12 - 1, 160, 36, value, FontAligns.Center);
            bitmap.DrawText(-1, 12 + 1, 160, 36, value, FontAligns.Center);
            bitmap.DrawText(1, 12 + 1, 160, 36, value, FontAligns.Center);

            if (isNumeric && numericValue < 0)
                bitmap.FontColor = new Color(176, 255, 144);
            else
                bitmap.FontColor = Colors.White;

            bitmap.DrawText(0, 12, 160, 36, value, FontAligns.Center);

            if (critical)
            {
                bitmap.FontSize = 20;
                bitmap.FontColor = Colors.Black;
                bitmap.DrawText(-1, -1, 160, 20, "CRITICAL", FontAligns.Center);
                bitmap.DrawText(1, -1, 160, 20, "CRITICAL", FontAligns.Center);
                bitmap.DrawText(-1, 1, 160, 20, "CRITICAL", FontAligns.Center);
                bitmap.DrawText(1, 1, 160, 20, "CRITICAL", FontAligns.Center);
                bitmap.FontColor = Colors.White;
                bitmap.DrawText(0, 0, 160, 20, "CRITICAL", FontAligns.Center);
            }

            damageSprite = new Sprite(this.viewport);
            damageSprite.Bitmap = bitmap;
            damageSprite.OX = 80;
            damageSprite.OY = 20;
            damageSprite.X = this.X;
            damageSprite.Y = this.Y - this.OY / 2;
            damageSprite.Z = 3000;
            damageDuration = 40;
        }

        public void Animation(Animation animation, bool hit)
        {
            DisposeAnimation();
            this.animation = animation;
            if (animation == null) return;
            this.animationHit = hit;
            this.animationDuration = animation.frameMax;
            string animationName = animation.animationName;
            int animationHue = animation.animationHue;

            Bitmap bitmap = Cache.LoadAnimation(animationName, animationHue);
            if (referenceCount.Keys.Contains(bitmap))
                referenceCount[bitmap]++;
            else
                referenceCount.Add(bitmap, 1);
            animationSprites = new Sprite[] { };

            if (animation.position != 3 || !animations.Contains(animation))
            {
                for (int i = 0; i <= 15; i++)
                {
                    Sprite sprite = new Sprite(this.viewport);
                    sprite.bitmap = bitmap;
                    sprite.visible = false;
                    animationSprites = animationSprites.Plus<Sprite>(sprite);
                }
                if (animations.Contains(animation))
                    animations = animations.Plus<Animation>(animation);
            }

            UpdateAnimation();
        }

        public void LoopAnimation(Animation animation)
        {
            if (animation == loopAnimation) return;
            DisposeLoopAnimation();
            loopAnimation = animation;
            if (loopAnimation == null) return;
            loopAnimationIndex = 0;
            string animationName = loopAnimation.animationName;
            int animationHue = loopAnimation.animationHue;

            Bitmap bitmap = Cache.LoadAnimation(animationName, animationHue);
            if (referenceCount.Keys.Contains(bitmap))
                referenceCount[bitmap]++;
            else
                referenceCount.Add(bitmap, 1);

            loopAnimationSprites = new Sprite[] { };
            for (int i = 0; i <= 15; i++)
            {
                Sprite sprite = new Sprite(this.viewport);
                sprite.bitmap = bitmap;
                sprite.visible = false;
                loopAnimationSprites = loopAnimationSprites.Plus(sprite);
            }

            UpdateLoopAnimation();
        }

        public void DisposeDamage()
        {
            if (damageSprite != null)
            {
                damageSprite.bitmap.Dispose();
                damageSprite.Dispose();
                damageSprite = null;
                damageDuration = 0;
            }
        }

        public void DisposeAnimation()
        {
            if (animationSprites != null)
            {
                Sprite sprite = animationSprites[0];
                if (sprite != null)
                {
                    referenceCount[sprite.bitmap]--;
                    if (referenceCount[sprite.bitmap] == 0)
                        sprite.bitmap.Dispose();
                }
                foreach (Sprite s in animationSprites)
                    s.Dispose();

                animationSprites = null;
                animation = null;
            }
        }

        public void DisposeLoopAnimation()
        {
            if (loopAnimationSprites != null)
            {
                Sprite sprite = loopAnimationSprites[0];
                if (sprite != null)
                {
                    referenceCount[sprite.bitmap]--;
                    if (referenceCount[sprite.bitmap] == 0)
                        sprite.bitmap.Dispose();
                }

                foreach (Sprite s in loopAnimationSprites)
                    s.Dispose();

                loopAnimationSprites = null;
                loopAnimation = null;
            }
        }

        public void BlinkOn()
        {
            if (!blink)
            {
                blink = true;
                blinkCount = 0;
            }
        }

        public void BlinkOff()
        {
            if (blink)
            {
                blink = false;
                this.Color = Colors.Clear;
            }
        }

        public bool IsEffectOccurring()
        {
            return whitenDuration > 0 ||
                appearDuration > 0 ||
                escapeDuration > 0 ||
                collapseDuration > 0 ||
                damageDuration > 0 ||
                animationDuration > 0;
        }

        public override void Update()
        {
            base.Update();

            if (whitenDuration > 0)
            {
                whitenDuration--;
                this.Color = new Color(Color.Red, Color.Green, Color.Blue, 128 - (16 - whitenDuration) * 10);
            }
            if (appearDuration > 0)
            {
                appearDuration--;
                this.Opactiy = (16 - appearDuration) * 16;
            }
            if (escapeDuration > 0)
            {
                escapeDuration--;
                this.Opactiy = 256 - (32 - escapeDuration) * 10;
            }
            if (collapseDuration > 0)
            {
                collapseDuration--;
                this.Opactiy = 256 - (48 - collapseDuration) * 6;
            }
            if (damageDuration > 0)
            {
                damageDuration--;
                switch (damageDuration)
                {
                    case 38:
                    case 39:
                        damageSprite.Y -= 4; 
                        break;
                    case 36:
                    case 37:
                        damageSprite.Y -= 2;
                        break;
                    case 34:
                    case 35:
                        damageSprite.Y += 2;
                        break;
                    default:
                        if (damageDuration >= 28 && damageDuration <= 33)
                            damageSprite.Y += 4;
                        break;
                }

                damageSprite.Opactiy = 256 - (12 - damageDuration) * 32;

                if (damageDuration == 0)
                    DisposeDamage();
            }

            oddFrame = !oddFrame;
            if (animation != null && oddFrame)
            {
                animationDuration--;
                UpdateAnimation();
            }
            if (loopAnimation != null && oddFrame)
            {
                UpdateLoopAnimation();
                loopAnimationIndex++;
                loopAnimationIndex %= loopAnimation.frameMax;
            }
            if (blink)
            {
                blinkCount = (blinkCount + 1) % 32;
                int alpha;
                if (blinkCount < 16)
                    alpha = (16 - blinkCount) * 6;
                else
                    alpha = (blinkCount - 16) * 6;
                Color = new Color(255, 255, 255, alpha);
            }

            animations = new Animation[] { };
        }

        public void UpdateAnimation() { }
        //  def update_animation
        //    if @_animation_duration > 0
        //      frame_index = @_animation.frame_max - @_animation_duration
        //      cell_data = @_animation.frames[frame_index].cell_data
        //      position = @_animation.position
        //      animation_set_sprites(@_animation_sprites, cell_data, position)
        //      for timing in @_animation.timings
        //        if timing.frame == frame_index
        //          animation_process_timing(timing, @_animation_hit)
        //        end
        //      end
        //    else
        //      dispose_animation
        //    end
        //  end
        public void UpdateLoopAnimation() { }
        //  def update_loop_animation
        //    frame_index = @_loop_animation_index
        //    cell_data = @_loop_animation.frames[frame_index].cell_data
        //    position = @_loop_animation.position
        //    animation_set_sprites(@_loop_animation_sprites, cell_data, position)
        //    for timing in @_loop_animation.timings
        //      if timing.frame == frame_index
        //        animation_process_timing(timing, true)
        //      end
        //    end
        //  end
        //  def animation_set_sprites(sprites, cell_data, position)
        //    for i in 0..15
        //      sprite = sprites[i]
        //      pattern = cell_data[i, 0]
        //      if sprite == nil or pattern == nil or pattern == -1
        //        sprite.visible = false if sprite != nil
        //        next
        //      end
        //      sprite.visible = true
        //      sprite.src_rect.set(pattern % 5 * 192, pattern / 5 * 192, 192, 192)
        //      if position == 3
        //        if self.viewport != nil
        //          sprite.x = self.viewport.rect.width / 2
        //          sprite.y = self.viewport.rect.height - 160
        //        else
        //          sprite.x = 320
        //          sprite.y = 240
        //        end
        //      else
        //        sprite.x = self.x - self.ox + self.src_rect.width / 2
        //        sprite.y = self.y - self.oy + self.src_rect.height / 2
        //        sprite.y -= self.src_rect.height / 4 if position == 0
        //        sprite.y += self.src_rect.height / 4 if position == 2
        //      end
        //      sprite.x += cell_data[i, 1]
        //      sprite.y += cell_data[i, 2]
        //      sprite.z = 2000
        //      sprite.ox = 96
        //      sprite.oy = 96
        //      sprite.zoom_x = cell_data[i, 3] / 100.0
        //      sprite.zoom_y = cell_data[i, 3] / 100.0
        //      sprite.angle = cell_data[i, 4]
        //      sprite.mirror = (cell_data[i, 5] == 1)
        //      sprite.opacity = cell_data[i, 6] * self.opacity / 255.0
        //      sprite.blend_type = cell_data[i, 7]
        //    end
        //  end
        //  def animation_process_timing(timing, hit)
        //    if (timing.condition == 0) or
        //       (timing.condition == 1 and hit == true) or
        //       (timing.condition == 2 and hit == false)
        //      if timing.se.name != ""
        //        se = timing.se
        //        Audio.se_play("Audio/SE/" + se.name, se.volume, se.pitch)
        //      end
        //      case timing.flash_scope
        //      when 1
        //        self.flash(timing.flash_color, timing.flash_duration * 2)
        //      when 2
        //        if self.viewport != nil
        //          self.viewport.flash(timing.flash_color, timing.flash_duration * 2)
        //        end
        //      when 3
        //        self.flash(nil, timing.flash_duration * 2)
        //      end
        //    end
        //  end
        //  def x=(x)
        //    sx = x - self.x
        //    if sx != 0
        //      if @_animation_sprites != nil
        //        for i in 0..15
        //          @_animation_sprites[i].x += sx
        //        end
        //      end
        //      if @_loop_animation_sprites != nil
        //        for i in 0..15
        //          @_loop_animation_sprites[i].x += sx
        //        end
        //      end
        //    end
        //    super
        //  end
        //  def y=(y)
        //    sy = y - self.y
        //    if sy != 0
        //      if @_animation_sprites != nil
        //        for i in 0..15
        //          @_animation_sprites[i].y += sy
        //        end
        //      end
        //      if @_loop_animation_sprites != nil
        //        for i in 0..15
        //          @_loop_animation_sprites[i].y += sy
        //        end
        //      end
        //    end
        //    super
        //  end
        //end
    }
}
