using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Game
{
    public class Enemy : Battler
    {
        protected int troopId;

        protected int enemyId;
        public int Id
        {
            get { return enemyId; }
        }

        protected int memberIndex;
        public override int Index
        {
            get { return memberIndex; }
        }
    
        public string Name 
        { 
            get { return Data.Enemies[enemyId].name; } 
        }

        public override int BaseAgi
        {
            get { return Data.Enemies[enemyId].agi; }
        }

        public override int BaseAtk
        {
            get { return Data.Enemies[enemyId].atk; }
        }

        public override int BaseDex
        {
            get { return Data.Enemies[enemyId].dex; }
        }

        public override int BaseEva
        {
            get { return Data.Enemies[enemyId].eva; }
        }

        public override int BaseInt
        {
            get { return Data.Enemies[enemyId].intel; }
        }

        public override int BaseMaxHp
        {
            get { return Data.Enemies[enemyId].maxhp; }
        }

        public override int BaseMaxSp
        {
            get { return Data.Enemies[enemyId].maxsp; }
        }

        public override int BaseMDef
        {
            get { return Data.Enemies[enemyId].mdef; }
        }

        public override int BasePDef
        {
            get { return Data.Enemies[enemyId].pdef; }
        }

        public override int BaseStr
        {
            get { return Data.Enemies[enemyId].str; }
        }

        public int Animation1Id
        {
            get { return Data.Enemies[enemyId].animation1Id; }
        }

        public int Animation2Id
        {
            get { return Data.Enemies[enemyId].animation2Id; }
        }

        public override int[] StateRanks
        {
            get { return Data.Enemies[enemyId].stateRanks; }
        }

        public override int[] ElementSet
        {
            get { return new int[] { }; }
        }

        public override int[] PlusStateSet
        {
            get { return new int[] { }; }
        }

        public override int[] MinusStateSet
        {
            get { return new int[] { }; }
        }

        public DataClasses.Enemy.Action[] Actions
        {
            get { return Data.Enemies[enemyId].actions; }
        }

        public int Exp
        {
            get { return Data.Enemies[enemyId].exp; }
        }

        public int Gold
        {
            get { return Data.Enemies[enemyId].gold; }
        }

        public int ItemId
        {
            get { return Data.Enemies[enemyId].itemId; }
        }

        public int WeaponId
        {
            get { return Data.Enemies[enemyId].itemId; }
        }

        public int ArmorId
        {
            get { return Data.Enemies[enemyId].armorId; }
        }

        public int TreasurePro
        {
            get { return Data.Enemies[enemyId].treasureProb; }
        }

        public int ScreenX
        {
            get { return Data.Troops[troopId].members[memberIndex].x; }
        }

        public int ScreenY
        {
            get { return Data.Troops[troopId].members[memberIndex].y; }
        }

        public int ScreenZ
        {
            get { return ScreenY; }
        }

        public Enemy(int troopId, int memberIndex) : base()
        {
            this.troopId = troopId;
            this.memberIndex = memberIndex;
            DataClasses.Troop troop = Data.Troops[troopId];
            enemyId = troop.members[memberIndex].enemyId;
            DataClasses.Enemy enemy = Data.Enemies[enemyId];
            battlerName = enemy.battlerName;
            battlerHue = enemy.battlerHue;
            hp = MaxHp;
            sp = MaxSp;
            hidden = troop.members[memberIndex].hidden;
            immortal = troop.members[memberIndex].immortal;
        }

        public override bool DoesStateGuard(int stateId)
        {
            return false;
        }
        
        public override int ElementRate(int elementId)
        {
            int[] table = new int[] { 0, 200, 150, 100, 50, 0, -100 };
            int result = table[Data.Enemies[enemyId].elementRanks[enemyId]];

            foreach (int i in states)
                if (Data.States[i].guardElementSet.Includes(elementId))
                    result /= 2;

            return result;
        }

        public void Escape()
        {
            hidden = true;
            this.CurrentAction.Clear();
        }

        public void Transform(int enemyId)
        {
            this.enemyId = enemyId;
            this.battlerName = Data.Enemies[enemyId].battlerName;
            this.battlerHue = Data.Enemies[enemyId].battlerHue;

            MakeAction();
        }

        public void MakeAction()
        {
            this.CurrentAction.Clear();

            if (!this.IsMovable)
                return;

            List<DataClasses.Enemy.Action> availableActions = new List<DataClasses.Enemy.Action>();
            int ratingMax = 0;

            foreach (DataClasses.Enemy.Action action in this.Actions)
            {
                int n = Globals.GameTemp.battleTurn;
                int a = action.conditionTurn_a;
                int b = action.conditionTurn_b;

                if ((b == 0 && n != a) || (b > 0 && (n < 1 || n < a || n % b != a % b)))
                    continue;

                if (this.hp * 100.0 / this.MaxHp > action.conditionHp)
                    continue;

                if (Globals.GameParty.MaxLevel < action.conditionLevel)
                    continue;

                int switchId = action.conditionSwitch_id;
                if (switchId > 0 && Globals.GameSwitches[switchId] == false)
                    continue;

                availableActions.Add(action);
                if (action.rating > ratingMax)
                    ratingMax = action.rating;
            }

            int ratingsTotal = 0;
            foreach (DataClasses.Enemy.Action action in availableActions)
                if (action.rating > ratingMax - 3)
                    ratingsTotal += action.rating - (ratingMax - 3);

            if (ratingsTotal > 0)
            {
                int value = Rand.Next(ratingsTotal);

                foreach (DataClasses.Enemy.Action action in availableActions)
                {
                    if (action.rating > ratingMax - 3)
                    {
                        if (value < action.rating - (ratingMax - 3))
                        {
                            this.CurrentAction.Kind = action.kind;
                            this.CurrentAction.Basic = action.basic;
                            this.CurrentAction.SkillId = action.skillId;
                            this.CurrentAction.DecideRandomTargetForEnemy();
                            return;
                        }
                        else
                        {
                            value -= action.rating - (ratingMax - 3);
                        }
                    }
                }
            }
        }
    }
}
