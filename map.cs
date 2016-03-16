using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{
    class map
    {

        private string id;
        private string north = "";
        private string south = "";
        private string west = "";
        private string east = "";
        public bool visited = false;
        private bool taunted = false;

        private int round = 0;

        MapBuffer buff = null;
        MapCall call = null;

        public delegate void MapBuffer();
        public delegate void MapCall();

        private string name;
        private string text;

        private List<Iitem> items = new List<Iitem>();

        private monster mob;

        private List<actor> actors = new List<actor>(); 

        public map(string id, string n, string s, string w, string e, string name, string text)
        {
            id = id.ToUpper();
            n = n.ToUpper();
            s = s.ToUpper();
            w = w.ToUpper();
            e = e.ToUpper();

            addActor("intmob", false, false, false);

            this.id = id;
            north = n;
            south = s;
            west = w;
            east = e;
            this.name = name;
            this.text = text;
        }

        public string getId()
        {
            return id;
        }

        public void setNorth(string d)
        {
            north = d;
        }

        public void addRound()
        {
            round++;
        }

        public int getRound()
        {
            return round;
        }

        public void setSouth(string d)
        {
            south = d;
        }

        public void setWest(string d)
        {
            west = d;
        }

        public void setEast(string d)
        {
            east = d;
        }

        public string getSouth()
        {
            return south;
        }

        public string getNorth()
        {
            return north;
        }

        public string getWest()
        {
            return west;
        }

        public string getEast()
        {
            return east;
        }

        public bool getVisited()
        {
            return visited;
        }

        public void setVisited(bool vis = true)
        {
            visited = vis;
        }

        public List<actor> getActors()
        {
            return actors;
        }

        public actor addActor( string name, bool visible, bool npc, bool store)
        {
            name = name.ToUpper();
            if (findActor(name) != null)
            {
                console.log(2, "Failed to add actor with name: " + name + "to map " + getId() + ", name already used");
                return null;
            }
            actor add = new actor(name, visible, npc, store    );
            actors.Add(add);
            return add;
        }

        public bool getTaunt()
        {
            return taunted;
        }

        public void setTaunt(bool state)
        {
            taunted = state;
        }

        public void addMonster(monster m, List<action> options, bool taunted = false)
        {

            actor mob = findActor("intmob");
            this.taunted = taunted;
            mob.setVisible();
            mob.addAction("FIGHT", true, null, null);

            action.ActionChoice fight = delegate (string par)
            {


                map fite = new map("fight", game.getMap().getId(), "", "", "", "", "");
                fite.setMonster(m);
                fite.getMonster().reset();
                actor chat = fite.addActor("chat", true, false, false);

                foreach (action a in options)
                {
                    chat.addAction(a.getKeyword(), true, a.getParamList(), a.getAct());
                }

                game.changeMap(fite);

            };


            mob.findAction("FIGHT").changeAct(fight);

        }

        public monster getMonster()
        {
            return mob;
        }

        private void setMonster(monster m)
        {
            mob = m;
        }

        public void resetMonster()
        {
            mob = null;
            actor mobs = findActor("intmob");
            mobs.findAction("FIGHT").setVisible(false);
        }

        public actor findActor(string name  )
        {
            name = name.ToUpper();

            foreach (actor a in actors)
            {
                if (a.getName() == name)
                {
                    return a;
                }
            }

            return null;
        }

        public List<Iitem> getItems()
        {
            return items;

        }

        public void removeItem(string name)
        {
            Iitem i = findItem(name);
            if (i == null)
                return;
            items.Remove(i);
        }

        public Iitem findItem(string name)
        {
            foreach (Iitem i in items)
            {
                if (i.getName().ToUpper() == name.ToUpper())
                {
                    return i;
                }
            }
            return null;
        }

        public void OnChange()
        {

            if (call != null)
                call();
            if (buff != null)
                buff();

        }

        public void setDescription(string description)
        {
            text = description;
        }

        public string getName()
        {
            return name;
        }

        public void addItem(Iitem i)
        {
            items.Add(i);
        }

        public void changeBuffer(MapBuffer buffer)
        {
            buff = buffer;
        }

        public void changeCall(MapCall inCall)
        {
            call = inCall;
        }

        public string getAvaibleDirections()
        {
            string ret = "";

            if (north != "" && game.findMap(north) != null)
                ret = "North";

            if (south != "" && game.findMap(south) != null)
            {
                if (ret != "")
                    ret += ", ";
                ret += "South";
            }

            if (west != "" && game.findMap(west) != null)
            {
                if (ret != "")
                    ret += ", ";
                ret += "West";
            }

            if (east != "" && game.findMap(east) != null)
            {
                if (ret != "")
                    ret += ", ";
                ret += "East";
            }

            return ret;

        }

        public override string ToString()
        {
            return name.ToUpper() + "\n\n" + text;
        }

        
    }
}
