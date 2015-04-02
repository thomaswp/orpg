using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Scenes
{
    public partial class Battle : Scene
    {
        private int troopId;
        private int waitCount;

        private Windows.Command actorCommandWindow;
        private Windows.PartyCommand partyCommandWindow;
        private Windows.Help helpWindow;
        private Windows.BattleStatus statusWindow;
        private Windows.Message messageWindow;

        private Spriteset.Battle spriteset;

        public Battle()
        {
            Globals.GameTemp.inBattle = true;
            Globals.GameTemp.battleTurn = 0;
            Globals.GameTemp.battleEventFlags = new bool[] { };
            Globals.GameTemp.battleAbort = false;
            Globals.GameTemp.battleMainPhase = false;
            Globals.GameTemp.battlebackName = Globals.GameMap.BattlebackName;
            Globals.GameTemp.forcingBattler = null;
            Globals.GameSystem.BattleInterpreter.Setup(null, 0);

            troopId = Globals.GameTemp.battleTroopId;
            Globals.GameTroop.Setup(troopId);

            string s1 = Data.Misc.words.attack;
            string s2 = Data.Misc.words.skill;
            string s3 = Data.Misc.words.guard;
            string s4 = Data.Misc.words.item;

            actorCommandWindow = new Game_Player.Windows.Command(160, new string[] { s1, s2, s3, s4 });
            actorCommandWindow.Y = 160;
            actorCommandWindow.BackOpacity = 160;
            actorCommandWindow.Active = false;
            actorCommandWindow.Visible = false;

            partyCommandWindow = new Windows.PartyCommand();

            helpWindow = new Game_Player.Windows.Help();
            helpWindow.BackOpacity = 160;
            helpWindow.Visible = false;

            statusWindow = new Windows.BattleStatus();
            messageWindow = new Game_Player.Windows.Message();

            spriteset = new Spriteset.Battle();

            waitCount = 0;

            if (Data.Misc.battleTransition == "")
                Graphics.Transition(20);
            else
                Graphics.Transition(40, Data.Misc.battleTransition);

            //StartPhase1();
        }

        public override void End() 
        { 
    //        $game_map.refresh
    //# トランジション準備
    //Graphics.freeze
    //# ウィンドウを解放
    //@actor_command_window.dispose
    //@party_command_window.dispose
    //@help_window.dispose
    //@status_window.dispose
    //@message_window.dispose
    //if @skill_window != nil
    //  @skill_window.dispose
    //end
    //if @item_window != nil
    //  @item_window.dispose
    //end
    //if @result_window != nil
    //  @result_window.dispose
    //end
    //# スプライトセットを解放
    //@spriteset.dispose
    //# タイトル画面に切り替え中の場合
    //if $scene.is_a?(Scene_Title)
    //  # 画面をフェードアウト
    //  Graphics.transition
    //  Graphics.freeze
    //end
    //# 戦闘テストからゲームオーバー画面以外に切り替え中の場合
    //if $BTEST and not $scene.is_a?(Scene_Gameover)
    //  $scene = nil
    //end
        }

  //      def judge
  //  # 全滅判定が真、またはパーティ人数が 0 人の場合
  //  if $game_party.all_dead? or $game_party.actors.size == 0
  //    # 敗北可能の場合
  //    if $game_temp.battle_can_lose
  //      # バトル開始前の BGM に戻す
  //      $game_system.bgm_play($game_temp.map_bgm)
  //      # バトル終了
  //      battle_end(2)
  //      # true を返す
  //      return true
  //    end
  //    # ゲームオーバーフラグをセット
  //    $game_temp.gameover = true
  //    # true を返す
  //    return true
  //  end
  //  # エネミーが 1 体でも存在すれば false を返す
  //  for enemy in $game_troop.enemies
  //    if enemy.exist?
  //      return false
  //    end
  //  end
  //  # アフターバトルフェーズ開始 (勝利)
  //  start_phase5
  //  # true を返す
  //  return true
  //end
  //#--------------------------------------------------------------------------
  //# ● バトル終了
  //#     result : 結果 (0:勝利 1:敗北 2:逃走)
  //#--------------------------------------------------------------------------
  //def battle_end(result)
  //  # 戦闘中フラグをクリア
  //  $game_temp.in_battle = false
  //  # パーティ全員のアクションをクリア
  //  $game_party.clear_actions
  //  # バトル用ステートを解除
  //  for actor in $game_party.actors
  //    actor.remove_states_battle
  //  end
  //  # エネミーをクリア
  //  $game_troop.enemies.clear
  //  # バトル コールバックを呼ぶ
  //  if $game_temp.battle_proc != nil
  //    $game_temp.battle_proc.call(result)
  //    $game_temp.battle_proc = nil
  //  end
  //  # マップ画面に切り替え
  //  $scene = Scene_Map.new
  //end
  //#--------------------------------------------------------------------------
  //# ● バトルイベントのセットアップ
  //#--------------------------------------------------------------------------
  //def setup_battle_event
  //  # バトルイベント実行中の場合
  //  if $game_system.battle_interpreter.running?
  //    return
  //  end
  //  # バトルイベントの全ページを検索
  //  for index in 0...$data_troops[@troop_id].pages.size
  //    # イベントページを取得
  //    page = $data_troops[@troop_id].pages[index]
  //    # イベント条件を c で参照可能に
  //    c = page.condition
  //    # 何も条件が指定されていない場合は次のページへ
  //    unless c.turn_valid or c.enemy_valid or
  //           c.actor_valid or c.switch_valid
  //      next
  //    end
  //    # 実行済みの場合は次のページへ
  //    if $game_temp.battle_event_flags[index]
  //      next
  //    end
  //    # ターン 条件確認
  //    if c.turn_valid
  //      n = $game_temp.battle_turn
  //      a = c.turn_a
  //      b = c.turn_b
  //      if (b == 0 and n != a) or
  //         (b > 0 and (n < 1 or n < a or n % b != a % b))
  //        next
  //      end
  //    end
  //    # エネミー 条件確認
  //    if c.enemy_valid
  //      enemy = $game_troop.enemies[c.enemy_index]
  //      if enemy == nil or enemy.hp * 100.0 / enemy.maxhp > c.enemy_hp
  //        next
  //      end
  //    end
  //    # アクター 条件確認
  //    if c.actor_valid
  //      actor = $game_actors[c.actor_id]
  //      if actor == nil or actor.hp * 100.0 / actor.maxhp > c.actor_hp
  //        next
  //      end
  //    end
  //    # スイッチ 条件確認
  //    if c.switch_valid
  //      if $game_switches[c.switch_id] == false
  //        next
  //      end
  //    end
  //    # イベントをセットアップ
  //    $game_system.battle_interpreter.setup(page.list, 0)
  //    # このページのスパンが [バトル] か [ターン] の場合
  //    if page.span <= 1
  //      # 実行済みフラグをセット
  //      $game_temp.battle_event_flags[index] = true
  //    end
  //    return
  //  end
  //end

        public override void Update() 
        { 
    //        # バトルイベント実行中の場合
    //if $game_system.battle_interpreter.running?
    //  # インタプリタを更新
    //  $game_system.battle_interpreter.update
    //  # アクションを強制されているバトラーが存在しない場合
    //  if $game_temp.forcing_battler == nil
    //    # バトルイベントの実行が終わった場合
    //    unless $game_system.battle_interpreter.running?
    //      # 戦闘継続の場合、バトルイベントのセットアップを再実行
    //      unless judge
    //        setup_battle_event
    //      end
    //    end
    //    # アフターバトルフェーズでなければ
    //    if @phase != 5
    //      # ステータスウィンドウをリフレッシュ
    //      @status_window.refresh
    //    end
    //  end
    //end
    //# システム (タイマー)、画面を更新
    //$game_system.update
    //$game_screen.update
    //# タイマーが 0 になった場合
    //if $game_system.timer_working and $game_system.timer == 0
    //  # バトル中断
    //  $game_temp.battle_abort = true
    //end
    //# ウィンドウを更新
    //@help_window.update
    //@party_command_window.update
    //@actor_command_window.update
    //@status_window.update
    //@message_window.update
    //# スプライトセットを更新
    //@spriteset.update
    //# トランジション処理中の場合
    //if $game_temp.transition_processing
    //  # トランジション処理中フラグをクリア
    //  $game_temp.transition_processing = false
    //  # トランジション実行
    //  if $game_temp.transition_name == ""
    //    Graphics.transition(20)
    //  else
    //    Graphics.transition(40, "Graphics/Transitions/" +
    //      $game_temp.transition_name)
    //  end
    //end
    //# メッセージウィンドウ表示中の場合
    //if $game_temp.message_window_showing
    //  return
    //end
    //# エフェクト表示中の場合
    //if @spriteset.effect?
    //  return
    //end
    //# ゲームオーバーの場合
    //if $game_temp.gameover
    //  # ゲームオーバー画面に切り替え
    //  $scene = Scene_Gameover.new
    //  return
    //end
    //# タイトル画面に戻す場合
    //if $game_temp.to_title
    //  # タイトル画面に切り替え
    //  $scene = Scene_Title.new
    //  return
    //end
    //# バトル中断の場合
    //if $game_temp.battle_abort
    //  # バトル開始前の BGM に戻す
    //  $game_system.bgm_play($game_temp.map_bgm)
    //  # バトル終了
    //  battle_end(1)
    //  return
    //end
    //# ウェイト中の場合
    //if @wait_count > 0
    //  # ウェイトカウントを減らす
    //  @wait_count -= 1
    //  return
    //end
    //# アクションを強制されているバトラーが存在せず、
    //# かつバトルイベントが実行中の場合
    //if $game_temp.forcing_battler == nil and
    //   $game_system.battle_interpreter.running?
    //  return
    //end
    //# フェーズによって分岐
    //case @phase
    //when 1  # プレバトルフェーズ
    //  update_phase1
    //when 2  # パーティコマンドフェーズ
    //  update_phase2
    //when 3  # アクターコマンドフェーズ
    //  update_phase3
    //when 4  # メインフェーズ
    //  update_phase4
    //when 5  # アフターバトルフェーズ
    //  update_phase5
    //end
        }

    }
}
