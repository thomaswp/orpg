using System;
using System.Collections.Generic;
using System.Text;
using DataClasses;

namespace Game_Player.Game
{
    public partial class Battler
    {
        public virtual bool CanUseSkill(int skillId)
        {
            if (Data.Skills[skillId].spCost > this.sp)
                return false;

            if (IsDead)
                return false;

            if (Data.Skills[skillId].atkF == 0 && this.Restriction == 1)
                return false;

            int occasion = Data.Skills[skillId].occasion;

            if (Globals.GameTemp.inBattle)
                return (occasion == 0 || occasion == 1);
            else
                return (occasion == 0 || occasion == 2);
        }

        public bool AttackEffect(Battler attacker)
        {
            this.critical = false;
            bool hitResult = Rand.Next(100) < this.Hit;
            int dmgCalc = 0;

            if (hitResult)
            {
                int atk = Math.Max(attacker.Atk - this.PDef / 2, 0);
                //substitute for direct assignemtn to damage
                //so it can be a string
                dmgCalc = atk * (20 + attacker.Str) / 20;
                dmgCalc *= ElementsCorrect(attacker.ElementSet);
                dmgCalc /= 100;

                if (dmgCalc > 0)
                {
                    if (Rand.Next(100) < 4 * attacker.Dex / this.Agi)
                    {
                        dmgCalc *= 2;
                        this.critical = true;
                    }

                    if (IsGuarding)
                        dmgCalc /= 2;
                }

                if (Math.Abs(dmgCalc) > 0)
                {
                    int amp = Math.Max(Math.Abs(dmgCalc) * 15, 1);
                    dmgCalc += 2 * Rand.Next(amp + 1) - amp;
                }

                int eva = 8 * this.Agi / attacker.Dex + this.Eva;
                int hit = dmgCalc < 0 ? 100 : 100 - eva;
                hit = this.CantEvade ? 100 : hit;
                hitResult = (Rand.Next(100) < hit);
            }

            if (hitResult)
            {
                this.damage = dmgCalc.ToString();
                RemoveStatesShock();
                this.hp -= dmgCalc;
                stateChanged = false;
                StatesPlus(attacker.PlusStateSet);
                StatesMinus(attacker.MinusStateSet);
            }
            else
            {
                this.damage = "Miss";
                this.critical = false;
            }

            return true;
        }

        public bool SkillEffect(Battler user, Skill skill)
        {
            int dmgCalc = 0;

            this.critical = false;

            if (((skill.scope == 3 || skill.scope == 4) && this.hp == 0) ||
                ((skill.scope == 5 || skill.scope == 6) && this.hp >= 1))
                return false;

            bool effective = false;
            effective |= skill.commonEventId > 0;

            int hit = skill.hit;
            bool hitResult = (Rand.Next(100) < hit);
            if (skill.atkF > 0)
                hit *= user.Hit / 100;

            effective |= hit < 100;

            if (hitResult)
            {
                int power = skill.power + user.Atk * skill.atkF / 100;
                if (power > 0)
                {
                    power -= this.PDef * skill.pdefF / 200;
                    power -= this.MDef * skill.mdefF / 200;
                    power = Math.Max(power, 0);
                }

                int rate = 20;
                rate += user.Str * skill.strF / 100;
                rate += user.Dex * skill.dexF / 100;
                rate += user.Agi * skill.agiF / 100;
                rate += user.Int * skill.intF / 100;

                dmgCalc = power * rate / 20;
                dmgCalc *= ElementsCorrect(skill.elementSet);
                dmgCalc /= 100;

                if (dmgCalc > 0)
                    if (this.IsGuarding)
                        dmgCalc /= 2;

                if (skill.variance > 0 && Math.Abs(dmgCalc) > 0)
                {
                    int amp = Math.Max(Math.Abs(dmgCalc) * skill.variance / 100, 1);
                    dmgCalc += 2 * Rand.Next(amp + 1) - amp;
                }

                int eva = 8 * this.Agi / user.Dex + this.Eva;
                hit = dmgCalc < 0 ? 100 : 100 - eva * skill.evaF / 100;
                hit = this.CantEvade ? 100 : hit;
                hitResult = (Rand.Next(100) < hit);

                effective |= hit < 100;

                //SUB
                this.damage = dmgCalc.ToString();
            }

            if (hitResult)
            {
                if (skill.power != 0 && skill.atkF > 0)
                {
                    RemoveStatesShock();
                    effective = true;
                }

                int lastHp = this.hp;
                this.hp -= dmgCalc;

                effective |= this.hp != lastHp;
                stateChanged = false;
                effective |= StatesPlus(skill.plusStateSet);
                effective |= StatesMinus(skill.minusStateSet);

                if (skill.power == 0)
                {
                    this.damage = "";

                    if (!stateChanged)
                        this.damage = "Miss";
                }
            }
            else
            {
                this.damage = "Miss";
            }

            if (!Globals.GameTemp.inBattle)
                this.damage = null;

            return effective;
        }

        public bool ItemEffect(Item item)
        {
            int dmgCalc = 0;

            this.critical = false;

            if (((item.scope == 3 || item.scope == 4) && this.hp == 0) ||
                ((item.scope == 5 || item.scope == 6) && this.hp >= 1))
                return false;

            bool effective = false;
            effective |= item.commonEventId > 0;

            bool hitResult = (Rand.Next(100) < item.hit);
            effective |= item.hit < 100;

            if (hitResult)
            {
                int recoverHp = MaxHp * item.recoverHpRate / 100 + item.recoverHp;
                int recoverSp = MaxSp * item.recoverSpRate / 100 + item.recoverSp;

                if (recoverHp < 0)
                {
                    recoverHp += this.PDef * item.pdefF / 20;
                    recoverHp += this.MDef * item.mdefF / 20;
                    recoverHp = Math.Min(recoverHp, 0);
                }

                recoverHp *= ElementsCorrect(item.elementSet);
                recoverHp /= 100;
                recoverSp *= ElementsCorrect(item.elementSet);
                recoverSp /= 100;

                if (item.variance > 0 && Math.Abs(recoverHp) > 0)
                {
                    int amp = Math.Max(Math.Abs(recoverHp * item.variance) / 100, 1);
                    recoverHp += 2 * Rand.Next(amp + 1) - amp;
                }
                if (item.variance > 0 && Math.Abs(recoverSp) > 0)
                {
                    int amp = Math.Max(Math.Abs(recoverSp * item.variance) / 100, 1);
                    recoverSp += 2 * Rand.Next(amp + 1) - amp;
                }

                if (recoverHp < 0)
                    if (this.IsGuarding)
                        recoverHp /= 2;

                dmgCalc = -recoverHp;

                int lastHp = this.hp;
                int lastSp = this.sp;
                Hp += recoverHp;
                Sp += recoverSp;

                effective |= this.hp != lastHp;
                effective |= this.sp != lastSp;

                stateChanged = false;

                effective |= StatesPlus(item.plusStateSet);
                effective |= StatesMinus(item.minusStateSet);

                if (item.parameterType > 0 && item.parameterPoints != 0)
                {
                    switch (item.parameterType)
                    {
                        case 1: maxHpPlus += item.parameterPoints; break;
                        case 2: maxSpPlus += item.parameterPoints; break;
                        case 3: strPlus += item.parameterPoints; break;
                        case 4: dexPlus += item.parameterPoints; break;
                        case 5: agiPlus += item.parameterPoints; break;
                        case 6: intPlus += item.parameterPoints; break;
                    }

                    effective = true;
                }
            }

            //SUB
            this.damage = dmgCalc.ToString();

            if (item.recoverHpRate == 0 && item.recoverHp == 0)
            {
                this.damage = "";

                if (item.recoverSpRate == 0 && item.recoverSp == 0 &&
                    (item.parameterType == 0 || item.parameterPoints == 0))
                    if (!stateChanged)
                        this.damage = "Miss";
            }
            else
            {
                this.damage = "Miss";
            }

            if (!Globals.GameTemp.inBattle)
                this.damage = null;

            return effective;
        }

        public bool SlipDamageEffect()
        {
            int dmgCalc = this.MaxHp / 10;

            if (Math.Abs(dmgCalc) > 0)
            {
                int amp = Math.Max(Math.Abs(dmgCalc * 15 / 100), 1);
                dmgCalc += 2 * Rand.Next(amp + 1) - amp;
            }

            this.hp -= dmgCalc;
            this.damage = dmgCalc.ToString();

            return true;
        }

        public int ElementsCorrect(int[] elementsSet)
        {
            if (elementsSet.Length == 0)
                return 100;

            int weakest = -100;
            foreach (int i in elementsSet)
                weakest = Math.Max(weakest, this.ElementRate(i));

            return weakest;
        }
    }
}
