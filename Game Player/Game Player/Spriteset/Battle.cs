using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Spriteset
{
    public class Battle
    {
        #region properties

        protected Viewport viewport1;
        public Viewport Viewport1 { get { return viewport1; } }

        protected Viewport viewport2;
        public Viewport Viewport2 { get { return viewport2; } }

        #endregion

        private Viewport viewport3;
        private Viewport viewport4;

        private Sprite battlebackSprite;
        private Sprites.Battler[] enemySprites;

        public Battle()
        {
            Rect modifiedSR = Graphics.ScreenRect;
            modifiedSR.Height -= 160;

            viewport1 = new Viewport(modifiedSR);
            viewport2 = new Viewport(Graphics.ScreenRect);
            viewport3 = new Viewport(Graphics.ScreenRect);
            viewport4 = new Viewport(modifiedSR);

            viewport2.Z = 101;
            viewport3.Z = 200;
            viewport4.Z = 5000;

            battlebackSprite = new Sprite(viewport1);
            enemySprites = new Sprites.Battler[] { };
            foreach (Game.Enemy enemy in Globals.GameTroop.Enemies.Reverse())
            {
                enemySprites = enemySprites.Plus(new Sprites.Battler(viewport1, enemy));
            }

        }

  //def initialize
  //  # バトルバックスプライトを作成
  //  @battleback_sprite = Sprite.new(@viewport1)
  //  # エネミースプライトを作成
  //  @enemy_sprites = []
  //  for enemy in $game_troop.enemies.reverse
  //    @enemy_sprites.push(Sprite_Battler.new(@viewport1, enemy))
  //  end
  //  # アクタースプライトを作成
  //  @actor_sprites = []
  //  @actor_sprites.push(Sprite_Battler.new(@viewport2))
  //  @actor_sprites.push(Sprite_Battler.new(@viewport2))
  //  @actor_sprites.push(Sprite_Battler.new(@viewport2))
  //  @actor_sprites.push(Sprite_Battler.new(@viewport2))
  //  # 天候を作成
  //  @weather = RPG::Weather.new(@viewport1)
  //  # ピクチャスプライトを作成
  //  @picture_sprites = []
  //  for i in 51..100
  //    @picture_sprites.push(Sprite_Picture.new(@viewport3,
  //      $game_screen.pictures[i]))
  //  end
  //  # タイマースプライトを作成
  //  @timer_sprite = Sprite_Timer.new
  //  # フレーム更新
  //  update
  //end
  //#--------------------------------------------------------------------------
  //# ● 解放
  //#--------------------------------------------------------------------------
  //def dispose
  //  # バトルバックビットマップが存在していたら解放
  //  if @battleback_sprite.bitmap != nil
  //    @battleback_sprite.bitmap.dispose
  //  end
  //  # バトルバックスプライトを解放
  //  @battleback_sprite.dispose
  //  # エネミースプライト、アクタースプライトを解放
  //  for sprite in @enemy_sprites + @actor_sprites
  //    sprite.dispose
  //  end
  //  # 天候を解放
  //  @weather.dispose
  //  # ピクチャスプライトを解放
  //  for sprite in @picture_sprites
  //    sprite.dispose
  //  end
  //  # タイマースプライトを解放
  //  @timer_sprite.dispose
  //  # ビューポートを解放
  //  @viewport1.dispose
  //  @viewport2.dispose
  //  @viewport3.dispose
  //  @viewport4.dispose
  //end
  //#--------------------------------------------------------------------------
  //# ● エフェクト表示中判定
  //#--------------------------------------------------------------------------
  //def effect?
  //  # エフェクトが一つでも表示中なら true を返す
  //  for sprite in @enemy_sprites + @actor_sprites
  //    return true if sprite.effect?
  //  end
  //  return false
  //end
  //#--------------------------------------------------------------------------
  //# ● フレーム更新
  //#--------------------------------------------------------------------------
  //def update
  //  # アクタースプライトの内容を更新 (アクターの入れ替えに対応)
  //  @actor_sprites[0].battler = $game_party.actors[0]
  //  @actor_sprites[1].battler = $game_party.actors[1]
  //  @actor_sprites[2].battler = $game_party.actors[2]
  //  @actor_sprites[3].battler = $game_party.actors[3]
  //  # バトルバックのファイル名が現在のものと違う場合
  //  if @battleback_name != $game_temp.battleback_name
  //    @battleback_name = $game_temp.battleback_name
  //    if @battleback_sprite.bitmap != nil
  //      @battleback_sprite.bitmap.dispose
  //    end
  //    @battleback_sprite.bitmap = RPG::Cache.battleback(@battleback_name)
  //    @battleback_sprite.src_rect.set(0, 0, 640, 320)
  //  end
  //  # バトラースプライトを更新
  //  for sprite in @enemy_sprites + @actor_sprites
  //    sprite.update
  //  end
  //  # 天候グラフィックを更新
  //  @weather.type = $game_screen.weather_type
  //  @weather.max = $game_screen.weather_max
  //  @weather.update
  //  # ピクチャスプライトを更新
  //  for sprite in @picture_sprites
  //    sprite.update
  //  end
  //  # タイマースプライトを更新
  //  @timer_sprite.update
  //  # 画面の色調とシェイク位置を設定
  //  @viewport1.tone = $game_screen.tone
  //  @viewport1.ox = $game_screen.shake
  //  # 画面のフラッシュ色を設定
  //  @viewport4.color = $game_screen.flash_color
  //  # ビューポートを更新
  //  @viewport1.update
  //  @viewport2.update
  //  @viewport4.update
  //end
    }
}
