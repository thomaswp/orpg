using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Game
{
    public class BattleAction
    {
        #region Properties
        private int speed;
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private int kind;
        public int Kind
        {
            get { return kind; }
            set { kind = value; }
        }

        private int basic;
        public int Basic
        {
            get { return basic; }
            set { basic = value; }
        }

        private int skillId;
        public int SkillId
        {
            get { return skillId; }
            set { skillId = value; }
        }

        private int itemId;
        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        private int targetIndex;
        public int TargetIndex
        {
            get { return targetIndex; }
            set { targetIndex = value; }
        }

        private bool forcing;
        public bool Forcing
        {
            get { return forcing; }
            set { forcing = value; }
        }

        public bool IsValid { get { return !(kind == 0 && basic == 3); } }

        public bool IsForOneFriend
        {
            get
            {
                if (kind == 1 && (new int[] { 3, 5 }).Includes(Data.Skills[skillId].scope))
                    return true;

                if (kind == 2 && (new int[] { 3, 5 }).Includes(Data.Items[itemId].scope))
                    return true;

                return false;
            }
        }

        public bool IsForOneFriendHp0
        {
            get
            {
                if (kind == 1 && 5 == Data.Skills[skillId].scope)
                    return true;

                if (kind == 2 && 5 == Data.Items[skillId].scope)
                    return true;

                return false;
            }
        }
        #endregion

        public BattleAction()
        {
            Clear();
        }

        public void Clear()
        {
            speed = 0;
            kind = 0;
            basic = 3;
            skillId = 3;
            itemId = 0;
            targetIndex = -1;
            forcing = false;
        }

        public void DecideRandomTargetForActor()
        {
            Game.Battler battler;

            if (IsForOneFriendHp0)
                battler = Globals.GameParty.RandomTargetActorHp0();
            else if (IsForOneFriend)
                battler = Globals.GameParty.RandomTargetActor();
            else
                battler = Globals.GameTroop.RandomTargetEnemy();

            if (battler != null)
                targetIndex = battler.Index;
            else
                Clear();
        }

        public void DecideRandomTargetForEnemy()
        {
            Battler battler;
            if (IsForOneFriendHp0)
                battler = Globals.GameTroop.RandomTargetEnemyHp0();
            else if (IsForOneFriend)
                battler = Globals.GameTroop.RandomTargetEnemy();
            else
                battler = Globals.GameParty.RandomTargetActor();

            if (battler != null)
                targetIndex = battler.Index;
            else
                Clear();
        }

        public void DecideLastTargetForActor()
        {
            Battler battler;
            if (targetIndex == -1)
                battler = null;
            else if (IsForOneFriend)
                battler = Globals.GameParty.Actors[targetIndex];
            else
                battler = Globals.GameTroop.Enemies[targetIndex];

            if (battler == null || !battler.Exists)
                Clear();
        }

        public void DecideLastTargetForEnemy()
        {
            Battler battler;
            if (targetIndex == -1)
                battler = null;
            else if (IsForOneFriend)
                battler = Globals.GameTroop.Enemies[targetIndex];
            else
                battler = Globals.GameParty.Actors[targetIndex];

            if (battler == null || !battler.Exists)
                Clear();
        }
    }
}
