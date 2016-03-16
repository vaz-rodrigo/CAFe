using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{

    class game
    {

        private static List<map> maps = new List<map>();
        private static List<action> actions = new List<action>();


        public static int savegameFlags = 10;

        private static map spawn = null;
        private static map currentMap = null;

        public static string title = "Defenders of Jamiroqual";
        public static string styledTitle = "";

        public static List<string> story = new List<string>();
        public static List<string> ending = new List<string>();
        public static List<string> death = new List<string>();

        public static string author = "Rodrigo Vaz";
        public static string version = "1.0";

        public static bool skipCharacter = false;
        public static bool skipStats = false;
        public static bool running = false;
        public static bool dialogOptions = true;

        public static int statPoints = 30;
        public static int startMoney = 10;
        public static int starthp = 100;

        public static float dodgeMultiplier = 0.1f;
        public static float barrierMultiplier = 0.2f;
        public static float strengthMultiplier = 0.3f;
        public static int attackModifier = 5;
        public static float priceMultiplier = 2.0f;
        public static float monsterArmorMultiplier = 0.1f;


        public static map overwroteMap = null;
        public static string overwroteMapSign = "";

        public static string getTitle()
        {
            if (styledTitle == "")
            {
                console.log(1, "Using plain title because styledTitle was not supplied.");
                return title;
            }
            return styledTitle;
        }



        public static void addStory(string text)
        {
            story.Add(text);
        }

        public static void addEnding(string text)
        {
            ending.Add(text);
        }

        public static void addDeath(string text)
        {
            death.Add(text);
        }


        public static action addAction(string keyword, bool single, List<string> param, action.ActionChoice act)
        {
            keyword = keyword.ToUpper();
            if (findAction(keyword) != null)
            {
                console.log(2, "Failed to add default action of keyword: " + keyword + ", keyword already used");
                return null;
            }
            action add = new action(keyword, single, param, act);
            actions.Add(add);
            return add;

        }

        public static action findAction(string keyword)
        {
            keyword = keyword.ToUpper();

            foreach (action a in actions)
            {
                if (a.getKeyword() == keyword)
                {
                    return a;
                }
            }

            return null;
        }

        public static void removeAction(string keyword)
        {
            keyword = keyword.ToUpper();

            if(keyword == "TALK" || keyword == "MOVE")
            {
                console.log(2, "Tried to remove protected keyword " + keyword + ".");
                return;
            }
            if (findAction(keyword) == null)
            {
                return;
            }

            actions.Remove(findAction(keyword));

        }



        public static map addMap(string id, string n, string s, string w, string e, string name, string text)
        {
            id = id.ToUpper();
            if (findMap(id) != null || id == "DIALOG")
            {
                console.log(2, "Failed to add map of id: " +id + ", id already exists");
                return null;
            }
            map add = new map(id, n, s, w, e, name, text);
            maps.Add(add);
            return add;

        }

        public static map findMap(string id)
        {
            id = id.ToUpper();
            foreach (map m in maps)
            {
                if (m.getId() == id)
                {
                    return m;
                }
            }

            return null;
        }

        public static void setSpawn(map s)
        {
            spawn = s;
        }

        public static map getSpawn()
        {

            return spawn;
        }

        public static map getMap()
        {
            return currentMap;
        }

        public static void changeMap(map destiny)
        {
            if (currentMap == destiny)
            {
                console.log(2, "Tried changing map to the current map!");
                return;
            }
            if (destiny == null)
            {
                console.log(3, "Tried to load a map that does not exists");
                CAFe.crash("THE GAME CRASHED BECAUSE IT FAILED TO LOAD A MAP!");
                return;
            }

            if (currentMap != null && !currentMap.getVisited())
                currentMap.setVisited();

            currentMap = destiny;
            action TAKE = findAction("TAKE");

            foreach(Iitem i in currentMap.getItems())
            {
                TAKE.addParam(i.getName());
            }
        }

        public static bool hasSave(int i)
        {
            return (File.Exists(Environment.CurrentDirectory + "\\saves\\save" + i + "\\player.txt"));
        }

        public static string getSaveName(int i)
        {
            try
            {
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\saves\\save" + i + "\\player.txt"))
                {
                    return sr.ReadLine();
                }
            }
            catch (Exception e)
            {
                console.log(3,"FAILED TO LOAD SAVEFILE #" + i + "!");
                return null;
            }

        }



        private static bool saveMaps(int i)
        {
            string path = Environment.CurrentDirectory + "\\saves\\save" + i + "\\maps.txt";
            string save = "";

            save += maps.Count + Environment.NewLine;
            foreach (map m in maps)
            {
                save += m.getId() + Environment.NewLine + m.getNorth() + Environment.NewLine + m.getSouth() + Environment.NewLine;
                save += m.getWest() + Environment.NewLine + m.getEast() + Environment.NewLine;
                save += m.getVisited().ToString() + Environment.NewLine;
                save += m.getItems().Count + Environment.NewLine;

                foreach(Iitem it in m.getItems())
                    save += it.getId() + Environment.NewLine;
                if (m.findActor("intmob").findAction("FIGHT").getVisible() == true)
                    save += "True";
                else
                    save += "False";

            }
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
                console.log(3, "FAILED TO SAVE GAME AT SAVE FILE #" + i + ", MAP SAVE FAIL");
                return false;
            }

        }

        private static bool loadMaps(int i)
        {
            try
            {
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\saves\\save" + i + "\\maps.txt"))
                {

                    int amount = int.Parse(sr.ReadLine());

                    for(int j = 0; j < amount; j++)
                    {
                        map m = findMap(sr.ReadLine());

                        m.setNorth(sr.ReadLine());
                        m.setSouth(sr.ReadLine());
                        m.setWest(sr.ReadLine());
                        m.setEast(sr.ReadLine());

                        string dec = sr.ReadLine().ToUpper();

                        if (dec == "FALSE")
                            m.setVisited(false);
                        else
                            m.setVisited();

                        int loopmax = int.Parse(sr.ReadLine());
                        m.getItems().Clear();

                        for (int k = 0; k < loopmax; k++)
                        {
                            Iitem it;
                            string id = sr.ReadLine();
                            if (item.getItem(id) != null)
                                m.addItem(item.getItem(id));
                            else if (usable.getUsable(id) != null)
                                m.addItem(usable.getUsable(id));
                            else if (equipment.getEquipment(id) != null)
                                m.addItem(equipment.getEquipment(id));
                        }

                        dec = sr.ReadLine().ToUpper();

                        if (dec == "FALSE")
                            m.findActor("intmob").findAction("FIGHT").setVisible(false);
                        else
                            m.findActor("intmob").findAction("FIGHT").setVisible(true);

                    }
                    
                    return true;
                }
            }
            catch (Exception e)
            {
                console.log(3, "FAILED TO LOAD SAVEFILE #" + i + "!");
                return false;
            }

        }

        private static bool saveInfo(int i)
        {
            string path = Environment.CurrentDirectory + "\\saves\\save" + i + "\\game.txt";
            string save = CAFe.getVersion() + Environment.NewLine + getSpawn().getId() + Environment.NewLine + currentMap.getId();


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
                console.log(3, "FAILED TO SAVE GAME AT SAVE FILE #" + i + ", GAME SAVE FAIL");
                return false;
            }

        }

        private static bool loadInfo(int i)
        {
            try
            {
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\saves\\save" + i + "\\game.txt"))
                {
                    if (!sr.ReadLine().Equals(CAFe.getVersion()))
                        console.log(2, "Loading save file #" + i + " of different CAFe version!");

                    currentMap = null;
                    spawn = findMap(sr.ReadLine());
                    changeMap(findMap(sr.ReadLine()));

                    return true;
                }
            }
            catch (Exception e)
            {
                console.log(3, "FAILED TO LOAD SAVEFILE #" + i + "!");
                return false;
            }
        }

        public static bool save(int i)
        {
            System.IO.Directory.CreateDirectory("saves");
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "\\saves\\save" + i);

            return player.save(i) && saveMaps(i) && saveInfo(i);
        }

        public static bool load(int i )
        {
            running = true;
            return player.load(i) && loadMaps(i) && loadInfo(i);
        }


    }
}
