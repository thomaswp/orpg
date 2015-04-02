using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Game
{
    public class Temp
    {
        public string mapBgm;
        public string messageText;
        public delegate void MessageProc();
        public MessageProc messageProc;
        public int choiceStart;
        public int choiceMax;
        public int choiceCancelType;
        public delegate void ChoiceProc(int result);
        public ChoiceProc choiceProc;
        public int numInputStart;
        public int numInputVariableId;
        public int numInputDigitsMax;
        public bool messageWindowShowing;
        public int commonEventId;
        public bool inBattle;
        public bool battleCalling;
        public int battleTroopId;
        public bool battleCanEscape;
        public bool battleCanLose;
        public delegate void BattleProc(int result);
        public BattleProc battleProc;
        public int battleTurn;
        public bool[] battleEventFlags;
        public bool battleAbort;
        public bool battleMainPhase;
        public string battlebackName;
        public Battler forcingBattler;
        public bool shopCalling;
        public int shopId;
        public bool nameCalling;
        public int nameActorId;
        public int nameMaxChar;
        public bool menuCalling;
        public bool menuBeep;
        public bool saveCalling;
        public bool debugCalling;
        public bool playerTransferring;
        public int playerNewMapId;
        public int playerNewX;
        public int playerNewY;
        public int playerNewDirection;
        public bool transitionProcessing;
        public string transitionName;
        public bool gameover;
        public bool toTitle;
        public int lastFileIndex;
        public int debugTopRow;
        public int debugIndex;

        public Temp()
        {
            messageText = "";
            messageProc = null;
            choiceStart = 99;
            numInputStart = 99;
            battleEventFlags = new bool[] {};
            battleAbort = false;
            battleMainPhase = false;
            battlebackName = "";
            forcingBattler = null;
            shopCalling = false;
            shopId = 0;
            nameCalling = false;
            nameActorId = 0;
            nameMaxChar = 0;
            menuCalling = false;
            menuBeep = false;
            saveCalling = false;
            debugCalling = false;
            playerTransferring = false;
            playerNewMapId = 0;
            playerNewX = 0;
            playerNewY = 0;
            playerNewDirection = 0;
            transitionProcessing = false;
            transitionName = "";
            gameover = false;
            toTitle = false;
            lastFileIndex = 0;
            debugTopRow = 0;
            debugIndex = 0;
        }
    }
}
