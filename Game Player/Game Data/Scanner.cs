//TODO: Event Command Parameters!

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DataClasses;

namespace Game_Player
{
    public class Scanner
    {
        public static StreamReader reader;
        static string[] stoppers = { "</array>", "</table>", "</x>", "</y>", "</hash>" };

        public static void Load(string file)
        {
            if (File.Exists(file))
            {
                reader = File.OpenText(file);

                Data.Actors = ReadDataArray<Actor>("</data_actors>", ReadActor);
                Data.Classes = ReadDataArray<Class>("</data_classes>", ReadClass);
                Data.Skills = ReadDataArray<Skill>("</data_skills>", ReadSkill);
                Data.Items = ReadDataArray<Item>("</data_items>", ReadItem);
                Data.Weapons = ReadDataArray<Weapon>("</data_weapons>", ReadWeapon);
                Data.Armors = ReadDataArray<Armor>("</data_armors>", ReadArmor);
                Data.Enemies = ReadDataArray<Enemy>("</data_enemies>", ReadEnemy);
                Data.Troops = ReadDataArray<Troop>("</data_troops>", ReadTroop);
                Data.States = ReadDataArray<State>("</data_states>", ReadState);
                Data.Animations = ReadDataArray<Animation>("</data_animations>", ReadAnimation);
                Data.Tilesets = ReadDataArray<Tileset>("</data_tilesets>", ReadTileset);
                Data.CommonEvents = ReadDataArray<CommonEvent>("</data_common_events>", ReadCommonEvent);
                reader.ReadLine();
                Data.Misc = ReadMisc();
                reader.ReadLine();
                Data.Maps = ReadDataArray<Map>("</data_maps>", ReadMap);
            }
        }

        static Map ReadMap()
        {
            Map map = new Map(1, 1);

            map.tilesetId = ReadInt();
            map.width = ReadInt();
            map.height = ReadInt();
            map.autoplayBgm = ReadBool();
            reader.ReadLine();
            map.bgm = ReadAudioFile();
            reader.ReadLine();
            map.autoplayBgs = ReadBool();
            reader.ReadLine();
            map.bgs = ReadAudioFile();
            reader.ReadLine();
            map.encounterList = ReadIntArray();
            map.encounterStep = ReadInt();
            map.data = ConvertTable(Read3DTable());

            Event[] events = ReadArray<Event>(ReadEvent);
            map.events = new Dictionary<int, Event>();
            foreach (Event e in events)
                map.events.Add(e.id, e);

            map.id = ReadInt();
            map.name = ReadString();
            map.parentId = ReadInt();
            map.order = ReadInt();
            map.expanded = ReadBool();
            map.scrollX = ReadInt();
            map.scrollY = ReadInt();

            return map;
        }

        static Misc.Words ReadMiscWords()
        {
            Misc.Words words = new Misc.Words();

            words.gold = ReadString();
            words.hp = ReadString();
            words.sp = ReadString();
            words.str = ReadString();
            words.dex = ReadString();
            words.agi = ReadString();
            words.intel = ReadString();
            words.atk = ReadString();
            words.pdef = ReadString();
            words.mdef = ReadString();
            words.weapon = ReadString();
            words.armor1 = ReadString();
            words.armor2 = ReadString();
            words.armor3 = ReadString();
            words.armor4 = ReadString();
            words.attack = ReadString();
            words.skill = ReadString();
            words.guard = ReadString();
            words.item = ReadString();
            words.equip = ReadString();

            return words;
        }

        static Misc.TestBattler ReadMiscTestBattler()
        {
            Misc.TestBattler battler = new Misc.TestBattler();

            battler.actorId = ReadInt();
            battler.level = ReadInt();
            battler.weaponId = ReadInt();
            battler.armor1Id = ReadInt();
            battler.armor2Id = ReadInt();
            battler.armor3Id = ReadInt();
            battler.armor4Id = ReadInt();

            return battler;
        }

        static Misc ReadMisc()
        {
            Misc misc = new Misc();

            misc.magicNumber = ReadInt();
            misc.partyMembers = ReadIntArray();
            misc.elements = ReadStringArray();
            misc.switches = ReadStringArray();
            misc.variables = ReadStringArray();
            misc.windowSkinName = ReadString();
            misc.titleName = ReadString();
            misc.gameoverName = ReadString();
            misc.battleTransition = ReadString();
            reader.ReadLine();
            misc.titleBgm = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.battleBgm = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.battleEndMe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.gameoverMe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.cursorSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.decisionSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.cancelSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.buzzerSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.equipSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.shopSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.saveSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.loadSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.battleStartSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.escapeSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.actorCollapseSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.enemySollapseSe = ReadAudioFile();
            reader.ReadLine();
            reader.ReadLine();
            misc.words = ReadMiscWords();
            reader.ReadLine();
            misc.testBattlers = ReadArray<Misc.TestBattler>(ReadMiscTestBattler);
            misc.testTroopId = ReadInt();
            misc.startMapId = ReadInt();
            misc.startX = ReadInt();
            misc.startY = ReadInt();
            misc.battlebackName = ReadString();
            misc.battlerName = ReadString();
            misc.battlerHue = ReadInt();
            misc.editMapId = ReadInt();

            return misc;
        }

        static CommonEvent ReadCommonEvent()
        {
            CommonEvent evt = new CommonEvent();

            evt.id = ReadInt();
            evt.name = ReadString();
            evt.trigger = ReadInt();
            evt.switchId = ReadInt();
            evt.list = ReadArray<EventCommand>(ReadEventCommand);

            return evt;
        }

        static MoveCommand ReadMoveCommand()
        {
            MoveCommand command = new MoveCommand();

            command.code = ReadInt();
            command.parameters = ReadStringArray();

            return command;
        }

        static MoveRoute ReadMoveRoute()
        {
            MoveRoute route = new MoveRoute();

            route.repeat = ReadBool();
            route.skippable = ReadBool();
            route.list = ReadArray<MoveCommand>(ReadMoveCommand);

            return route;
        }

        static Event.Page.Graphic ReadEventPageGraphic()
        {
            Event.Page.Graphic graphic = new Event.Page.Graphic();

            graphic.tileId = ReadInt();
            graphic.characterName = ReadString();
            graphic.characterHue = ReadInt();
            graphic.direction = ReadInt();
            graphic.pattern = ReadInt();
            graphic.opacity = ReadInt();
            graphic.blendType = ReadInt();
            
            return graphic;
        }

        static Event.Page.Condition ReadEventPageCondition()
        {
            Event.Page.Condition condition = new Event.Page.Condition();

            condition.switch1Valid = ReadBool();
            condition.switch2Valid = ReadBool();
            condition.variableValid = ReadBool();
            condition.selfSwitchValid = ReadBool();
            condition.switch1Id = ReadInt();
            condition.switch2Id = ReadInt();
            condition.variableId = ReadInt();
            condition.variableValue = ReadInt();
            condition.selfSwitchCh = ReadChar();

            return condition;
        }

        static Event.Page ReadEventPage()
        {
            Event.Page page = new Event.Page();

            reader.ReadLine();
            page.condition = ReadEventPageCondition();
            reader.ReadLine();
            reader.ReadLine();
            page.graphic = ReadEventPageGraphic();
            reader.ReadLine();
            page.moveType = ReadInt();
            page.moveSpeed = ReadInt();
            page.moveFrequency = ReadInt();
            reader.ReadLine();
            page.moveRoute = ReadMoveRoute();
            reader.ReadLine();
            page.walkAnime = ReadBool();
            page.stepAnime = ReadBool();
            page.directionFix = ReadBool();
            page.through = ReadBool();
            page.alwaysOnTop = ReadBool();
            page.trigger = ReadInt();
            page.list = ReadArray<EventCommand>(ReadEventCommand);

            return page;
        }

        static Event ReadEvent()
        {
            Event evt = new Event(0, 0);

            evt.id = ReadInt();
            evt.name = ReadString();
            evt.x = ReadInt();
            evt.y = ReadInt();
            evt.pages = ReadArray<Event.Page>(ReadEventPage);

            return evt;
        }

        static Tileset ReadTileset()
        {
            Tileset tile = new Tileset();

            tile.id = ReadInt();
            tile.name = ReadString();
            tile.tilesetName = ReadString();
            tile.autotileNames = ReadStringArray();
            tile.panoramaName = ReadString();
            tile.panoramaHue = ReadInt();
            tile.fogName = ReadString();
            tile.fogHue = ReadInt();
            tile.fogOpacity = ReadInt();
            tile.fogBlendType = ReadInt();
            tile.fogZoom = ReadInt();
            tile.fogSx = ReadInt();
            tile.fogSy = ReadInt();
            tile.battlebackName = ReadString();
            tile.passages = ReadIntArray();
            tile.priorities = ReadIntArray();
            tile.terrainTags = ReadIntArray();

            return tile;
        }

        static Color ReadColor()
        {
            Color color = new Color();

            color.Red = (int)ReadDouble();
            color.Green = (int)ReadDouble();
            color.Blue = (int)ReadDouble();
            color.Alpha = (int)ReadDouble();

            return color;
        }

        static Animation.Timing ReadAnimationTiming()
        {
            Animation.Timing timing = new Animation.Timing();

            timing.frame = ReadInt();
            reader.ReadLine();
            timing.se = ReadAudioFile();
            reader.ReadLine();
            timing.flashScope = ReadInt();
            reader.ReadLine();
            timing.flashColor = ReadColor();
            reader.ReadLine();
            timing.flashDuration = ReadInt();
            timing.condition = ReadInt();

            return timing;
        }

        static Animation.Frame ReadAnimationFrame()
        {
            Animation.Frame frame = new Animation.Frame();

            frame.cellMax = ReadInt();
            frame.cellData = ConvertTable(Read2DTable());

            return frame;
        }

        static Animation ReadAnimation()
        {
            Animation animation = new Animation();

            animation.id = ReadInt();
            animation.name = ReadString();
            animation.animationName = ReadString();
            animation.animationHue = ReadInt();
            animation.position = ReadInt();
            animation.frameMax = ReadInt();
            animation.frames = ReadArray<Animation.Frame>(ReadAnimationFrame);
            animation.timings = ReadArray<Animation.Timing>(ReadAnimationTiming);

            return animation;
        }

        static State ReadState()
        {
            State state = new State();

            state.id = ReadInt();
            state.name = ReadString();
            state.animationId = ReadInt();
            state.restriction = ReadInt();
            state.nonresistance = ReadBool();
            state.zeroHp = ReadBool();
            state.cantGetExp = ReadBool();
            state.cantEvade = ReadBool();
            state.slipDamage = ReadBool();
            state.rating = ReadInt();
            state.hitRate = ReadInt();
            state.maxhpRate = ReadInt();
            state.maxspRate = ReadInt();
            state.strRate = ReadInt();
            state.dexRate = ReadInt();
            state.agiRate = ReadInt();
            state.intRate = ReadInt();
            state.atkRate = ReadInt();
            state.pdefRate = ReadInt();
            state.mdefRate = ReadInt();
            state.eva = ReadInt();
            state.battleOnly = ReadBool();
            state.holdTurn = ReadInt();
            state.autoReleaseProb = ReadInt();
            state.shockReleaseProb = ReadInt();
            state.guardElementSet = ReadIntArray();
            state.plusStateSet = ReadIntArray();
            state.minusStateSet = ReadIntArray();

            return state;
        }

        static Parameter ReadEventCommandParameters()
        {
            Parameter prms = new Parameter();

            string line = ReadString();
            while (line != "</array>")
            {
                if (line == "<array>")
                    prms.Add(ReadEventCommandParameters());
                else if (line == "<route>")
                {
                    prms.Add(ReadMoveRoute());
                    ReadString();
                }
                else
                    prms.Add(line);

                line = ReadString();
            }

            return prms;
        }

        static EventCommand ReadEventCommand()
        {
            EventCommand command = new EventCommand();

            command.code = ReadInt();
            command.indent = ReadInt();
            ReadString();
            command.parameters = ReadEventCommandParameters();

            return command;
        }

        static Troop.Page.Condition ReadTroopPageCondition()
        {
            Troop.Page.Condition condition = new Troop.Page.Condition();

            condition.turnValid = ReadBool();
            condition.enemyValid = ReadBool();
            condition.actorValid = ReadBool();
            condition.switchValid = ReadBool();
            condition.turnA = ReadInt();
            condition.turnB = ReadInt();
            condition.enemyIndex = ReadInt();
            condition.enemyHp = ReadInt();
            condition.actorId = ReadInt();
            condition.actorHp = ReadInt();
            condition.switchId = ReadInt();

            return condition;
        }

        static Troop.Page ReadTroopPage()
        {
            Troop.Page page = new Troop.Page();

            reader.ReadLine();
            page.condition = ReadTroopPageCondition();
            reader.ReadLine();
            page.span = ReadInt();
            page.list = ReadArray<EventCommand>(ReadEventCommand);

            return page;
        }

        static Troop.Member ReadTroopMember()
        {
            Troop.Member member = new Troop.Member();

            member.enemyId = ReadInt();
            member.x = ReadInt();
            member.y = ReadInt();
            member.hidden = ReadBool();
            member.immortal = ReadBool();

            return member;
        }

        static Troop ReadTroop()
        {
            Troop troop = new Troop();

            troop.id = ReadInt();
            troop.name = ReadString();
            troop.members = ReadArray<Troop.Member>(ReadTroopMember);
            troop.pages = ReadArray<Troop.Page>(ReadTroopPage);

            return troop;
        }

        static Enemy.Action ReadEnemyAction()
        {
            Enemy.Action action = new Enemy.Action();

            action.kind = ReadInt();
            action.basic = ReadInt();
            action.skillId = ReadInt();
            action.conditionTurn_a = ReadInt();
            action.conditionTurn_b = ReadInt();
            action.conditionHp = ReadInt();
            action.conditionLevel = ReadInt();
            action.conditionSwitch_id = ReadInt();
            action.rating = ReadInt();

            return action;
        }


        static Enemy ReadEnemy()
        {
            Enemy enemy = new Enemy();

            enemy.id = ReadInt();
            enemy.name = ReadString();
            enemy.battlerName = ReadString();
            enemy.battlerHue = ReadInt();
            enemy.maxhp = ReadInt();
            enemy.maxsp = ReadInt();
            enemy.str = ReadInt();
            enemy.dex = ReadInt();
            enemy.agi = ReadInt();
            enemy.intel = ReadInt();
            enemy.atk = ReadInt();
            enemy.pdef = ReadInt();
            enemy.mdef = ReadInt();
            enemy.eva = ReadInt();
            enemy.animation1Id = ReadInt();
            enemy.animation2Id = ReadInt();
            enemy.elementRanks = ReadIntArray();
            enemy.stateRanks = ReadIntArray();
            enemy.actions = ReadArray<Enemy.Action>(ReadEnemyAction);
            enemy.exp = ReadInt();
            enemy.gold = ReadInt();
            enemy.itemId = ReadInt();
            enemy.weaponId = ReadInt();
            enemy.armorId = ReadInt();
            enemy.treasureProb = ReadInt();

            return enemy;
        }

        static Armor ReadArmor()
        {
            Armor armor = new Armor();

            armor.id = ReadInt();
            armor.name = ReadString();
            armor.iconName = ReadString();
            armor.description = ReadString();
            armor.kind = ReadInt();
            armor.autoStateId = ReadInt();
            armor.price = ReadInt();
            armor.pdef = ReadInt();
            armor.mdef = ReadInt();
            armor.eva = ReadInt();
            armor.strPlus = ReadInt();
            armor.dexPlus = ReadInt();
            armor.agiPlus = ReadInt();
            armor.intPlus = ReadInt();
            armor.guardElementSet = ReadIntArray();
            armor.guardStateSet = ReadIntArray();

            return armor;
        }

        static Weapon ReadWeapon()
        {
            Weapon weapon = new Weapon();

            weapon.id = ReadInt();
            weapon.name = ReadString();
            weapon.iconName = ReadString();
            weapon.description = ReadString();
            weapon.animation1Id = ReadInt();
            weapon.animation2Id = ReadInt();
            weapon.price = ReadInt();
            weapon.atk = ReadInt();
            weapon.pdef = ReadInt();
            weapon.mdef = ReadInt();
            weapon.strPlus = ReadInt();
            weapon.dexPlus = ReadInt();
            weapon.agiPlus = ReadInt();
            weapon.intPlus = ReadInt();
            weapon.elementSet = ReadIntArray();
            weapon.plusStateSet = ReadIntArray();
            weapon.minusStateSet = ReadIntArray();

            return weapon;
        }

        static Item ReadItem()
        {
            Item item = new Item();

            item.id = ReadInt();
            item.name = ReadString();
            item.iconName = ReadString();
            item.description = ReadString();
            item.scope = ReadInt();
            item.occasion = ReadInt();
            item.animation1Id = ReadInt();
            item.animation2Id = ReadInt();
            reader.ReadLine();
            item.menuSe = ReadAudioFile();
            reader.ReadLine();
            item.commonEventId = ReadInt();
            item.price = ReadInt();
            item.consumable = ReadBool();
            item.parameterType = ReadInt();
            item.parameterPoints = ReadInt();
            item.recoverHpRate = ReadInt();
            item.recoverHp = ReadInt();
            item.recoverSpRate = ReadInt();
            item.recoverSp = ReadInt();
            item.hit = ReadInt();
            item.pdefF = ReadInt();
            item.mdefF = ReadInt();
            item.variance = ReadInt();
            item.elementSet = ReadIntArray();
            item.plusStateSet = ReadIntArray();
            item.minusStateSet = ReadIntArray();

            return item;
        }

        static AudioFile ReadAudioFile()
        {
            AudioFile file = new AudioFile();

            file.name = ReadString();
            file.volume = ReadInt();
            file.pitch = ReadInt();

            return file;
        }

        static Skill ReadSkill()
        {
            Skill skill = new Skill();

            skill.id = ReadInt();
            skill.name = ReadString();
            skill.iconName = ReadString();
            skill.description = ReadString();
            skill.scope = ReadInt();
            skill.occasion = ReadInt();
            skill.animation1Id = ReadInt();
            skill.animation2Id = ReadInt();
            reader.ReadLine();
            skill.menuSe = ReadAudioFile();
            reader.ReadLine();
            skill.commonEventId = ReadInt();
            skill.spCost = ReadInt();
            skill.power = ReadInt();
            skill.atkF = ReadInt();
            skill.evaF = ReadInt();
            skill.strF = ReadInt();
            skill.dexF = ReadInt();
            skill.agiF = ReadInt();
            skill.intF = ReadInt();
            skill.hit = ReadInt();
            skill.pdefF = ReadInt();
            skill.mdefF = ReadInt();
            skill.variance = ReadInt();
            skill.elementSet = ReadIntArray();
            skill.plusStateSet = ReadIntArray();
            skill.minusStateSet = ReadIntArray();

            return skill;
        }

        static Class.Learning ReadClassLearning()
        {
            Class.Learning learning = new Class.Learning();

            learning.level = ReadInt();
            learning.skillId = ReadInt();

            return learning;
        }

        static Class ReadClass()
        {
            Class cls = new Class();

            cls.id = ReadInt();
            cls.name = ReadString();
            cls.position = ReadInt();
            cls.weaponSet = ReadIntArray();
            cls.armorSet = ReadIntArray();
            cls.elementRanks = ReadIntArray();
            cls.stateRanks = ReadIntArray();
            cls.learnings = ReadArray<Class.Learning>(ReadClassLearning);

            return cls;
        }
        
        static Actor ReadActor()
        {
            Actor actor = new Actor();

            actor.id = ReadInt();
            actor.name = ReadString();
            actor.classId = ReadInt();
            actor.initialLevel = ReadInt();
            actor.finalLevel = ReadInt();
            actor.expBasis = ReadInt();
            actor.expInflation = ReadInt();
            actor.characterName = ReadString();
            actor.characterHue = ReadInt();
            actor.battlerName = ReadString();
            actor.battlerHue = ReadInt();
            actor.parameters = ConvertTable(Read2DTable());
            actor.weaponId = ReadInt();
            actor.armor1Id = ReadInt();
            actor.armor2Id = ReadInt();
            actor.armor3Id = ReadInt();
            actor.armor4Id = ReadInt();
            actor.weaponFix = ReadBool();
            actor.armor1Fix = ReadBool();
            actor.armor2Fix = ReadBool();
            actor.armor3Fix = ReadBool();
            actor.armor4Fix = ReadBool();

            return actor;
        }

        delegate object ReadData();
        static DataArray<T> ReadDataArray<T>(string stop, ReadData readData)
        {
            DataArray<T> data = new DataArray<T>();

            reader.ReadLine();
            string line = reader.ReadLine();
            while (line != stop)
            {
                data.Add((T)readData());

                reader.ReadLine();

                line = reader.ReadLine();
            }

            return data;
        }

        static int[, ,] ConvertTable(int[][][] array)
        {
            if (array.Length == 0)
                return new int[0, 0, 0];
            else if (array[0].Length == 0)
                return new int[0, 0, 0];
            else
            {
                int[, ,] newArray = new int[array.Length, array[0].Length, array[0][0].Length];
                for (int i = 0; i < array.Length; i++)
                    for (int j = 0; j < array[0].Length; j++)
                        for (int k = 0; k < array[0][0].Length; k++)
                            newArray[i, j, k] = array[i][j][k];

                return newArray;
            }
        }

        static int[,] ConvertTable(int[][] array)
        {
            if (array.Length == 0)
                return new int[0, 0];
            else
            {
                int[,] newArray = new int[array.Length, array[0].Length];

                for (int i = 0; i < array.Length; i++)
                    for (int j = 0; j < array[i].Length; j++)
                        newArray[i, j] = array[i][j];

                return newArray;
            }
        }

        static int[][][] Read3DTable()
        {
            int[][][] array = new int[][][] { };

            reader.ReadLine();
            string line = reader.ReadLine();
            while(line != "</table>")
            {
                Array.Resize<int[][]>(ref array, array.Length + 1);
                array[array.Length - 1] = Read2DTable(true);

                line = reader.ReadLine();
            }

            return array;
        }

        static int[][] Read2DTable() { return Read2DTable(false); }

        static int[][] Read2DTable(bool from3D)
        {
            int[][] array = new int[][] { };

            if (!from3D)
                reader.ReadLine();
            string line = reader.ReadLine();
            while (Array.IndexOf(stoppers, line) == -1)
            {
                Array.Resize<int[]>(ref array, array.Length + 1);
                array[array.Length - 1] = ReadIntArray(true);

                line = reader.ReadLine();
            }

            return array;
        }

        delegate object ReadMethod();
        static T[] ReadArray<T>(ReadMethod readMethod)
        {
            T[] array = { };

            reader.ReadLine();
            string line = reader.ReadLine();
            while (line != "</array>" && line != "</hash>")
            {
                Array.Resize<T>(ref array, array.Length + 1);
                array[array.Length - 1] = (T)readMethod();
                reader.ReadLine();
                line = reader.ReadLine();
            } 

            return array;
        }

        static int[] ReadIntArray() { return ReadIntArray(false); }

        static int[] ReadIntArray(bool from2D)
        {
            int[] array = { };

            if (!from2D)
                reader.ReadLine();
            string line = reader.ReadLine();
            while (Array.IndexOf(stoppers, line) == -1)
            {
                Array.Resize<int>(ref array, array.Length + 1);
                if (line.Length == 0)
                    array[array.Length - 1] = 0;
                else
                    array[array.Length - 1] = Int32.Parse(line);
                line = reader.ReadLine();
            }

            return array;
        }

        static bool[] ReadBoolArray()
        {
            bool[] array = { };

            reader.ReadLine();
            string line = reader.ReadLine();
            while (Array.IndexOf(stoppers, line) == -1)
            {
                Array.Resize<bool>(ref array, array.Length + 1);
                array[array.Length - 1] = Boolean.Parse(line);

                line = reader.ReadLine();
            }

            return array;
        }

        static string[] ReadStringArray()
        {
            string[] array = { };

            reader.ReadLine();
            string line = reader.ReadLine();
            while (Array.IndexOf(stoppers, line) == -1)
            {
                Array.Resize<string>(ref array, array.Length + 1);
                array[array.Length - 1] = line;

                line = reader.ReadLine();
            }

            return array;
        }

        static String ReadString()
        {
            return reader.ReadLine();
        }

        static int ReadInt()
        {
            return Int32.Parse(reader.ReadLine());
        }

        static double ReadDouble()
        {
            return Double.Parse(reader.ReadLine());
        }

        static bool ReadBool()
        {
            return Boolean.Parse(reader.ReadLine());
        }

        static char ReadChar()
        {
            return Char.Parse(reader.ReadLine());
        }
    }
}
