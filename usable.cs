using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{
    public class usable : Iitem
    {
        private string id;
        private string name;
        private int value;
        private string description;
        private UsableAct act = null;
        private static List<usable> usables = new List<usable>();

        public delegate void UsableAct();

        protected usable(string id, string name, string desc, int val, UsableAct act)
        {
            id = id.ToUpper();
            this.id = id;
            this.name = name;
            description = desc;

            value = val;
            this.act = act;

            console.log(1, "Added usable item of id: " + id);

        }



        public string getId()
        {
            return id;
        }

        public void use()
        {
            if (act != null)
                act();

        }

        public string getName()
        {
            return name;
        }

        public int getValue()
        {
            return value;
        }

        public string getDescription()
        {
            return description;
        }

        public string itemType()
        {
            return "USABLE";
        }


        public static usable addUsable(string id, string name, string desc, int val, UsableAct act)
        {
            id = id.ToUpper();

            if (getUsable(id) != null)
            {
                console.log(2, "Failed to add usable of id: " + id + ", id already exists");
                return null;
            }
            usable u = new usable(id, name, desc, val, act);
            usables.Add(u);
            return u;
        }

        public static usable getUsable(string id)
        {
            id = id.ToUpper();
            foreach (usable eq in usables)
            {
                if (eq.getId() == id)
                {
                    return eq;
                }
            }

            return null;
        }

    }
}
