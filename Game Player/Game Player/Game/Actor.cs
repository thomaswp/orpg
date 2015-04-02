using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataClasses;

namespace Game_Player.Game
{
    public class Actor : Battler
    {
        #region Properties
        private string name;
        public string Name 
        { 
            get { return name; }
            set { name = value; }
        }

        private string characterName;
        public string CharacterName { get { return characterName; } }

        protected int characterHue;
        public int CharacterHue { get { return characterHue; } }

        private int classId;
        public int ClassId 
        {
            get { return classId; }
            set
            {
                if (Data.Classes[value] != null)
                {
                    classId = value;
                    if (!IsEquippable(Data.Weapons[weaponId]))
                        Equip(0, 0);
                    if (!IsEquippable(Data.Armors[armor1Id]))
                        Equip(1, 0);
                    if (!IsEquippable(Data.Armors[armor2Id]))
                        Equip(2, 0);
                    if (!IsEquippable(Data.Armors[armor3Id]))
                        Equip(3, 0);
                    if (!IsEquippable(Data.Armors[armor4Id]))
                        Equip(4, 0);
                }
            }
        }

        private int weaponId;
        public int WeaponId { get { return weaponId; } }

        private int armor1Id;
        public int Armor1Id { get { return armor1Id; } }

        private int armor2Id;
        public int Armor2Id { get { return armor2Id; } }

        private int armor3Id;
        public int Armor3Id { get { return armor3Id; } }

        private int armor4Id;
        public int Armor4Id { get { return armor4Id; } }

        private int level;
        public int Level 
        {
            get { return level; }
            set
            {
                int level = value.MinMax(1, Data.Actors[actorId].finalLevel);
                this.exp = expList[level];
            }
        }

        private int exp;
        public int Exp 
        {
            get { return exp; }
            set
            {
                exp = value.MinMax(0, 9999999);

                while (exp >= expList[level + 1] && expList[level + 1] > 0)
                {
                    level++;

                    foreach (Class.Learning j in Data.Classes[classId].learnings)
                        if (j.level == level)
                            LearnSkill(j.skillId);
                }

                while (exp < expList[level])
                    level--;

                hp = Math.Min(hp, MaxHp);
                sp = Math.Min(sp, MaxSp);
            }
        }

        private int[] skills;
        public int[] Skills { get { return skills; } }

        public int Id { get { return actorId; } }

        public override int Index { get { return Array.IndexOf<Actor>(Globals.GameParty.Actors, this); } }

        public override int[] StateRanks
        {
            get { return Data.Classes[classId].stateRanks; }
        }

        public override int[] ElementSet
        {
            get
            {
                Weapon weapon = Data.Weapons[weaponId];
                return weapon != null ? weapon.elementSet : new int[] { };
            }
        }

        public override int[] PlusStateSet
        {
            get
            {
                Weapon weapon = Data.Weapons[weaponId];
                return weapon != null ? weapon.plusStateSet : new int[] { };
            }
        }

        public override int[] MinusStateSet
        {
            get
            {
                Weapon weapon = Data.Weapons[weaponId];
                return weapon != null ? weapon.minusStateSet : new int[] { };
            }
        }

        public override int MaxHp
        {
            get
            {
                double n = (BaseMaxHp + maxHpPlus).MinMax(1, 9999);

                foreach (int i in states)
                    n *= Data.States[i].maxhpRate / 100.0;

                n = n.MinMax(1, 9999);
                return (int)n;
            }
            set
            {
                base.MaxHp = value;
            }
        }

        public override int BaseMaxHp
        {
            get { return Data.Actors[actorId].parameters[0, level]; }
        }

        public override int BaseMaxSp
        {
            get { return Data.Actors[actorId].parameters[1, level]; }
        }

        public override int BaseStr
        {
            get
            {
                int n = Data.Actors[actorId].parameters[2, level];

                Weapon weapon = Data.Weapons[weaponId];
                Armor armor1 = Data.Armors[armor1Id];
                Armor armor2 = Data.Armors[armor2Id];
                Armor armor3 = Data.Armors[armor3Id];
                Armor armor4 = Data.Armors[armor4Id];

                n += weapon == null ? 0 : weapon.strPlus;
                n += armor1 == null ? 0 : armor1.strPlus;
                n += armor2 == null ? 0 : armor2.strPlus;
                n += armor3 == null ? 0 : armor3.strPlus;
                n += armor4 == null ? 0 : armor4.strPlus;

                return n.MinMax(1, 999);
            }
        }

        public override int BaseDex
        {
            get
            {
                int n = Data.Actors[actorId].parameters[3, level];

                Weapon weapon = Data.Weapons[weaponId];
                Armor armor1 = Data.Armors[armor1Id];
                Armor armor2 = Data.Armors[armor2Id];
                Armor armor3 = Data.Armors[armor3Id];
                Armor armor4 = Data.Armors[armor4Id];

                n += weapon == null ? 0 : weapon.dexPlus;
                n += armor1 == null ? 0 : armor1.dexPlus;
                n += armor2 == null ? 0 : armor2.dexPlus;
                n += armor3 == null ? 0 : armor3.dexPlus;
                n += armor4 == null ? 0 : armor4.dexPlus;

                return n.MinMax(1, 999);
            }
        }

        public override int BaseAgi
        {
            get
            {
                int n = Data.Actors[actorId].parameters[4, level];

                Weapon weapon = Data.Weapons[weaponId];
                Armor armor1 = Data.Armors[armor1Id];
                Armor armor2 = Data.Armors[armor2Id];
                Armor armor3 = Data.Armors[armor3Id];
                Armor armor4 = Data.Armors[armor4Id];

                n += weapon == null ? 0 : weapon.agiPlus;
                n += armor1 == null ? 0 : armor1.agiPlus;
                n += armor2 == null ? 0 : armor2.agiPlus;
                n += armor3 == null ? 0 : armor3.agiPlus;
                n += armor4 == null ? 0 : armor4.agiPlus;

                return n.MinMax(1, 999);
            }
        }

        public override int BaseInt
        {
            get
            {
                int n = Data.Actors[actorId].parameters[5, level];

                Weapon weapon = Data.Weapons[weaponId];
                Armor armor1 = Data.Armors[armor1Id];
                Armor armor2 = Data.Armors[armor2Id];
                Armor armor3 = Data.Armors[armor3Id];
                Armor armor4 = Data.Armors[armor4Id];

                n += weapon == null ? 0 : weapon.intPlus;
                n += armor1 == null ? 0 : armor1.intPlus;
                n += armor2 == null ? 0 : armor2.intPlus;
                n += armor3 == null ? 0 : armor3.intPlus;
                n += armor4 == null ? 0 : armor4.intPlus;

                return n.MinMax(1, 999);
            }
        }

        public override int BaseAtk
        {
            get
            {
                Weapon weapon = Data.Weapons[weaponId];
                return weapon != null ? weapon.atk : 0;
            }
        }

        public override int BasePDef
        {
            get
            {
                int n = 0;

                Weapon weapon = Data.Weapons[weaponId];
                Armor armor1 = Data.Armors[armor1Id];
                Armor armor2 = Data.Armors[armor2Id];
                Armor armor3 = Data.Armors[armor3Id];
                Armor armor4 = Data.Armors[armor4Id];

                n += weapon == null ? 0 : weapon.pdef;
                n += armor1 == null ? 0 : armor1.pdef;
                n += armor2 == null ? 0 : armor2.pdef;
                n += armor3 == null ? 0 : armor3.pdef;
                n += armor4 == null ? 0 : armor4.pdef;

                return n;
            }
        }

        public override int BaseMDef
        {
            get
            {
                int n = 0;

                Weapon weapon = Data.Weapons[weaponId];
                Armor armor1 = Data.Armors[armor1Id];
                Armor armor2 = Data.Armors[armor2Id];
                Armor armor3 = Data.Armors[armor3Id];
                Armor armor4 = Data.Armors[armor4Id];

                n += weapon == null ? 0 : weapon.mdef;
                n += armor1 == null ? 0 : armor1.mdef;
                n += armor2 == null ? 0 : armor2.mdef;
                n += armor3 == null ? 0 : armor3.mdef;
                n += armor4 == null ? 0 : armor4.mdef;

                return n;
            }
        }

        public override int BaseEva
        {
            get
            {
                int n = 0;

                Armor armor1 = Data.Armors[armor1Id];
                Armor armor2 = Data.Armors[armor2Id];
                Armor armor3 = Data.Armors[armor3Id];
                Armor armor4 = Data.Armors[armor4Id];

                n += armor1 == null ? 0 : armor1.eva;
                n += armor2 == null ? 0 : armor2.eva;
                n += armor3 == null ? 0 : armor3.eva;
                n += armor4 == null ? 0 : armor4.eva;

                return n;
            }
        }

        public int Animation1Id
        {
            get
            {
                Weapon weapon = Data.Weapons[weaponId];
                return weapon != null ? weapon.animation1Id : 0;
            }
        }

        public int Animation2Id
        {
            get
            {
                Weapon weapon = Data.Weapons[weaponId];
                return weapon != null ? weapon.animation2Id : 0;
            }
        }

        public string ClassName
        {
            get { return Data.Classes[classId].name; }
        }

        public string ExpS
        {
            get { return expList[level + 1] > 0 ? exp.ToString() : "-------"; }
        }

        public string NextExpS
        {
            get { return expList[level + 1] > 0 ? expList[level + 1].ToString() : "-------"; }
        }

        public string NextRestExpS
        {
            get { return expList[level + 1] > 0 ? (expList[level + 1] - exp).ToString() : "-------"; }
        }

        public int ScreenX
        {
            get { return Index == -1 ? 0 : 160 * Index + 80; }
        }

        public int ScreenY
        {
            get { return 464; }
        }

        public int ScreenZ
        {
            get { return Index == -1 ? 0 : 4 - Index; }
        }
        #endregion

        int actorId;
        int[] expList;

        public Actor(int actorId) : base()
        {
            Setup(actorId);
        }

        public void Setup(int actorId)
        {
            DataClasses.Actor actor = Data.Actors[actorId];
            this.actorId = actorId;
            name = actor.name;
            characterName = actor.characterName;
            characterHue = actor.characterHue;
            battlerName = actor.battlerName;
            battlerHue = actor.battlerHue;
            classId = actor.classId;
            weaponId = actor.weaponId;
            armor1Id = actor.armor1Id;
            armor2Id = actor.armor2Id;
            armor3Id = actor.armor3Id;
            armor4Id = actor.armor4Id;
            level = actor.initialLevel;
            expList = new int[101];
            MakeExpList();
            exp = expList[level];
            skills = new int[] { };
            hp = MaxHp;
            sp = MaxSp;
            states = new int[] { };
            statesTurn = new Dictionary<int, int>();
            maxHpPlus = 0;
            maxSpPlus = 0;
            strPlus = 0;
            dexPlus = 0;
            agiPlus = 0;
            intPlus = 0;

            for (int i = 1; i < level; i++)
                foreach (DataClasses.Class.Learning j in Data.Classes[classId].learnings)
                    LearnSkill(j.skillId);

            UpdateAutoState(null, Data.Armors[armor1Id]);
            UpdateAutoState(null, Data.Armors[armor2Id]);
            UpdateAutoState(null, Data.Armors[armor3Id]);
            UpdateAutoState(null, Data.Armors[armor4Id]);
        }

        public void MakeExpList()
        {
            DataClasses.Actor actor = Data.Actors[actorId];
            expList[1] = 0;
            double powI = 2.4 + actor.expInflation / 100.0;
            for (int i = 2; i < 100; i++)
            {
                if (i > actor.finalLevel)
                    expList[i] = 0;
                else
                {
                    double n = actor.expBasis * (Math.Pow((i + 3), powI)) / Math.Pow(5, powI);
                    n = Math.Round(n, 5); //added to avoid rounding errors
                    expList[i] = expList[i - 1] + (int)n;
                }
            }
        }

        public override int ElementRate(int elementId)
        {
            int[] table = { 0, 200, 150, 100, 50, 0, -100 };
            int result = table[Data.Classes[classId].elementRanks[elementId]];

            foreach (int i in new int[] { armor1Id, armor2Id, armor3Id, armor4Id })
            {
                Armor armor = Data.Armors[i];
                if (armor != null && armor.guardElementSet.Includes(elementId))
                    result /= 2;
            }

            foreach (int i in states)
                if (Data.States[i].guardElementSet.Includes(elementId))
                    result /= 2;

            return result;
        }

        public override bool DoesStateGuard(int stateId)
        {
            foreach (int i in new int[] { armor1Id, armor2Id, armor3Id, armor4Id })
            {
                Armor armor = Data.Armors[i];
                if (armor != null)
                    if (armor.guardElementSet.Includes(stateId))
                        return true;
            }

            return false;
        }

        public void UpdateAutoState(Armor oldArmor, Armor newArmor)
        {
            if (oldArmor != null)
                if (oldArmor.autoStateId != 0)
                    RemoveState(oldArmor.autoStateId, true);

            if (newArmor != null)
                if (newArmor.autoStateId != 0)
                    AddState(newArmor.autoStateId, true);
        }

        public bool IsEquipFixed(int equipType)
        {
            switch (equipType)
            {
                case 0: return Data.Actors[actorId].weaponFix;
                case 1: return Data.Actors[actorId].armor1Fix;
                case 2: return Data.Actors[actorId].armor2Fix;
                case 3: return Data.Actors[actorId].armor3Fix;
                case 4: return Data.Actors[actorId].armor4Fix;
            }

            return false;
        }

        public void Equip(int equipType, int id)
        {
            switch (equipType)
            {
                case 0:
                    if (id == 0 || Globals.GameParty.WeaponNumber(id) > 0)
                    {
                        Globals.GameParty.GainWeapon(weaponId, 1);
                        weaponId = id;
                        Globals.GameParty.LoseWeapon(id, 1);
                    }
                    break;

                case 1:
                    if (id == 0 || Globals.GameParty.ArmorNumber(id) > 0)
                    {
                        UpdateAutoState(Data.Armors[armor1Id], Data.Armors[id]);
                        Globals.GameParty.GainArmor(armor1Id, 1);
                        armor1Id = id;
                        Globals.GameParty.LoseArmor(id, 1);
                    }
                    break;

                case 2:
                    if (id == 0 || Globals.GameParty.ArmorNumber(id) > 0)
                    {
                        UpdateAutoState(Data.Armors[armor2Id], Data.Armors[id]);
                        Globals.GameParty.GainArmor(armor2Id, 1);
                        armor2Id = id;
                        Globals.GameParty.LoseArmor(id, 1);
                    }
                    break;

                case 3:
                    if (id == 0 || Globals.GameParty.ArmorNumber(id) > 0)
                    {
                        UpdateAutoState(Data.Armors[armor3Id], Data.Armors[id]);
                        Globals.GameParty.GainArmor(armor3Id, 1);
                        armor3Id = id;
                        Globals.GameParty.LoseArmor(id, 1);
                    }
                    break;

                case 4:
                    if (id == 0 || Globals.GameParty.ArmorNumber(id) > 0)
                    {
                        UpdateAutoState(Data.Armors[armor4Id], Data.Armors[id]);
                        Globals.GameParty.GainArmor(armor4Id, 1);
                        armor4Id = id;
                        Globals.GameParty.LoseArmor(id, 1);
                    }
                    break;
            }
        }

        public bool IsEquippable(ItemType item)
        {
            if (item is Weapon)
            {
                if (Data.Classes[classId].weaponSet.Includes(item.id))
                    return true;
            }

            if (item is Armor)
            {
                if (Data.Classes[classId].armorSet.Includes(item.id))
                    return true;
            }

            return false;
        }

        public void LearnSkill(int skillId)
        {
            if (skillId > 0 && !IsSkillLearned(skillId))
            {
                skills = skills.Plus<int>(skillId);
                skills.Sort();
            }
        }

        public void ForgetSkill(int skillId)
        {
            skills = skills.Minus<int>(skillId);
        }

        public bool IsSkillLearned(int skillId)
        {
            return skills.Includes(skillId);
        }

        public override bool CanUseSkill(int skillId)
        {
            if (!IsSkillLearned(skillId))
                return false;
            else
                return base.CanUseSkill(skillId);
        }

        public void SetGraphic(string characterName, int characterHue, string battlerName, int battlerHue)
        {
            this.characterName = characterName;
            this.characterHue = characterHue;
            this.battlerName = battlerName;
            this.battlerHue = battlerHue;
        }
    }
}
