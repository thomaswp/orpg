using System;
using System.Collections.Generic;
using System.Text;
using DataClasses;

namespace Game_Player.Game
{
    //NOT FINISHED
    public class Party
    {

#region Properties
        Actor[] actors;
        public Actor[] Actors { get { return actors; } }

        int gold;
        public int Gold { get { return gold; } }

        int steps;
        public int Steps { get { return steps; } }

        public int MaxLevel
        {
            get
            {
                if (actors.Length == 0)
                    return 0;

                int level = 0;

                foreach (Actor actor in actors)
                    if (level < actor.Level)
                        level = actor.Level;
                return level;
            }
        }

        public bool IsImputable
        {
            get
            {
                foreach (Actor actor in actors)
                    if (actor.IsImputable)
                        return true;

                return false;
            }
        }

        public bool AreAllDead
        {
            get
            {
                if (Globals.GameParty.actors.Length == 0)
                    return false;

                foreach (Actor actor in actors)
                    if (actor.Hp > 0)
                        return false;

                return true;
            }
        }
#endregion

        int[] items;
        int[] weapons;
        int[] armors;

        public Party()
        {
            actors = new Actor[Data.Misc.partyMembers.Length];
            items = new int[Data.Items.Length];
            weapons = new int[Data.Weapons.Length];
            armors = new int[Data.Armors.Length];
        }

        public void SetupStartingMembers()
        {
            Array.Resize<Actor>(ref actors, Data.Misc.partyMembers.Length);
            for (int i = 0; i < Data.Misc.partyMembers.Length; i++)
                Actors[i] = Globals.GameActors[Data.Misc.partyMembers[i]];
        }

        public void SetupBattleTestMembers()
        {
            Array.Resize<Actor>(ref actors, Data.Misc.testBattlers.Length);
            for (int i = 0; i < Data.Misc.testBattlers.Length; i++)
            {
                Misc.TestBattler battler = Data.Misc.testBattlers[i];
                Actor actor = Globals.GameActors[battler.actorId];
                actor.Level = battler.level;
                GainWeapon(battler.weaponId, 1);
                GainArmor(battler.armor1Id, 1);
                GainArmor(battler.armor2Id, 1);
                GainArmor(battler.armor3Id, 1);
                GainArmor(battler.armor4Id, 1);
                actor.Equip(0, battler.weaponId);
                actor.Equip(1, battler.armor1Id);
                actor.Equip(2, battler.armor2Id);
                actor.Equip(3, battler.armor3Id);
                actor.Equip(4, battler.armor4Id);
                actor.RecoverAll();
                actors[i] = actor;
            }
        }

        public void Refresh()
        {
            Actor[] newActors = new Actor[] { };
            for (int i = 0; i < actors.Length; i++ )
            {
                if (Data.Actors[actors[i].Id] != null)
                {
                    Array.Resize<Actor>(ref newActors, newActors.Length + 1);
                    newActors[newActors.Length - 1] = Globals.GameActors[actors[i].Id];
                }
            }
        }

        public void AddActor(int actorId)
        {
            Actor actor = Globals.GameActors[actorId];

            if (actors.Length < 4 && !actors.Includes(actor))
            {
                Array.Resize<Actor>(ref actors, actors.Length + 1);
                actors[actors.Length - 1] = actor;

                Globals.GamePlayer.Refresh();
            }
        }

        public void RemoveActor(int actorId)
        {
            actors = actors.Minus<Actor>(Globals.GameActors[actorId]);

            Globals.GamePlayer.Refresh();
        }

        public void GainGold(int n)
        {
            gold = (gold + n).MinMax(0, 9999999);
        }

        public void LoseGold(int n)
        {
            GainGold(-n);
        }

        public void IncreaseSteps()
        {
            steps = Math.Min(steps + 1, 9999999);
        }

        public int ItemNumber(int itemId)
        {
            return itemId < items.Length ? items[itemId] : 0;
        }

        public int WeaponNumber(int weaponId)
        {
            return weaponId < weapons.Length ? weapons[weaponId] : 0;
        }

        public int ArmorNumber(int armorId)
        {
            return armorId < armors.Length ? armors[armorId] : 0;
        }

        public void GainItem(int itemId, int n)
        {
            if (itemId >= items.Length) //added
                Array.Resize<int>(ref items, itemId + 1);

            if (itemId > 0)
                items[itemId] = (ItemNumber(itemId) + n).MinMax(0, 99);
        }

        public void GainWeapon(int weaponId, int n)
        {
            if (weaponId >= weapons.Length) //added
                Array.Resize<int>(ref weapons, weaponId + 1);

            if (weaponId > 0)
                weapons[weaponId] = (WeaponNumber(weaponId) + n).MinMax(0, 99);
        }

        public void GainArmor(int armorId, int n)
        {
            if (armorId >= armors.Length) //added
                Array.Resize<int>(ref armors, armorId + 1);

            if (armorId > 0)
                armors[armorId] = (ArmorNumber(armorId) + n).MinMax(0, 99);
        }

        public void LoseItem(int itemId, int n)
        {
            GainItem(itemId, -n);
        }

        public void LoseWeapon(int weaponId, int n)
        {
            GainWeapon(weaponId, -n);
        }

        public void LoseArmor(int armorId, int n)
        {
            GainArmor(armorId, -n);
        }

        public bool CanUseItem(int itemId)
        {
            if (ItemNumber(itemId) == 0)
                return false;

            int occasion = Data.Items[itemId].occasion;

            if (Globals.GameTemp.inBattle)
                return occasion == 0 || occasion == 1;

            return occasion == 0 || occasion == 2;
        }

        public void ClearActors()
        {
            foreach (Actor actor in actors)
                actor.CurrentAction.Clear();
        }

        public void CheckMapSlipDamage()
        {
            foreach (Actor actor in actors)
            {
                if (actor.Hp > 0 && actor.SlipDamage)
                {
                    actor.Hp -= Math.Max(actor.MaxHp / 100, 1);
                    if (actor.Hp == 0)
                        Audio.SE.Play(Data.Misc.actorCollapseSe);

                    Globals.GameScreen.StartFlash(new Color(255, 0, 0, 128), 4);
                    Globals.GameTemp.gameover = Globals.GameParty.AreAllDead;
                }
            }
        }

        public Actor RandomTargetActor() { return RandomTargetActor(false); }
        public Actor RandomTargetActor(bool hp0)
        {
            Actor[] roulette = { };

            foreach (Actor actor in actors)
            {
                if ((!hp0 && actor.Exists) || (hp0 && actor.IsHp0))
                {
                    int position = Data.Classes[actor.ClassId].position;
                    int n = 4 - position;
                    for (int i = 0; i < n; i++)
                    {
                        Array.Resize<Actor>(ref roulette, roulette.Length + 1);
                        roulette[roulette.Length - 1] = actor;
                    }
                }
            }

            if (actors.Length == 0)
                return null;

            return roulette[Rand.Next(roulette.Length)];
        }

        public Actor RandomTargetActorHp0()
        {
            return RandomTargetActor(true);
        }

        public Actor SmoothTargetActor(int actorIndex)
        {
            Actor actor = actors[actorIndex];

            if (actor != null && actor.Exists)
                return actor;

            foreach (Actor a in actors)
                if (a.Exists)
                    return actor;

            return null;
        }
    }
}
