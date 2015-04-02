using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.DataClasses
{
    public class Misc
    {
        string _windowSkin = "";
        public string WindowSkin
        {
            get { return _windowSkin; }
        }

        string _titleScreen = "";
        public string TitleScreen
        {
            get { return _titleScreen; }
        }

        string _gameOverScreen = "";
        public string GameOverScreen
        {
            get { return _gameOverScreen; }
        }

        string _battleTransition = "";
        public string BattleTransition
        {
            get { return _battleTransition; }
        }

        string _titleScreenBGM = "";
        public string TitleScreenBGM
        {
            get { return _titleScreenBGM; }
        }

        string _battleBGM = "";
        public string BattleBGM
        {
            get { return _battleBGM; }
        }

        string _victoryME = "";
        public string VictoryME
        {
            get { return _victoryME; }
        }

        string _gameOverME = "";
        public string GameOverME
        {
            get { return _gameOverME; }
        }

        string _cursorSE = "";
        public string CursorSE
        {
            get { return _cursorSE; }
        }

        string _decisionSE = "";
        public string DecisionSE
        {
            get { return _decisionSE; }
        }

        string _cancelSE = "";
        public string CancelSE
        {
            get { return _cancelSE; }
        }

        string _buzzerSE = "";
        public string BuzzerSE
        {
            get { return _buzzerSE; }
        }

        string _equipSE = "";
        public string EquipSE
        {
            get { return _equipSE; }
        }

        string _shopSE = "";
        public string ShopSE
        {
            get { return _shopSE; }
        }

        string _saveSE = "";
        public string SaveSE
        {
            get { return _saveSE; }
        }

        string _loadSE = "";
        public string LoadSE
        {
            get { return _loadSE; }
        }

        string _battleStartSE = "";
        public string BattleStartSE
        {
            get { return _battleStartSE; }
        }

        string _fleeSE = "";
        public string FleeSE
        {
            get { return _fleeSE; }
        }

        string _heroDeadSE = "";
        public string HeroDeadSE
        {
            get { return _heroDeadSE; }
        }

        string _monsterDeadSE = "";
        public string MonsterDeadSE
        {
            get { return _monsterDeadSE; }
        }

        string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public void Load()
        {
            _titleScreen = Data.RTP + "Graphics\\Titles\\001-Title01.jpg";
            _windowSkin = Data.RTP + "Graphics\\Windowskins\\001-Blue01.png";
            //_windowSkin = "C:\\Users\\Thomas\\Desktop\\rmxp_windowskins\\vpl_rmxpWindowskins\\vpl_checkard.blue.png";
            _title = "Game Player";
            _cursorSE = "001-System01.ogg";
        }
    }
}
