using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{
    class actor
    {

        private string name;
        private bool visible = false;
        private bool npc = false;
        private bool store = false;
        private List<action> actions = new List<action>();

        public actor(string name, bool visible, bool npc, bool store)
        {
            name = name.ToUpper();
            this.name = name;
            this.visible = visible;
            this.npc = npc;

            if (npc)
                addAction("TALK", false, new List<string>(new string[] { name }), null);

            if (store)
                addAction("SHOP", false, new List<string>(new string[] { name }), null);

        }

        public void setVisible()
        {
            visible = true;
        }

        public void setInvisible()
        {
            visible = false;
        }

        public bool getVisible()
        {
            return visible;
        }

        public bool getNpc()
        {
            return npc;
        }

        public bool getStore()
        {
            return store;
        }

        public string getName()
        {
            return name;
        }

        public void addDialog(string initial, List<action> options, bool overrideMap)
        {
            action.ActionChoice dialog = delegate (string par)
            {
                
                map curMap = game.getMap();
                game.overwroteMapSign = curMap.getName();

                map dial = new map("dialog", "", "", "", "", "Talking to " + getName(), initial);
                actor chat = dial.addActor("chat", true, false, false);

                chat.addAction("QUIT", true, null, delegate (string param)
                {
                    if (overrideMap)
                        game.changeMap(game.overwroteMap);
                    else
                        game.changeMap(curMap);

                    game.overwroteMapSign = "";
                    game.overwroteMap = null;
                });

                foreach (action a in options)
                {
                    chat.addAction(a.getKeyword(), true, a.getParamList(), a.getAct());
                }

                game.changeMap(dial);

            };


            findAction("TALK").changeAct(dialog);

        }

        public void addStore(string initial, List<Iitem> items)
        {
            action.ActionChoice dialog = delegate (string par)
            {

                map curMap = game.getMap();

                map dial = new map("store", "", "", "", "", "SHOPPING " + getName(), initial);
                actor chat = dial.addActor("chat", true, false, false);

                chat.addAction("BUY", false, new List<string>(), delegate (string param)
                {
                    gui.throwError("Do you want to buy " + param + " for " + (int) Math.Round(dial.findItem(param).getValue()*game.priceMultiplier) + " gold? Enter 1 to accept." );
                    if (CAFe.readNumber() == 1)
                    {
                        if (player.getMoney() >= dial.findItem(param).getValue() * game.priceMultiplier)
                        {
                            player.giveItem(dial.findItem(param));
                            player.setMoney(player.getMoney() - (int)Math.Round(dial.findItem(param).getValue() * game.priceMultiplier));
                        }
                        else
                            gui.throwError("You do not have enough money!");
                    }
                });

                chat.addAction("SELL", false, new List<string>(), delegate (string param)
                {
                    gui.throwError("Do you want to sell " + param + " for " + player.findItem(param).getValue() + " gold? Enter 1 to accept.");
                    if (CAFe.readNumber() == 1)
                    {
                        chat.findAction("SELL").getParamList().Remove(param);
                        player.setMoney(player.getMoney() + player.findItem(param).getValue());
                        player.removeItem(player.findItem(param));

                        
                    }
                });

                chat.addAction("QUIT", true, new List<string>(), delegate (string param)
                {
                    game.changeMap(curMap);

                });

                foreach (Iitem i in items)
                {
                    dial.addItem(i);
                    chat.findAction("BUY").addParam(i.getName());
                }

                foreach (Iitem i in player.getInventory())
                {
                    chat.findAction("SELL").addParam(i.getName());
                }

                game.changeMap(dial);

            };

            findAction("SHOP").changeAct(dialog);
        }


        public List<action> getActions()
        {
            return actions;
        }

        public action addAction(string keyword, bool single, List<string> param, action.ActionChoice act)
        {
            keyword = keyword.ToUpper();
            if (findAction(keyword) != null)
            {
                console.log(2, "Failed to add actor action of keyword: " + keyword + " to actor " + name + " , keyword already used");
                return null;
            }
            action add = new action(keyword, single, param, act);
            actions.Add(add);
            return add;

        }

        public action findAction(string keyword)
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

        public void removeAction(string keyword)
        {
            keyword = keyword.ToUpper();

            if (findAction(keyword) == null)
            {
                return;
            }

            actions.Remove(findAction(keyword));

        }

    }
}
