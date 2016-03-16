using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{
    public class equipment : Iitem
    {

        private int strength;
        private int dexterity;
        private int inteligence;
        private string type;

        private string id;
        private string name;
        private string description;
        private int value;


        private static List<equipment> equipments = new List<equipment>();

        protected equipment(string id, string name, string desc, int s, int d, int i, int val, string type)
        {
            id = id.ToUpper();
            this.id = id;
            this.name = name;
            description = desc;

            strength = s;
            dexterity = d;
            inteligence = i;

            value = val;

            this.type = type;

            console.log(1, "Added " + type + " equipment piece of id: " + id);

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

        public int getValue()
        {
            return value;
        }

        public string itemType()
        {
            return "EQUIPMENT";
        }


        public string Type()
        {
            return type;
        }


        public int getStrength()
        {
            return strength;
        }

        public int getInteligence()
        {
            return inteligence;
        }

        public int getDexterity()
        {
            return dexterity;
        }





        public static equipment addEquipment(string id, string name, string desc, int s, int d, int i, int val, string type)
        {
            id = id.ToUpper();

            if (getEquipment(id) != null)
            {
                console.log(2, "Failed to add " + type + " equipment piece of id: " + id + ", id already exists");
                return null;
            }
            equipment a = new equipment(id, name, desc, s, d, i, val, type);
            equipments.Add(a);
            return a;
        }




        public static equipment getEquipment(string id)
        {
            id = id.ToUpper();
            foreach (equipment eq in equipments)
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