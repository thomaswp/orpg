using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataClasses;

namespace Game_Player.Game
{
    public partial class Battler
    {

#region Properties
        public int StateAnimationId
        {
            get
            {
                if (states.Length == 0)
                    return 0;

                return Data.States[states[0]].animationId;
            }
        }

        public int Restriction
        {
            get
            {
                int restrictionMax = 0;

                foreach (int i in states)
                    if (Data.States[i].restriction >= restrictionMax)
                        restrictionMax = Data.States[i].restriction;

                return restrictionMax;
            }
        }

        public bool CantGetExp
        {
            get
            {
                foreach (int i in states)
                    if (Data.States[i].cantGetExp)
                        return true;

                return false;
            }
        }

        public bool CantEvade
        {
            get
            {
                foreach (int i in states)
                    if (Data.States[i].cantEvade)
                        return true;

                return false;
            }
        }

        public bool SlipDamage
        {
            get
            {
                foreach (int i in states)
                    if (Data.States[i].slipDamage)
                        return true;

                return false;
            }
        }
#endregion

        public bool HasState(int stateId)
        {
            return Array.IndexOf<int>(States, stateId) != -1;
        }

        public bool IsStateFull(int stateId)
        {
            if (!HasState(stateId))
                return false;

            if (statesTurn[stateId] == -1)
                return true;

            return statesTurn[stateId] == Data.States[stateId].holdTurn;
        }

        public void AddState(int stateId)
        {
            AddState(stateId, false);
        }

        public void AddState(int stateId, bool force)
        {
            if (Data.States[stateId] == null)
                return;

            if (!force)
            {
                foreach (int i in states)
                {
                    if (Data.States[i].minusStateSet.Includes(stateId) &&
                        !Data.States[stateId].minusStateSet.Includes(i))
                        return;
                }
            }

            if (!HasState(stateId))
            {
                Array.Resize<int>(ref states, states.Length + 1);
                states[states.Length - 1] = stateId;

                if (Data.States[stateId].zeroHp)
                    hp = 0;

                for (int i = 1; i < Data.States.Length; i++ )
                {
                    if (Data.States[stateId].plusStateSet.Includes(i))
                        AddState(i);

                    if (Data.States[stateId].minusStateSet.Includes(i))
                        RemoveState(i);
                }

                //substitution for custom sort in ruby... see the
                //CompareTo() method in DataClasses.State for more
                State[] sortStates = new State[states.Length];
                for (int i = 0; i < states.Length; i++)
                    sortStates[i] = Data.States[i];

                Array.Sort(sortStates, states);
            }

            if (force)
                statesTurn[stateId] = -1;

            if (statesTurn[stateId] != -1)
                statesTurn[stateId] = Data.States[stateId].holdTurn;

            if (!IsMovable)
                currentAction.Clear();

            hp = Math.Min(hp, MaxHp);
            sp = Math.Min(sp, MaxSp);
        }

        public void RemoveState(int stateId)
        {
            RemoveState(stateId, false);
        }

        public void RemoveState(int stateId, bool force)
        {
            if (HasState(stateId))
            {
                if (statesTurn[stateId] == -1 && !force)
                    return;

                if (hp == 0 && Data.States[stateId].zeroHp)
                {
                    bool zeroHp = false;
                    foreach (int i in states)
                        if (i != stateId && Data.States[i].zeroHp)
                            zeroHp = true;

                    if (!zeroHp)
                        hp = 1;
                }

                //substitution for @states.delete(stateId)
                int delIndex = Array.IndexOf<int>(states, stateId);
                for (int i = delIndex; i < states.Length - 1; i++)
                    states[i] = states[i + 1];
                Array.Resize<int>(ref states, States.Length - 1);

                statesTurn.Remove(stateId);
            }

            hp = Math.Min(hp, MaxHp);
            sp = Math.Min(sp, MaxSp);
        }

        public void RemoveStatesBattle()
        {
            foreach (int i in (int[])states.Clone())
                if (Data.States[i].battleOnly)
                    RemoveState(i);
        }

        public void RemoveStatesAuto()
        {
            foreach (int i in statesTurn.Keys)
                if (statesTurn[i] > 0)
                    statesTurn[i]--;
                else if (Rand.Next(100) < Data.States[i].autoReleaseProb)
                    RemoveState(i);
        }

        public void RemoveStatesShock()
        {
            foreach (int i in (int[])states.Clone())
                if (Rand.Next(100) < Data.States[i].shockReleaseProb)
                    RemoveState(i);
        }

        public bool StatesPlus(int[] plusStateSet)
        {
            bool effective = false;

            foreach (int i in plusStateSet)
            {
                if (!DoesStateGuard(i))
                {
                    effective |= IsStateFull(i) == false;

                    if (Data.States[i].nonresistance)
                    {
                        stateChanged = true;
                        AddState(i);
                    }
                    else if (!IsStateFull(i))
                    {
                        if (Rand.Next(100) < (new int[] { 0, 100, 80, 60, 40, 20, 0 })[StateRanks[i]])
                        {
                            stateChanged = true;
                            AddState(i);
                        }
                    }
                }
            }

            return effective;
        }

        public bool StatesMinus(int[] minusStateSet)
        {
            bool effective = false;

            foreach (int i in minusStateSet)
            {
                effective |= HasState(i);
                stateChanged = true;
                RemoveState(i);
            }

            return effective;
        }
    }
}
