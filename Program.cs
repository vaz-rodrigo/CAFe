using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{
    class Program
    {
        static void Main(string[] args)
        {

            string b = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Phasellus ac tempor purus. Vivamus consectetur ultricies lobortis.
Pellentesque mattis at lacus eu pharetra. Nunc vel magna vehicula,
condimentum ante eu, ullamcorper erat. Nam eget pulvinar odio.
Aliquam sagittis rutrum leo. Nam a efficitur nunc. Ut vestibulum
consequat mi id aliquam. Nullam accumsan ornare hendrerit. Nunc
in ipsum nec massa vulputate vestibulum. Aliquam congue at dolor
ut scelerisque.";

            string c = @"Maecenas ut nisl eget velit feugiat sollicitudin.
Quisque quis diam ac ipsum tempus mattis. Vivamus at sem erat.
Nulla semper orci ac condimentum blandit. Aliquam placerat, est
vitae pretium sagittis, arcu sem posuere turpis, eu elementum dui
dui et erat. Aliquam finibus, justo nec porttitor porta, leo mauris
faucibus nisi, non condimentum justo magna luctus felis. Curabitur
feugiat arcu sed aliquet luctus. Suspendisse porttitor elementum
lectus sed imperdiet. Phasellus at tempor nulla. In maximus tellus
tortor, id venenatis nisi volutpat vel.";

            string d = @"Du isst nur weil die gabel sich bewegt
Nur weil die gabel sich bewegt
Weil du existierst aber nicht lebst
Du isst nur weil die gabel sich bewegt
Ob man nur existiert oder auch lebt
Interessiert uns nicht";
            string e = @"
Was du unter vorstellungskraft verstehst
Ist gerade das was vor dir steht
Nur was wir bei tageslicht sehen
Und ob man reflektiert oder nur schläft
Interessiert uns nicht";
            string f = @"You say nostalgia is negation
It can't go on like that
It will all end
Contentment to be in nature
And kindness is still so hard to find
Sadness is rebellion
And self hate only respect";

            action MOVE = game.addAction("MOVE", false, new List<string>(), delegate (string par) {
                map dest;
                if (game.getMap().findActor("intmob").getVisible() && game.getMap().getTaunt())
                {
                    gui.throwError("You must FIGHT the monster in this area!");
                    return;
                }

                if (par == "NORTH")
                {
                    dest = game.findMap(game.getMap().getNorth());
                    if(dest == null)
                    {
                        gui.throwError("You can't go " + par);
                        return;
                    }
                    game.changeMap(dest);
                    return;
                }
                else if (par == "SOUTH")
                {
                    dest = game.findMap(game.getMap().getSouth());
                    if (dest == null)
                    {
                        gui.throwError("You can't go " + par);
                        return;
                    }
                    game.changeMap(dest);
                    return;
                }
                else if (par == "WEST")
                {
                    dest = game.findMap(game.getMap().getWest());
                    if (dest == null)
                    {
                        gui.throwError("You can't go " + par);
                        return;
                    }
                    game.changeMap(dest);
                    return;
                }
                else if (par == "EAST")
                {
                    dest = game.findMap(game.getMap().getEast());
                    if (dest == null)
                    {
                        gui.throwError("You can't go " + par);
                        return;
                    }
                    game.changeMap(dest);
                    return;
                }
            });
            MOVE.addParam("NORTH");
            MOVE.addParam("SOUTH");
            MOVE.addParam("WEST");
            MOVE.addParam("EAST");

            action TAKE = game.addAction("TAKE", false, new List<string>(), delegate (string par)
            {

                Iitem i = game.getMap().findItem(par.ToUpper());
                player.giveItem(i);
                game.getMap().getItems().Remove(i);

                gui.throwError("You took the " + par + ".");

            });

            action QUIT = game.addAction("QUIT", true, null, delegate (string par)
            {

                gui.throwError("Do you really want to leave? Unsaved progress will be lost!, Enter 1 to confirm.");

                if (CAFe.readNumber() == 1)
                    game.running = false;


            });

            action INVENTORY = game.addAction("INVENTORY", true, null, delegate (string par)
            {

                gui.clearScreen();
                gui.clearOption();

                List<Iitem> inv = player.getInventory();
                Dictionary<string, equipment> set = player.getSet();
                string itext;

                if (inv.Count == 0)
                    itext = "\n YOUR INVENTORY IS EMPTY!";
                else
                {
                    itext = "\n YOU CURRENTLY HOLD\n";

                    foreach(Iitem i in inv)
                    {
                        itext += "\n " + i.getName().ToUpper() + " : " + i.itemType().ToUpper();
                    }
                }

                string etext;

                if (set.Count == 0)
                    etext = "\nYOU DO NOT HAVE ANY EQUIPMENT! ";
                else
                {
                    etext = "\nYOU CURRENTLY HAVE EQUIPPED \n";
                    foreach (KeyValuePair<string, equipment> E in set)
                    {
                        etext += "\n" + E.Value.getName() + " : " + E.Value.Type().ToUpper() + " ";
                    }
                }

                guiElement eT = gui.text(0, 0, etext);
                eT.rightTop();

                guiElement invBox = gui.textBox(0, 0, "INVENTORY");
                invBox.alignHorizontal();

                gui.queue(invBox);
                gui.queue(gui.text(0, 0, itext));
                gui.queue(eT);

                gui.load();
                gui.draw();

                gui.throwError("Press any key to continue.");

            });

            action STATUS = game.addAction("STATUS", true, null, delegate (string par)
            {

                gui.clearScreen();
                gui.clearOption();

                string etText = "   NAME   \n\n" + player.getName() + "\n\n";
                etText += "STRENGTH: " + player.getBaseStrength() + " + ( " + (player.getStrength() - player.getBaseStrength()) + " )\n";
                etText += "INTELIGENCE: " + player.getBaseInteligence() + " + ( " + (player.getInteligence() - player.getBaseInteligence()) + " )\n";
                etText += "DEXTERITY: " + player.getBaseDexterity() + " + ( " + (player.getDexterity() - player.getBaseDexterity()) + " )\n";

                guiElement invBox = gui.textBox(0, 0, "STATUS");
                invBox.alignHorizontal();

                guiElement hpBox = gui.textBox(0, 0, "   HEALTH   \n\n" + player.getHealth() + " / " + player.getMaxHealth());
                hpBox.rightTop();

                guiElement moneyBox = gui.textBox(0, 0, "   MONEY   \n\n" + player.getMoney() + " GOLD" );
                moneyBox.rightBottom();

                gui.queue(gui.textBox(1, 1, etText));
                gui.queue(invBox);
                gui.queue(hpBox);
                gui.queue(moneyBox);

                gui.load();
                gui.draw();

                gui.throwError("Press any key to continue.");

            });

            action EQUIP = game.addAction("EQUIP", false, new List<string>(), delegate (string par)
            {
                Iitem i = player.findItem(par);
                player.equip( (equipment) i );
                player.getInventory().Remove(i);

                gui.throwError("You equipped the " + par + ".");

            });

            action UNEQUIP = game.addAction("UNEQUIP", false, new List<string>(), delegate (string par)
            {
                player.removeEquipment(player.getEquipment(par));
                gui.throwError("You unequipped the " + par + ".");


            });

            action USE = game.addAction("USE", false, new List<string>(), delegate (string par)
            {
                usable u = (usable) player.findItem(par.ToUpper());
                u.use();
                gui.throwError("You used the " + par + ".");
                player.removeItem(u);


            });

            action SAVE = game.addAction("SAVE", true, new List<string>(), delegate (string par)
            {

                gui.clearScreen();
                gui.clearOption();

                for (int i = 1; i < 4; i++)
                {
                    string savetext = "   SAVE #" + i +"   \n\n";
                    string savename = null;
                    if (game.hasSave(i))
                    {
                        savename = game.getSaveName(i);
                        if (savename != null)
                            savetext += game.getSaveName(i);
                    }
                    else
                        savetext += "EMPTY";

                    guiElement savebox = gui.textBox(0, 0, savetext);
                    if (i == 1)
                        savebox.alignVertical();
                    if (i == 2)
                        savebox.center();
                    if (i == 3)
                    {
                        savebox.rightTop();
                        savebox.alignVertical();
                    }

                    gui.queue(savebox);
                }

                gui.load();

                gui.draw();

                gui.throwError("Please enter the number of the save file you wish to use!");
                gui.setCursor(false);

                int which = CAFe.readNumber();

                if(which < 1 || which > 3)
                    gui.throwError("That file does not exists!");
                else
                {
                    if (game.hasSave(which))
                    {
                        gui.throwError("Do you wish to overwrite the save file? Enter 1 to overwrite.");

                        if (CAFe.readNumber() == 1)
                        {
                            game.save(which);
                            gui.throwError("Saved game at file #" + which);
                        }
                        else
                            gui.throwError("Saving was canceled.");
                    }
                    else
                    {
                        game.save(which);
                        gui.throwError("Saved game at file #" + which);
                    }
                }


            });

            action LOAD = game.addAction("LOAD", true, new List<string>(), delegate (string par)
            {

                gui.clearScreen();
                gui.clearOption();

                for (int i = 1; i < 4; i++)
                {
                    string savetext = "   FILE #" + i + "   \n\n";
                    string savename = null;
                    if (game.hasSave(i))
                    {
                        savename = game.getSaveName(i);
                        if (savename != null)
                            savetext += game.getSaveName(i);
                    }
                    else
                        savetext += "EMPTY";

                    guiElement savebox = gui.textBox(0, 0, savetext);
                    if (i == 1)
                        savebox.alignVertical();
                    if (i == 2)
                        savebox.center();
                    if (i == 3)
                    {
                        savebox.rightTop();
                        savebox.alignVertical();
                    }

                    gui.queue(savebox);
                }

                gui.load();

                gui.draw();

                gui.throwError("Please enter the number of the save file you wish to load!");
                gui.setCursor(false);

                int which = CAFe.readNumber();

                if (which < 1 || which > 3)
                    gui.throwError("That file does not exists!");
                else
                {
                    if (game.hasSave(which))
                    {
                        gui.throwError("Loading file #" + which + "!");
                        game.load(which);
                    }
                    else
                    {

                        gui.throwError("File #" + which + " is empty!");
                    }
                }


            });


            //game.addStory(b);
            //game.addStory(c);


            map a = game.addMap("id", "id2", "", "", "", "lorem ipsum", b);
            game.setSpawn(a);
            equipment sword = equipment.addEquipment("id1", "Sword", "A powerful sword.", 1000, 5, 1, 10, "weapon");
            usable potion = usable.addUsable("id2", "Potion", "Heals 50 hp.", 15, delegate ()
            {
                player.setHealth( player.getHealth() + 50 );
            });
            item rock = item.addItem("id3", "rock", "It's a rock.", 1);

            a.addItem(sword);
            a.addItem(potion);
            a.addItem(rock);


            actor bob = a.addActor("BOB", true, true, false);

            List<action> subjects = new List<action>();
            action n = new action("boobs", true, new List<string>(new string[] { d }), null);
            subjects.Add(n);
            n = new action("ass", true, new List<string>(new string[] { e }), null);
            subjects.Add(n);
            bob.addDialog(f, subjects, false);

            actor store = a.addActor("WEAPONS", true, false, true);

            store.addStore("You see the weapons hanging on the walls.", new List<Iitem>(new Iitem[] { sword }));

            monster dog = new monster("Doge", null, 100, 100, 5);
            dog.setAttack(true, 5, 1);
            dog.setMagic(true, 5, 0, 1);
            dog.setHeal(true, 5, 0, false, 0, 50, 0, 5);
            List<action> user = new List<action>();
            n = new action("attack", true, null, delegate (string param)
            {
                game.getMap().getMonster().takeDamage(player.getStrength());
            });
            user.Add(n);
            n = new action("heal", true, null, delegate (string param)
            {
                game.getMap().findActor("chat").findAction("heal").setVisible(false);
                action.reset["heal"] = game.getMap().getRound() + 2;
                player.heal( player.getMaxHealth()*player.getInteligence()/250 );
            });
            user.Add(n);
            a.addMonster(dog, user);



            //a.removeItem("sword");

            CAFe.mainMenu();

            //CAFe.ending();


        }

    }
}
