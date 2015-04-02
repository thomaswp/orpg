using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataClasses;

namespace Game_Player.Game
{
    public abstract partial class Battler
    {
#region properties
        protected string battlerName;
        public string BattlerName { get { return battlerName; } }

        protected int battlerHue;
        public int BattlerHue { get { return battlerHue; } }

        protected int hp;
        public int Hp
        {
	        get { return hp; }
            set
            {
                hp = value.MinMax(0, MaxHp);

                for (int i = 1; i < Data.States.Length; i++)
                    if (Data.States[i].zeroHp)
                    {
                        if (IsDead)
                            AddState(i);
                        else
                            RemoveState(i);
                    }
            }
        }

        protected int sp;
        public int Sp
        {
	        get { return sp; }
            set { sp = value.MinMax(0, MaxSp); }
        }

        protected int[] states;
        public int[] States
        {
	        get { return states; }
	        set { states = value; }
        }

        protected bool hidden;
        public bool Hidden
        {
            get { return hidden; }
            set { hidden = value; }
        }

        protected bool immortal;
        public bool Immortal
        {
            get { return immortal; }
            set { immortal = value; }
        }

        protected bool damagePop;
        public bool DamagePop
        {
            get { return damagePop; }
            set { damagePop = value; }
        }

        protected string damage;
        public string Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        protected bool critical;
        public bool Critical
        {
            get { return critical; }
            set { critical = value; }
        }

        protected int animationID;
        public int AnimationID
        {
            get { return animationID; }
            set { animationID = value; }
        }

        protected bool animationHit;
        public bool AnimationHit
        {
            get { return animationHit; }
            set { animationHit = value; }
        }

        protected bool whiteFlash;
        public bool WhiteFlash
        {
            get { return whiteFlash; }
            set { whiteFlash = value; }
        }

        protected bool blink;
        public bool Blink
        {
            get { return blink; }
            set { blink = value; }
        }

        public virtual int MaxHp
        {
            get
            {
                double n = (BaseMaxHp + maxHpPlus).MinMax(1, 999999);
                foreach (int i in states)
                    n = Data.States[i].maxhpRate / 100.0;
                n = n.MinMax(1, 999999);
                return (int)n;
            }
            set
            {
                maxHpPlus += value - MaxHp;
                maxHpPlus = maxHpPlus.MinMax(-9999, 9999);
                hp = Math.Min(hp, MaxHp);
            }
        }

        public int MaxSp
        {
            get
            {
                double n = (BaseMaxSp + maxSpPlus).MinMax(1, 9999);
                foreach (int i in states)
                    n = Data.States[i].maxspRate / 100.0;
                n = n.MinMax(1, 9999);
                return (int)n;
            }
            set
            {
                maxSpPlus += value - MaxSp;
                maxSpPlus = maxSpPlus.MinMax(-9999, 9999);
                hp = Math.Min(sp, MaxSp);
            }
        }

        public int Str
        {
            get
            {
                double n = (BaseStr + strPlus).MinMax(1, 999);
                foreach (int i in states)
                    n = Data.States[i].strRate / 100.0;
                n = n.MinMax(1, 999);
                return (int)n;
            }
            set
            {
                strPlus += value - Str;
                strPlus = strPlus.MinMax(-999, 999);
            }
        }

        public int Dex
        {
            get
            {
                double n = (BaseDex + dexPlus).MinMax(1, 999);
                foreach (int i in states)
                    n = Data.States[i].dexRate / 100.0;
                n = n.MinMax(1, 999);
                return (int)n;
            }
            set
            {
                dexPlus += value - Dex;
                dexPlus = dexPlus.MinMax(-999, 999);
            }
        }

        public int Agi
        {
            get
            {
                double n = (BaseAgi + agiPlus).MinMax(1, 999);
                foreach (int i in states)
                    n = Data.States[i].agiRate / 100.0;
                n = n.MinMax(1, 999);
                return (int)n;
            }
            set
            {
                agiPlus += value - Agi;
                agiPlus = agiPlus.MinMax(-999, 999);
            }
        }

        public int Int
        {
            get
            {
                double n = (BaseInt + intPlus).MinMax(1, 999);
                foreach (int i in states)
                    n = Data.States[i].intRate / 100.0;
                n = n.MinMax(1, 999);
                return (int)n;
            }
            set
            {
                intPlus += value - Int;
                intPlus = intPlus.MinMax(-999, 999);
            }
        }

        public int Atk
        {
            get
            {
                double n = BaseAtk;
                foreach (int i in states)
                    n = Data.States[i].atkRate / 100.0;
                return (int)n;
            }
        }

        public int MDef
        {
            get
            {
                double n = BaseMDef;
                foreach (int i in states)
                    n = Data.States[i].mdefRate / 100.0;
                return (int)n;
            }
        }

        public int PDef
        {
            get
            {
                double n = BasePDef;
                foreach (int i in states)
                    n = Data.States[i].pdefRate / 100.0;
                return (int)n;
            }
        }

        public int Eva
        {
            get
            {
                double n = BaseEva;
                foreach (int i in states)
                    n += Data.States[i].eva;
                return (int)n;
            }
        }

        public int Hit
        {
            get
            {
                double n = 100;
                foreach (int i in states)
                    n *= Data.States[i].hitRate / 100.0;
                return (int)n;
            }
        }

        BattleAction currentAction;
        public BattleAction CurrentAction
        {
            get { return currentAction; }
        }

        public bool IsDead { get { return hp == 0 && !immortal; } }

        public bool Exists { get { return !hidden && (hp > 0 || immortal); } }

        public bool IsHp0 { get { return !hidden && hp == 0; } }

        public bool IsImputable { get { return !hidden && Restriction <= 1; } }

        public bool IsMovable { get { return !hidden && Restriction < 4; } }

        public bool IsGuarding { get { return currentAction.Kind == 0 && currentAction.Basic == 1; } }

        public bool IsResting { get { return currentAction.Kind == 0 && currentAction.Basic == 3; } }

        public abstract int BaseMaxHp { get; }
        public abstract int BaseMaxSp { get; }
        public abstract int BaseStr { get; }
        public abstract int BaseDex { get; }
        public abstract int BaseAgi { get; }
        public abstract int BaseInt { get; }
        public abstract int BaseAtk { get; }
        public abstract int BaseMDef { get; }
        public abstract int BasePDef { get; }
        public abstract int BaseEva { get; }
        public abstract int[] StateRanks { get; }
        public abstract int[] ElementSet { get; }
        public abstract int[] PlusStateSet { get; }
        public abstract int[] MinusStateSet { get; }

        public abstract int Index { get; }
#endregion

        protected int maxHpPlus;
        protected int maxSpPlus;
        protected int strPlus;
        protected int dexPlus;
        protected int agiPlus;
        protected int intPlus;
        protected Dictionary<int, int> statesTurn;
        protected bool stateChanged = false;

        public Battler()
        {
            states = new int[] { };
            currentAction = new BattleAction();
            statesTurn = new Dictionary<int, int>();
        }
        
        public abstract bool DoesStateGuard(int arg);
        public abstract int ElementRate(int arg);

        public void RecoverAll()
        {
            hp = MaxHp;
            sp = MaxSp;
            foreach (int i in states)
                RemoveState(i);
        }

        public void MakeActionSpeed()
        {
            currentAction.Speed = Agi + Rand.Next(10 + Agi / 4);
        }
    }
}
