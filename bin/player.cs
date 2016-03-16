using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{
    class player
    {

        private static string name = "";
        private static int strength = 0;
        private static int inteligence = 0;
        private static int dexterity = 0;
        private static int health = 0;
        private static int curhealth = 0;
        private static int money = 0;

        private static Dictionary<string, equipment> set = new Dictionary<string, equipment>();

        private static int[] savegameFlags = new int[game.savegameFlags];


        private static List<Iitem> inventory = new List<Iitem>();

        private static DeathCall died = null;
        private static DamageCall dmg = null;

        public delegate void DeathCall();
        public delegate void DamageCall();

        public static int[] getPlayerFlags()
        {
            return savegameFlags;
        }

        public static void equip(equipment e)
        {
            if (set.ContainsKey(e.Type()))
            {
                giveItem(set[e.Type()]);
            }
                set[e.Type()] = e;
            game.findAction("EQUIP").getParamList().Remove(e.getName().ToUpper());
            game.findAction("UNEQUIP").addParam(e.getName().ToUpper());
        }

        public static void removeEquipment(equipment e)
        {
            if (findEquipment(e.Type()) != null)
            {
                set.Remove(e.Type());
                giveItem(e);
                game.findAction("UNEQUIP").getParamList().Remove(e.getName().ToUpper());
            }
        }

        public static equipment findEquipment(string type)
        {
            if (set.ContainsKey(type))
                return set[type];

            return null;
        }

        public static equipment getEquipment(string name)
        {
            foreach (KeyValuePair<string, equipment> e in set)
            {
                if (e.Value.getName().ToUpper() == name.ToUpper())
                {
                    return e.Value;
                }
            }
            return null;
        }

        public static bool setName(string name)
        {
            if (name == "" || name == null)
                return false;

            player.name = name;
            return true;
        }

        public static string getName()
        {
            return name;
        }

        public static void die()
        {
            if (died == null)
            {
                CAFe.death();
            }
            else
                died();

        }

        public static void removeItem(Iitem i)
        {
            if (findItem(i.getName()) == null)
                return;
            inventory.Remove(i);
            if(i.itemType().ToUpper() == "EQUIPMENT")
                game.findAction("EQUIP").getParamList().Remove(i.getName().ToUpper());
            if (i.itemType().ToUpper() == "USABLE")
                game.findAction("USE").getParamList().Remove(i.getName().ToUpper());
        }

        public static void giveItem(Iitem i)
        {
            inventory.Add(i);

            if (i.itemType().ToUpper() == "EQUIPMENT")
                game.findAction("EQUIP").addParam(i.getName());
            if (i.itemType().ToUpper() == "USABLE")
                game.findAction("USE").addParam(i.getName());

            game.findAction("TAKE").getParamList().Remove(i.getName().ToUpper());
        }

        public static Iitem findItem(string name)
        {
            foreach (Iitem i in inventory)
            {
                if (i.getName().ToUpper() == name.ToUpper())
                {
                    return i;
                }
            }
            return null;
        }

        public static void setOnDeath(DeathCall death)
        {
            died = death;
        }

        public static int getStrength()
        {
            int added = strength;

            foreach(KeyValuePair<string, equipment> e in set)
            {
                added += e.Value.getStrength();
            }

            return added;
        }

        public static int getHealth()
        {
            return curhealth;
        }

        public static int getMaxHealth()
        {
            return health;
        }

        public static void setHealth(int hp)
        {
            if (hp > health)
                hp = health;
            if (hp < 0)
                hp = 0;
            curhealth = hp;

        }

        public static void setMaxHealth(int hp)
        {
            health = hp;
        }

        public static int getInteligence()
        {
            int added = inteligence;

            foreach (KeyValuePair<string, equipment> e in set)
            {
                added += e.Value.getInteligence();
            }

            return added;
        }

        public static int getDexterity()
        {
            int added = dexterity;

            foreach (KeyValuePair<string, equipment> e in set)
            {
                added += e.Value.getDexterity();
            }

            return added;
        }

        public static int getBaseStrength()
        {
            return strength;
        }

        public static int getBaseInteligence()
        {
            return inteligence;
        }

        public static int getBaseDexterity()
        {
            return dexterity;
        }

        public static int getMoney()
        {
            return money;
        }

        public static void setStrength(int s)
        {
            strength = s;

        }

        public static void setMoney(int m)
        {
            money = m;
        }

        public static List<Iitem> getInventory()
        {
            return inventory;
        }

        public static Dictionary<string,equipment> getSet()
        {
            return set;
        }

        public static void setDexterity(int d)
        {
            dexterity = d;

        }

        public static void setInteligence(int i)
        {
            inteligence = i;

        }

        public static void attack(int damage)
        {
            setHealth( curhealth - damage);
            if (dmg != null)
                dmg();


        }

        public static bool save(int i)
        {

            string save = "";
            string path = Environment.CurrentDirectory + "\\saves\\save" + i + "\\player.txt";

            if (name == null || name == "")
            {
                save += "SAVE GAME" + Environment.NewLine;
            }
            else
                save += getName() + Environment.NewLine;

            save += getBaseStrength() + " " + getBaseInteligence() + " " + getBaseDexterity() + Environment.NewLine;
            save += getMoney() + Environment.NewLine;
            save += getHealth() + " " + getMaxHealth() + Environment.NewLine;
            save += getInventory().Count + Environment.NewLine;

            foreach (Iitem it in getInventory())
                save += it.getId() + Environment.NewLine;

            save += getSet().Count + Environment.NewLine;
            foreach (KeyValuePair<string, equipment> eq in getSet())
                save += eq.Value.getId() + Environment.NewLine;

            foreach (int flag in getPlayerFlags())
            {
                save += flag + " ";
            }
            save = save.Remove(save.Length - 1);

            try
            {
                System.IO.TextWriter textOut =
                new System.IO.StreamWriter(path);

                textOut.Write(save);
                textOut.Close();
                return true;
            }
            catch
            {
                console.log(3, "FAILED TO SAVE GAME AT SAVE FILE #" + i + ", PLAYER SAVE FAIL");
                return false;
            }

        }

        public static bool load(int i)
        {
            try
            {
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\saves\\save" + i + "\\player.txt"))
                {
                    setName(sr.ReadLine());
                    string[] stats = sr.ReadLine().Split(' ');
                    setStrength(int.Parse(stats[0]));
                    setInteligence(int.Parse(stats[1]));
                    setDexterity(int.Parse(stats[2]));
                    setMoney(int.Parse(sr.ReadLine()));
                    string[] hps = sr.ReadLine().Split(' ');
                    setMaxHealth(int.Parse(hps[1]));
                    setHealth(int.Parse(hps[0]));
                    int loopmax = int.Parse(sr.ReadLine());
                    inventory = new List<Iitem>();
                    set = new Dictionary<string, equipment>();

                    for (int j = 0; j < loopmax; j++)
                    {
                        Iitem it;
                        string id = sr.ReadLine();
                        if (item.getItem(id) != null)
                            giveItem(item.getItem(id));
                        else if (usable.getUsable(id) != null)
                            giveItem(usable.getUsable(id));
                        else if (equipment.getEquipment(id) != null)
                            giveItem(equipment.getEquipment(id));
                    }

                    loopmax = int.Parse(sr.ReadLine());

                    for (int j = 0; j < loopmax; j++)
                    {
                        string id = sr.ReadLine();
                        equip(equipment.getEquipment(id));
                    }

                    string[] flags = sr.ReadLine().Split(' ');

                    for (int j = 0; j < game.savegameFlags; j++)
                        getPlayerFlags()[j] = int.Parse(flags[j]);

                    game.running = true;
                    return true;
                }
            }
            catch (Exception e)
            {
                console.log(3, "FAILED TO LOAD SAVEFILE #" + i + "!");
                return false;
            }

        }

        public static void heal(int amount)
        {
            setHealth(curhealth + amount);
            gui.throwError("You healed yourself for " + amount + " health points!");

        }

    }
}
