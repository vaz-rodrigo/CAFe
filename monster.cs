using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{
    class monster
    {
        private string name;

        private MonsterBehavior act = null;
        private MonsterBehavior deathAct = null;
        private MonsterBehavior damageAct = null;

        public delegate void MonsterBehavior();
        Random rand = new Random();

        private List<Iitem> loot = new List<Iitem>();
        private int moneyLoot = 0;

        private List<string> messageAttack = new List<string>();
        private List<string> messageMagic = new List<string>();




        private int health = 0;
        private int curhealth = 0;
        private int armor = 0;
        private bool next = false;
        private int attackModifier = 0;
        private int healModifier = 0;
        private int magicModifier = 0;



        private bool canHeal = false;
        private bool healed = false;
        private int healAmount = 0;
        private int delayHeal = 0;
        private int minHpHeal = 0;

        private bool debuffActive = false;
        private int debuffCounter = 0;
        private int debuffDuration = 0;
        private int debufAmount = 0;
        private int healCounter = 0;
        private bool resetDebuff = false;



        private bool canMagic = false;
        private bool castedMagic = false;
        private int magicDamage = 0;
        private int delayMagic = 0;
        private int magicCounter = 0;


        private bool canAttack = true;
        private int attackDamage = 0;


        public monster(string name, List<Iitem> loot, int moneyLoot, int hp, int arm)
        {
            this.name = name;
            this.loot = loot;
            this.moneyLoot = moneyLoot;
            health = hp;
            armor = arm;
            curhealth = hp;

        }



        public void reset()
        {
            curhealth = health;
            next = false;
            healed = false;
            debuffActive = false;
            debuffCounter = 0;
            healCounter = 0;
            castedMagic = false;
            magicCounter = 0;


        }

        public bool getNext()
        {
            return next;
        }

        public void setNext()
        {
            next = true;
        } 

        public void addMessage(bool attack, string message)
        {
            if(attack)
            {
                messageAttack.Add(message);
                return;
            }

            messageMagic.Add(message);

        }

        public List<Iitem> getLoot()
        {
            return loot;
        }

        public void setDamageAct(MonsterBehavior act)
        {
            damageAct = act;
        }

        public int getValue()
        {
            return moneyLoot;
        }

        public string getName()
        {
            return name;
        }


        public void setBehavior(bool death, MonsterBehavior act)
        {
            if (death)
                deathAct = act;
            else
                this.act = act;
        }

        public int getHealth()
        {
            return curhealth;
        }

        public int getMaxHealth()
        {
            return health;
        }

        public int getArmor()
        {
            return armor;
        }

        public void setHeal(bool canHeal, int amount, int delay, bool resetDebuff, int debufAmount, int healUnder, int debuffDuration, int hm)
        {
            this.canHeal = canHeal;
            healAmount = amount;
            delayHeal = delay;
            this.debuffDuration = debuffDuration;
            this.resetDebuff = resetDebuff;
            healModifier = hm;

            if (debufAmount < 0)
                this.debufAmount = 0;
            else if (debufAmount > 100)
                this.debufAmount = 100;
            else
                this.debufAmount = debufAmount;

            if (healUnder < 0)
                minHpHeal = 0;
            else if (healUnder > 100)
                minHpHeal = health;
            else
                minHpHeal = (health*healUnder/100);

        }

        public void setMagic(bool canMagic, int damage, int delay, int mm)
        {
            this.canMagic = canMagic;
            magicDamage = damage;
            delayMagic = delay;
            magicModifier = mm;

        }

        public void setAttack(bool canAttack, int damage, int am)
        {
            this.canAttack = canAttack;
            attackDamage = damage;
            attackModifier = am;
        }



        public void perform()
        {
            next = false;
            if (act == null)
            {
                if (healed)
                    healCounter++;
                if (healCounter > delayHeal)
                    healed = false;

                if (debuffActive)
                    debuffCounter++;
                if (debuffCounter > debuffDuration)
                    debuffActive = false;

                if (castedMagic)
                    magicCounter++;
                if (magicCounter > delayMagic)
                    castedMagic = false;


                int choice = rand.Next(1, 11);

                if(canHeal && (curhealth <= minHpHeal) && !healed)
                {
                    heal();
                    return;
                }

                switch(choice)
                {
                    case 1:
                        if (canMagic && !castedMagic)
                            magic();
                        else
                            attack();
                        break;
                    case 3:
                        if (canMagic && !castedMagic)
                            magic();
                        else
                            attack();
                        break;
                    case 5:
                        if (canHeal && !healed && (curhealth <= minHpHeal))
                            heal();
                        else if (canMagic && !castedMagic)
                            magic();
                        else
                            attack();
                        break;
                    default:
                        attack();
                        break;
                }

                return;

            }
            else
            {
                act();
            }

        }

        public void death()
        {
            if(deathAct == null)
            {

            }
            else
            {
                deathAct();
            }

        }

        public void heal(int amount = 0)
        {
            if (amount == 0)
            {
                amount = rand.Next(healAmount - healModifier, healAmount + healModifier + 1);

                if (debuffActive)
                    amount = amount * debufAmount / 100;

                if (resetDebuff)
                    debuffCounter = 0;

                healed = true;
                debuffActive = true;

                curhealth += amount;

                if (curhealth > health)
                    curhealth = health;



            }
            else
            {
                if (amount > health)
                    amount = health;

                curhealth = amount;
            }

            gui.throwError("The enemy healed " + amount + " health points!");
        }

        public void attack(int amount = 0)
        {
            if(amount == 0)
            {
                amount = rand.Next(attackDamage - attackModifier, attackDamage + attackModifier + 1);

                amount = amount - ((int) Math.Round((player.getDexterity()*game.dodgeMultiplier))) ;

                if (amount < 0)
                    amount = 0;




            }

            if (messageAttack.Count == 0)
                messageAttack.Add("attacked furiously for");

            player.attack(amount);

            gui.throwError(name + " " + messageAttack[rand.Next( 0, messageAttack.Count )] + " " + amount + " damage!");

        } 

        public void magic(int amount = 0)
        {
            if (amount == 0)
            {
                amount = rand.Next(magicDamage - magicModifier, magicDamage + magicModifier + 1);

                amount = amount - ((int)Math.Round((player.getInteligence() * game.barrierMultiplier)));

                if (amount < 0)
                    amount = 0;
            }

            if (messageMagic.Count == 0)
                messageMagic.Add("unleashed a powerful spell for");

            player.attack(amount);

            gui.throwError(name + " " + messageMagic[rand.Next(0, messageMagic.Count)] + " " + amount + " damage!");

        }

        public void takeDamage(int strength, string message = "", int amount = 0)
        {

            if (amount == 0)
            {
                int attack = ((int)Math.Round(strength * game.strengthMultiplier));

                amount = rand.Next(attack - game.attackModifier, attack + game.attackModifier + 1);

                amount = amount - ((int)Math.Round((armor * game.monsterArmorMultiplier)));

                if (amount < 0)
                    amount = 0;

            }

            curhealth = curhealth - amount;
            if (curhealth < 0)
                curhealth = 0;

            if (message == "" || message == null)
                message = "You attacked the monster for " + amount + " health points!";

            gui.throwError(message);

            if (damageAct != null)
                damageAct();
        }

    }
}
