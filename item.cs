using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{
    class item: Iitem
    {
        private string id;
        private string name;
        private string description;
        private int value;

        private static List<item> items = new List<item>();

        protected item(string id, string name, string desc, int val)
        {
            id = id.ToUpper();
            this.id = id;
            this.name = name;
            description = desc;

            value = val;

            console.log(1, "Added item of id: " + id);

        }

        public string getId()
        {
            return id;
        }

        public string getName()
        {
            return name;
        }

        public string getDescription()
        {
            return description;
        }

        public string itemType()
        {
            return "ITEM";
        }

        public int getValue()
        {
            return value;
        }

        public static item addItem(string id, string name, string desc, int val)
        {
            id = id.ToUpper();

            if (getItem(id) != null)
            {
                console.log(2, "Failed to add item of id: " + id + ", id already exists");
                return null;
            }
            item u = new item(id, name, desc, val);
            items.Add(u);
            return u;
        }

        public static item getItem(string id)
        {
            id = id.ToUpper();
            foreach (item eq in items)
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
