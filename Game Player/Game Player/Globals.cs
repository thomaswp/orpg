using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    /// <summary>
    /// A class of static properties that can be accessed from anywhere.
    /// </summary>
    public partial class Globals
    {
        private static bool _btest = false;
        /// <summary>
        /// Indicates whether or not the current game is a Battle Test.
        /// </summary>
        public static bool BTEST
        {
            get { return _btest; }
            set { _btest = value; }
        }

        private static bool _debug;
        public static bool DEBUG
        {
            get { return _debug; }
            set { _debug = value; }
        }

        private static Scene _scene;// = new Scene();
        /// <summary>
        /// A class for identifying and implementing the current scene.
        /// </summary>
        public static Scene Scene
        {
            get { return _scene; }
            set
            {
                //End the current scene if ther is one.
                Scene oldScene = _scene;
                _scene = value;
                if (oldScene != null)
                    oldScene.End();
            }
        }

        private static Game.Switches _gameSwitches = new Game.Switches();
        /// <summary>
        /// A class containing a an array of boolean switches.
        /// </summary>
        public static Game.Switches GameSwitches
        {
            get { return _gameSwitches; }
            set { _gameSwitches = value; }
        }

        private static Game.Variables _gameVariables = new Game.Variables();
        /// <summary>
        /// A class containing a an array of integer variables.
        /// </summary>
        public static Game.Variables GameVariables
        {
            get { return _gameVariables; }
            set { _gameVariables = value; }
        }

        private static Game.SelfSwitches _gameSelfSwitches = new Game.SelfSwitches();
        /// <summary>
        /// A class containing a an array of hash Switches.
        /// </summary>
        public static Game.SelfSwitches GameSelfSwitches
        {
            get { return _gameSelfSwitches; }
            set { _gameSelfSwitches = value; }
        }

        private static Game.Actors _gameActors = new Game.Actors();
        /// <summary>
        /// A class for identifying and implementing game actors.
        /// </summary>
        public static Game.Actors GameActors
        {
            get { return _gameActors; }
            set { _gameActors = value; }
        }

        private static Game.Temp _gameTemp = new Game.Temp();
        /// <summary>
        /// A class holding temporary variable information.
        /// </summary>
        public static Game.Temp GameTemp
        {
            get { return _gameTemp; }
            set { _gameTemp = value; }
        }

        private static Game.System _gameSystem = new Game.System();
        /// <summary>
        /// A class for performing system activities.
        /// </summary>
        public static Game.System GameSystem
        {
            get { return _gameSystem; }
            set { _gameSystem = value; }
        }




        private static Game.Screen _gameScreen = new Game.Screen();
        /// <summary>
        /// A class containing the game screen.
        /// </summary>
        public static Game.Screen GameScreen
        {
            get { return _gameScreen; }
            set { _gameScreen = value; }
        }



        private static Game.Party _gameParty = new Game.Party();
        /// <summary>
        /// A class containing the player's current party.
        /// </summary>
        public static Game.Party GameParty
        {
            get { return _gameParty; }
            set { _gameParty = value; }
        }

        private static Game.Troop _gameTroop = new Game.Troop();
        /// <summary>
        /// A class containing teh current enemy party.
        /// </summary>
        public static Game.Troop GameTroop
        {
            get { return _gameTroop; }
            set { _gameTroop = value; }
        }

        private static Game.Map _gameMap = new Game.Map();
        /// <summary>
        /// A class containing the current map.
        /// </summary>
        public static Game.Map GameMap
        {
            get { return _gameMap; }
            set { _gameMap = value; }
        }

        private static Game.Player _gamePlayer = new Game.Player();
        /// <summary>
        /// A class containing the players map sprite.
        /// </summary>
        public static Game.Player GamePlayer
        {
            get { return _gamePlayer; }
            set { _gamePlayer = value; }
        }
    }
}
