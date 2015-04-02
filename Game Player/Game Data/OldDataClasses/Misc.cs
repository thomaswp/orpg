using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.DataClasses
{
    /// <summary>
    /// A data class containing miscellaneous user-defined information.
    /// </summary>
    public class Misc
    {
        string _windowSkin = "";
        /// <summary>
        /// The file path containing the window skin.
        /// </summary>
        public string WindowSkin
        {
            get { return _windowSkin; }
        }

        string _titleScreen = "";
        /// <summary>
        /// The file path containing the title screen image.
        /// </summary>
        public string TitleScreen
        {
            get { return _titleScreen; }
        }

        string _gameOverScreen = "";
        /// <summary>
        /// The file path containing the game over image.
        /// </summary>
        public string GameOverScreen
        {
            get { return _gameOverScreen; }
        }

        string _battleTransition = "";
        /// <summary>
        /// The file path containing the battle transition.
        /// </summary>
        public string BattleTransition
        {
            get { return _battleTransition; }
        }

        string _titleScreenBGM = "";
        /// <summary>
        /// The file path containing the title screen music.
        /// </summary>
        public string TitleScreenBGM
        {
            get { return _titleScreenBGM; }
        }

        string _battleBGM = "";
        /// <summary>
        /// The file path containing the battle music.
        /// </summary>
        public string BattleBGM
        {
            get { return _battleBGM; }
        }

        string _victoryME = "";
        /// <summary>
        /// The file path containing the victory music.
        /// </summary>
        public string VictoryME
        {
            get { return _victoryME; }
        }

        string _gameOverME = "";
        /// <summary>
        /// The file path containing the game over music.
        /// </summary>
        public string GameOverME
        {
            get { return _gameOverME; }
        }

        string _cursorSE = "";
        /// <summary>
        /// The file path containing the sound played on cursor movement.
        /// </summary>
        public string CursorSE
        {
            get { return _cursorSE; }
        }

        string _decisionSE = "";
        /// <summary>
        /// The file path containing the "ok" sound.
        /// </summary>
        public string DecisionSE
        {
            get { return _decisionSE; }
        }

        string _cancelSE = "";
        /// <summary>
        /// The file path containing the cancel sound.
        /// </summary>
        public string CancelSE
        {
            get { return _cancelSE; }
        }

        string _buzzerSE = "";
        /// <summary>
        /// The file path containing played to show an incorrect selection.
        /// </summary>
        public string BuzzerSE
        {
            get { return _buzzerSE; }
        }

        string _equipSE = "";
        /// <summary>
        /// The file path containing the sound effect played on equiping an item.
        /// </summary>
        public string EquipSE
        {
            get { return _equipSE; }
        }

        string _shopSE = "";
        /// <summary>
        /// The file path containing the sound played when a successful shopping transaction occurs.
        /// </summary>
        public string ShopSE
        {
            get { return _shopSE; }
        }

        string _saveSE = "";
        /// <summary>
        /// The file path containing the sound played when the game is saved.
        /// </summary>
        public string SaveSE
        {
            get { return _saveSE; }
        }

        string _loadSE = "";
        /// <summary>
        /// The file path containing the sound played when the game loads.
        /// </summary>
        public string LoadSE
        {
            get { return _loadSE; }
        }

        string _battleStartSE = "";
        /// <summary>
        /// The file path containing the sound played at the start of a battle.
        /// </summary>
        public string BattleStartSE
        {
            get { return _battleStartSE; }
        }

        string _fleeSE = "";
        /// <summary>
        /// The file path containing the sound played when the player flees a battle.
        /// </summary>
        public string FleeSE
        {
            get { return _fleeSE; }
        }

        string _heroDeadSE = "";
        /// <summary>
        /// The file path containing the sound played when a hero dies.
        /// </summary>
        public string HeroDeadSE
        {
            get { return _heroDeadSE; }
        }

        string _monsterDeadSE = "";
        /// <summary>
        /// The file path containing the sound played when a monster dies.
        /// </summary>
        public string MonsterDeadSE
        {
            get { return _monsterDeadSE; }
        }

        string _title;
        /// <summary>
        /// The name of the game.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// A temporary method for initializing defaults until a class to load from is initialized.
        /// </summary>
        public void Load()
        {
            _titleScreen = Data.RTP + "Graphics\\Titles\\001-Title01.jpg";
            _windowSkin = Data.RTP + "Graphics\\Windowskins\\001-Blue01.png";
            //_windowSkin = "C:\\Users\\Thomas\\Desktop\\rmxp_windowskins\\vpl_rmxpWindowskins\\vpl_checkard.blue.png";
            _title = "Game Player";
            _titleScreenBGM = "064-Slow07";
            _cursorSE = "001-System01";
            _decisionSE = "002-System02";
        }
    }
}
