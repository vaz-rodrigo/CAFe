using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{
    class CAFe
    {
        public static int height = 25;
        public static int width = 100;
        public static int buffer_size = 10;

        private static string cafeVersion = "1.0";

        public static string getAuthor()
        {
            return "Rodrigo Vaz";
        }

        public static string getVersion()
        {
            return cafeVersion;
        }



        public static void mainMenu()
        {

            int option;

            while (true)
            {
                guiElement titleBox = gui.textBox(0, 0, game.getTitle().ToUpper() + "\n\nA game by " + game.author);
                guiElement options = gui.text(1, 15, "Type 1 to start a new game.\n\nType 2 to load a game.\n\nType any other number to exit.");

                titleBox.alignHorizontal();

                gui.queue(titleBox);
                gui.queue(options);

                gui.buildFrame();
                gui.load();
                gui.draw();


                option = readNumber();

                switch (option)
                {
                    case (1):
                        newGame();
                        break;
                    case (2):
                        loadGame();
                        break;
                    default:
                        return;
                }

            }

        }


        private static void newGame()
        {

            gui.clearScreen();
            gui.clearOption();

            game.running = true;

            story();
            character();

            if(game.getSpawn() == null)
            {
                console.log(3, "No spawn map set.");
                crash("THE GAME CRASHED BECAUSE THERE WAS NO SPAWN MAP SET.");
                return;
            }

            game.changeMap(game.getSpawn());

            body();

        }

        private static void story()
        {

            if (game.story.Count == 0)
            {
                console.log(2, "Story pages were not suplied, skipping story.");
                return;
            }

            int page = 1;
            foreach (string pages in game.story)
            {
                gui.clearScreen();
                gui.clearOption();

                gui.queue(gui.text(1, 1, pages));

                guiElement toContinue = gui.text(0, 0, "Press any key to continue.");
                toContinue.leftBottom();

                guiElement outOff = gui.text(0, 0, "PAGE " + page + " OF " + game.story.Count);
                outOff.rightBottom();

                gui.queue(toContinue);
                gui.queue(outOff);

                gui.load();
                gui.draw();

                gui.setCursor(false);

                Console.ReadKey();
                page++;

            }

        }

        private static void character()
        {
            if(game.skipCharacter)
                return;

            int option = 0;
            string confirm;

            do
            {
                confirm = "";
                guiElement newBox = gui.textBox(0, 0, "NEW CHARACTER");
                newBox.alignHorizontal();
                gui.queue(newBox);

                gui.queue(gui.text(1, 7, "Please enter the name of your character:"));

                gui.load();
                gui.draw();

                do
                {
                    gui.clearOption();

                }
                while (!player.setName(Console.ReadLine()));

                gui.clearOption();

                confirm = confirm + "Your name is: \n" + player.getName() + "\n";

                if(!game.skipStats)
                {
                    int spent = 0;
                    int strength = 0;
                    int inteligence = 0;
                    int dexterity = 0;

                    gui.clearScreen();
                    gui.queue(gui.text(1, 7, "You have " + game.statPoints + " stat points to spend.\nHow many points do you want to spend into Strength?"
                        + "\n\nStrength scales your physical attacks."));
                    gui.queue(newBox);
                    gui.load();
                    gui.draw();

                    strength = readNumber();

                    while(strength > game.statPoints - spent || strength < 0)
                    {
                        gui.throwError("YOU CAN'T SPEND MORE POINTS THAN YOU HAVE!");
                        strength = readNumber();
                    }
                    spent = strength;

                    if (spent != game.statPoints)
                    {
                        gui.clearScreen();
                        gui.clearOption();
                        gui.queue(gui.text(1, 7, "You still have " + (game.statPoints - spent) + " stat points to spend.\nHow many points do you want to spend into Inteligence?"
                        + "\n\nInteligence scales your magical attacks and your magical defense."));
                        gui.queue(newBox);
                        gui.load();
                        gui.draw();

                        inteligence = readNumber();

                        while (inteligence > game.statPoints - spent || inteligence < 0)
                        {
                            gui.throwError("YOU CAN'T SPEND MORE POINTS THAN YOU HAVE!");
                            inteligence = readNumber();
                        }
                        spent += inteligence;

                        if (spent != game.statPoints)
                        {
                            gui.clearScreen();
                            gui.clearOption();
                            gui.queue(gui.text(1, 7, "You still have " + (game.statPoints - spent) + " stat points to spend.\nHow many points do you want to spend into Dexterity?"
                        + "\n\nDexterity scales your physical defense."));
                            gui.queue(newBox);
                            gui.load();
                            gui.draw();

                            dexterity = readNumber();

                            while (dexterity > game.statPoints - spent || dexterity < 0)
                            {
                                gui.throwError("YOU CAN'T SPEND MORE POINTS THAN YOU HAVE!");
                                dexterity = readNumber();
                            }
                            spent += dexterity;

                        }

                    }

                    confirm = confirm + "\nStrength: " + strength + "\n\nInteligence: " + inteligence + "\n\nDexterity: " + dexterity + "\n\nUnspent points: " + (game.statPoints - spent) + "\n";

                    player.setDexterity(dexterity);
                    player.setInteligence(inteligence);
                    player.setStrength(strength);

                }

                player.setMoney(game.startMoney);
                player.setMaxHealth(game.starthp );
                player.setHealth(game.starthp);

                gui.queue(gui.text(1, 1, confirm));

                guiElement toContinue = gui.text(0, 0, "Type 1 to continue. Type any other number to start again.");
                toContinue.leftBottom();

                gui.queue(newBox);
                gui.queue(toContinue);

                gui.load();
                gui.draw();

                option = readNumber();

            }

            while (option != 1);
        }



        private static void loadGame()
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

            int which = readNumber();

            if (which < 1 || which > 3)
                gui.throwError("That file does not exists!");
            else
            {
                if (game.hasSave(which))
                {
                    gui.throwError("Loading save file #" + which + "!");
                    game.load(which);
                    body();
                }
                else
                {

                    gui.throwError("File #" + which + " is empty!");
                }
            }

        }



        private static void body()
        {


            while(game.running)
            {
                gui.clearOption();
                map m = game.getMap();
                m.OnChange();
                if (!game.running)
                    return;

                string mapText = m.ToString() + "\n\n" + itemText() + "\n\n" + directionText() + "\n\n";



                if (m.getId().ToUpper() == "STORE")
                {

                    guiElement moneys = gui.textBox(0, 0, "YOUR MONEY:\n" + player.getMoney() + " GOLD");
                    moneys.rightBottom();
                    gui.queue(moneys);
                    mapText += "This store sells:\n\n";

                    foreach(Iitem it in m.getItems())
                    {
                        mapText += it.getName() + " - " + (int)Math.Round(it.getValue() * game.priceMultiplier) + " GOLD\n" + it.getDescription();
                    }
                }

                if (m.getId().ToUpper() == "DIALOG")
                {
                    string curname = game.overwroteMapSign;
                    if (game.overwroteMap != null)
                        curname = game.overwroteMap.getName();

                    if(game.dialogOptions)
                    {
                        string opttext = "CHAT OPTIONS:\n\n";

                        foreach(action a in m.findActor("chat").getActions())
                        {
                            if(a.getVisible())
                                if (opttext == "CHAT OPTIONS:\n\n")
                                    opttext += a.getKeyword().ToUpper();
                                else
                                    opttext += " " + a.getKeyword().ToUpper();
                        }

                        guiElement chatopt = gui.textBox(0, 0, opttext);
                        chatopt.rightBottom();
                        gui.queue(chatopt);
                    }

                    guiElement sign = gui.textBox(0, 0, "CURRENTLY IN:\n" + curname.ToUpper());
                    sign.rightTop();
                    gui.queue(sign);
                }

                if (m.getId().ToUpper() == "FIGHT")
                {
                    monster mob = m.getMonster();

                    if (!m.getVisited())
                    {
                        Random rnd = new Random();

                        int coin = rnd.Next(0, 2);

                        if (coin == 1)
                        {
                            mob.setNext();
                            gui.nextDrawError(mob.getName().ToUpper() + " WILL ATTACK FIRST!");
                        }
                        else
                            gui.nextDrawError("YOU WILL ATTACK FIRST!");


                    }

                    guiElement fightname = gui.textBox(0, 0, "FIGHTING\n" + mob.getName() );
                    fightname.alignHorizontal();
                    gui.queue(fightname);

                    string plytext = "PLAYER\n\nHP: " + player.getHealth() + " / " + player.getMaxHealth() + "\n\nSTRENGTH: " + player.getStrength();
                    plytext += "\n\nINTELIGENCE: " + player.getInteligence() + "\n\nDEXTERITY: " + player.getDexterity();

                    gui.queue(gui.textBox(0, 0, plytext ));

                    string enemyText = mob.getName() + "\n\nHP: " + mob.getHealth() + " / " + mob.getMaxHealth();
                    enemyText += "\n\nARMOR: " + mob.getArmor() + "\n\n\n\n";

                    guiElement enemyBox = gui.textBox(0, 0, enemyText );
                    enemyBox.rightTop();

                    gui.queue(enemyBox);



                    m.setVisited();

                    if (!m.getMonster().getNext())
                    {
                        string opttext = "ATTACK MOVES:\n\n";

                        foreach (action a in m.findActor("chat").getActions())
                        {
                            if (a.getVisible())
                                if (opttext == "ATTACK MOVES:\n\n")
                                    opttext += a.getKeyword().ToUpper();
                                else
                                    opttext += " " + a.getKeyword().ToUpper();
                        }

                        guiElement chatopt = gui.textBox(0, 0, opttext);
                        chatopt.rightBottom();
                        gui.queue(chatopt);

                    }

                }


                gui.queue(gui.text(1, 1, mapText));

                gui.load();
                gui.draw();

                if (m.getId().ToUpper() == "FIGHT")
                {
                    m.addRound();

                    foreach(KeyValuePair<string, int> ac in action.reset)
                    {
                        if (m.getRound() == (ac.Value+1))
                            m.findActor("chat").findAction(ac.Key).setVisible(true);
                    }

                    if (m.getMonster().getNext())
                    {
                        m.getMonster().perform();
                    }
                    else
                    {
                        readFightOption();
                        m.getMonster().setNext();

                    }

                    if (player.getHealth() <= 0)
                    {
                        gui.throwError("You received fatal damage!");
                        player.die();
                        return;
                    }
                    if(m.getMonster().getHealth() <= 0)
                    {
                        gui.throwError("You fatally wounded the enemy!");
                        m.getMonster().death();

                        gui.clearScreen();
                        gui.clearOption();
                        List<Iitem> loot = m.getMonster().getLoot();
                        string deathText = "YOU KILLED " + m.getMonster().getName() + "\n\nHIS LOOT IS:";

                        if (loot == null || loot.Count == 0)
                            deathText += " NOTHING";
                        else
                            foreach (Iitem i in loot)
                            {
                                deathText += " " + i.getName();
                                player.giveItem(i);
                            }
                        deathText += ".\n\nYOU FOUND " + m.getMonster().getValue() + " GOLD.";
                        player.setMoney(player.getMoney() + m.getMonster().getValue());

                        gui.queue(gui.text(1, 1, deathText));

                        guiElement toContinue = gui.text(0, 0, "Press any key to continue.");
                        toContinue.leftBottom();

                        gui.queue(toContinue);

                        gui.load();
                        gui.draw();

                        gui.setCursor(false);
                        Console.ReadKey();

                        m = game.findMap(m.getNorth());
                        m.resetMonster();
                        game.changeMap(m);


                    }


                }
                else
                    readOption();

            }

        }

        public static void ending()
        {
            gui.clearScreen();
            gui.clearOption();

            guiElement toContinue = gui.text(0, 0, "Press any key to continue.");
            toContinue.leftBottom();
            gui.queue(toContinue);

            if (game.ending.Count == 0)
            {
                gui.queue(gui.text(1, 1, "CONGRATULATIONS, YOU BEAT " + game.title.ToUpper() + "!"));
                console.log(2, "Ending pages were not suplied, using default ending.");
                gui.load();
                gui.draw();
                gui.setCursor(false);

                Console.ReadKey();
                game.running = false;

                return;
            }

            int page = 1;
            foreach (string pages in game.ending)
            {

                gui.queue(gui.text(1, 1, pages));

                guiElement outOff = gui.text(0, 0, "PAGE " + page + " OF " + game.ending.Count);
                outOff.rightBottom();

                gui.queue(outOff);

                gui.load();
                gui.draw();

                gui.setCursor(false);

                Console.ReadKey();
                page++;
                game.running = false;

            }

        }

        public static void death()
        {
            gui.clearScreen();
            gui.clearOption();

            guiElement toContinue = gui.text(0, 0, "Press any key to continue.");
            toContinue.leftBottom();
            gui.queue(toContinue);

            if (game.death.Count == 0)
            {
                gui.queue(gui.text(1, 1, "YOU DIED!"));
                console.log(2, "Death pages were not suplied, using default death.");
                gui.load();
                gui.draw();
                gui.setCursor(false);

                Console.ReadKey();
                game.running = false;

                return;
            }

            int page = 1;
            foreach (string pages in game.death)
            {

                gui.queue(gui.text(1, 1, pages));

                guiElement outOff = gui.text(0, 0, "PAGE " + page + " OF " + game.death.Count);
                outOff.rightBottom();

                gui.queue(outOff);

                gui.load();
                gui.draw();

                gui.setCursor(false);

                Console.ReadKey();
                page++;
                game.running = false;

            }

        }



        private static string itemText()
        {
            if (game.getMap().getId().ToUpper() == "DIALOG" || game.getMap().getId().ToUpper() == "FIGHT" || game.getMap().getId().ToUpper() == "STORE")
                return "";

            string ret = "";
            List < Iitem > items = game.getMap().getItems();

            if (items.Count == 0)
               return "There are no items around.";
            else
            {
                ret += "You can see the following items:\n";
                foreach (Iitem i in items)
                {
                    ret +=i.getName() + ", ";

                }

                ret = ret.Remove(ret.Length-2);

            }
            return ret;

        }

        private static void readOption()
        {

            string option;

            do
            {
                gui.clearOption();
                option = Console.ReadLine();

            }
            while ( option.Trim() == "" || option == null);

            string[] choice = option.ToUpper().Split(' ');
            string key = null;
            string par = null;

            key = choice[0];

            if (choice.Length > 1)
            {
                par = "";
                for (int i = 1; i < choice.Length; i++)
                    par += choice[i] + " ";
                par = par.Trim();
            }

            action act = game.findAction(key);
            if (act != null && game.getMap().getId().ToUpper() != "DIALOG" && game.getMap().getId().ToUpper() != "FIGHT" && game.getMap().getId().ToUpper() != "STORE")
            {

                if((par == null || par == "") && act.getParam() && act.getVisible())
                {
                    gui.throwError("The keyword " + key + " needs a parameter!");
                    return;
                }

                else if((par == null || par == "") && !act.getParam() && act.getVisible())
                {
                    act.perform(par);
                    return;
                }

                else if ((par != null && par != "") && act.getParam() && act.getVisible())
                {
                    if(!act.hasParam(par))
                    {
                        gui.throwError("You can't do that!");
                        return;
                    }
                    act.perform(par);
                    return;
                }
                else if((par != null && par != "") && !act.getParam() && act.getVisible())
                {
                    gui.throwError("The keyword " + key + " does not need a parameter!");
                    return;
                }

            }
            act = null;
            if(act == null)
            {
                List<actor> actors = game.getMap().getActors();
                foreach(actor a in actors)
                {
                    if (!a.getVisible())
                        continue;

                    act = a.findAction(key);

                    if(act == null)
                    {
                        continue;
                    }
                    if (act.getParam() && (par == null || par == "") && act.getVisible())
                    {
                        gui.throwError("The keyword " + key + " needs a parameter!");
                        return;
                    }
                    if (act.getParam() && !act.hasParam(par) && act.getVisible())
                    {
                        continue;
                    }
                    if (act.getParam() && act.hasParam(par) && act.getVisible())
                    {
                        act.perform(par);
                        return;
                    }
                    if (!act.getParam() && (par != null && par != "") && act.getVisible())
                    {
                        gui.throwError("The keyword " + key + " does not need a parameter!");
                        return;
                    }
                    if (!act.getParam() && (par == null || par == "") && act.getVisible())
                    {
                        if(game.getMap().getId().ToUpper() == "DIALOG" && act.getKeyword() != "QUIT")
                        {
                            game.getMap().setDescription( act.getParamList()[0] );
                        }
                        act.perform(par);
                        return;
                    }

                }
                
            }
            gui.throwError("Coudln't find an action!");
            gui.clearOption();


        }

        private static void readFightOption()
        {


            string option;
            bool valid = false;

            do
            {

                do
                {
                    gui.clearOption();
                    option = Console.ReadLine();

                }
                while (option.Trim() == "" || option == null);

                string[] choice = option.ToUpper().Split(' ');
                string key = null;
                string par = null;

                key = choice[0];

                if (choice.Length > 1)
                    par = choice[1].Trim();

                actor chatter = game.getMap().findActor("chat");

                action act = chatter.findAction(key);

                if (act == null || !act.getVisible())
                {

                    gui.throwError("Coudln't find an action!");
                    gui.clearOption();

                }
                else if (act.getParam() && (par == null || par == "") && act.getVisible())
                {
                    gui.throwError("The keyword " + key + " needs a parameter!");
                    gui.clearOption();

                }
                else if (act.getParam() && !act.hasParam(par) && act.getVisible())
                {
                }
                else if (act.getParam() && act.hasParam(par) && act.getVisible())
                {
                    act.perform(par);
                    valid = true;
                    gui.clearOption();
                }
                else if (!act.getParam() && (par != null && par != "") && act.getVisible())
                {
                    gui.throwError("The keyword " + key + " does not need a parameter!");
                    gui.clearOption();
                }
                else if (!act.getParam() && (par == null || par == "") && act.getVisible())
                {
                    act.perform(par);
                    valid = true;
                    gui.clearOption();
                }

            }
            while (!valid);
        }

        private static string directionText()
        {

            if (game.getMap().getId().ToUpper() == "DIALOG" || game.getMap().getId().ToUpper() == "FIGHT" || game.getMap().getId().ToUpper() == "STORE")
                return "";

            string dirs = game.getMap().getAvaibleDirections();

            if (dirs == "")
                return "You can't go anywhere.";
            else
                return "You can go " + dirs + ".";

        }

        public static int readNumber()
        {
            int option;
            do
            {
                gui.clearOption();

            }
            while (!int.TryParse(Console.ReadLine(), out option));

            return option;
        }



        public static void crash(string message)
        {

            gui.clearScreen();
            gui.clearOption();
            gui.queue(gui.text(1, 1, message));
            gui.load();
            gui.draw();

            Console.ReadKey();

        }

    }

    class gui
    {

        private const char FRAME_WALL = '|';
        private const char FRAME_ROOF = '=';
        private const int FRAME_MARGIN_LEFT = 2;
        private const int FRAME_MARGIN_TOP = 1;
        private static int CURSOR_OPTX = FRAME_MARGIN_LEFT + 2;
        private static int CURSOR_OPTY = FRAME_MARGIN_TOP + CAFe.height + 2;
        private static int CURSOR_DRAWX = FRAME_MARGIN_LEFT + 1;
        private static int CURSOR_DRAWY = FRAME_MARGIN_TOP + 1;
        private static string NEXT_DRAW_ERROR = "";

        private static List<guiElement> buffer = new List<guiElement>();
        private static char[] errorCopy = new char[CAFe.width];
        private static char[,] screen = new char[CAFe.height, CAFe.width];

        private static bool ready = false;
        private static bool frameBuilt = false;

        public static bool getReady()
        {
            return ready;
        }

        public static bool bufferFull()
        {
            if (buffer.Count() == CAFe.buffer_size)
                return true;
            return false;
        }

        public static char[,] parseString(string content)
        {
            char[,] ret = null;
            int width = 0;
            string contentStripped = content.Replace(Environment.NewLine, "\n");
            string[] sub = contentStripped.Split('\n');

            foreach (string s in sub)
                if (s.Length > width)
                    width = s.Length;

            ret = new char[sub.Length, width];

            for (int i = 0; i < sub.Length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j >= sub[i].Length)
                        ret[i, j] = ' ';
                    else
                        ret[i, j] = sub[i][j];
                }

            }


            return ret;
        }

        public static bool queue( guiElement queue )
        {
            if (bufferFull())
            {
                console.log(3, "Failed to queue guiElement, buffer is full. Try increasing its size.");
                return false;
            }

            if (buffer.Contains(queue))
            {
                console.log(2, "Tried to queue a guiElement which is already queued.");
                return false;
            }

            buffer.Add(queue);
            return true;

        }

        public static bool load()
        {
            if (getReady())
            {
                console.log(3, "Display was already loaded, you must draw it.");
                return false;
            }
            buffer.Reverse();

            for(int i = 0; i < CAFe.height; i++)
            {
                for(int j = 0; j < CAFe.width; j++)
                {
                    screen[i, j] = ' ';
                    foreach(guiElement w in buffer)
                    {
                        if ((i - w.getY()) < 0 || (j - w.getX()) < 0 || (i - w.getY()) >= w.getHeight() || (j - w.getX()) >= w.getWidth())
                            continue;
                        screen[i, j] = w.getScreen()[i - w.getY(), j - w.getX()];

                    }

                }

            }

            for (int i = 0; i < CAFe.width; i++)
                errorCopy[i] = screen[CAFe.height - 2, i];

            return ready = true;
        }

        public static void draw()
        {
            
            setCursor(true);

            for (int i = 0; i < CAFe.height; i++)
            {
                for (int j = 0; j < CAFe.width; j++)
                {
                    Console.Write(screen[i, j]);

                }
                Console.Write("\n");
                Console.SetCursorPosition(Console.CursorLeft + FRAME_MARGIN_LEFT + 1, Console.CursorTop);
            }

            clearBuffer();
            if (NEXT_DRAW_ERROR != "")
                throwError(NEXT_DRAW_ERROR);

            NEXT_DRAW_ERROR = "";
            ready = false;
            setCursor(false);

        }

        public static guiElement textBox(int x, int y, string text)
        {
            int width = 0;
            string[] sub = text.Split('\n');
            string toParse = "";

            foreach (string s in sub)
                if (s.Length > width)
                    width = s.Length;

            toParse = toParse + new string(FRAME_ROOF, width + 4) + "\n" + FRAME_WALL + new string(' ', width + 2) + FRAME_WALL +"\n";

            foreach(string s in sub)
            {
                toParse = toParse + FRAME_WALL + " " + s + new string(' ', width - s.Length) + " " + FRAME_WALL + "\n";
            }

            toParse = toParse + FRAME_WALL + new string(' ', width + 2) + FRAME_WALL + "\n" + new string(FRAME_ROOF, width + 4);

            return new guiElement(x, y, gui.parseString(toParse));

        }

        public static guiElement text(int x, int y, string text)
        {
            return new guiElement( x, y, gui.parseString( text ) );
        }

        public static guiElement box(int x, int y, int heigth, int width)
        {

            string toParse = new string(FRAME_ROOF, width + 2) + "\n";
            for (int i = 0; i < heigth; i++)
                toParse = toParse + FRAME_WALL + new string(' ', width) + FRAME_WALL + "\n";
            toParse = toParse + new string(FRAME_ROOF, width + 2) + "\n";

            return new guiElement(x, y, gui.parseString(toParse));
        }

        public static void buildFrame()
        {

            if(frameBuilt)
            {
                console.log(3, "Frame was already built.");
                return;
            }


            Console.Write(new string('\n', FRAME_MARGIN_TOP));
            Console.Write(new string(' ', FRAME_MARGIN_LEFT) + new string(FRAME_ROOF, CAFe.width + 2) + "\n");

            for(int i = 0; i < CAFe.height; i++)
            {
                Console.WriteLine(new string(' ', FRAME_MARGIN_LEFT) + FRAME_WALL + new string(' ', CAFe.width) + FRAME_WALL);
            }
            Console.WriteLine(new string(' ', FRAME_MARGIN_LEFT) + new string(FRAME_ROOF, CAFe.width + 2));
            Console.WriteLine(new string(' ', FRAME_MARGIN_LEFT) + FRAME_WALL + new string(' ', CAFe.width) + FRAME_WALL);
            Console.WriteLine(new string(' ', FRAME_MARGIN_LEFT) + new string(FRAME_ROOF, CAFe.width + 2));

            frameBuilt = true;

        }

        private static void clearBuffer()
        {
            buffer = new List<guiElement>();
        }

        public static void clearScreen()
        {
            screen = new char[CAFe.height, CAFe.width];
            draw();
        }

        public static void clearOption()
        {
            gui.setCursor(false);
            Console.Write(new string(' ', CAFe.width - 1));
            gui.setCursor(false);
        }

        public static void setCursor(bool draw)
        {
            if (draw)
                Console.SetCursorPosition(CURSOR_DRAWX, CURSOR_DRAWY);
            else
                Console.SetCursorPosition(CURSOR_OPTX, CURSOR_OPTY);
        }

        public static void throwError(string message)
        {
            Console.SetCursorPosition(FRAME_MARGIN_LEFT + 2, FRAME_MARGIN_TOP - 1 + CAFe.height);
            Console.Write(message);
            Console.ReadKey();
            Console.SetCursorPosition(FRAME_MARGIN_LEFT + 1, FRAME_MARGIN_TOP - 1 + CAFe.height);
            Console.Write(errorCopy);
            setCursor(false);

        }

        public static void nextDrawError(string message)
        {
            NEXT_DRAW_ERROR = message;
        }

    }

    class guiElement
    {
        private int height;
        private int width;

        private int x;
        private int y;

        private char[,] screen;

        public guiElement(int height, int width, int x, int y, char[,] screen)
        {
            this.height = height;
            this.width = width;

            this.x = x;
            this.y = y;

            this.screen = screen;
        }

        public guiElement(int height, int width, int x, int y) :
            this (height, width, x, y, new char[height, width])
        {
        }

        public guiElement(int x, int y, char[,] screen) :
            this (screen.GetLength(0), screen.GetLength(1), x, y, screen)
        {
        }

        public bool setContent( char[,] screen )
        {
            int height = screen.GetLength(0);
            int width = screen.GetLength(1);

            if (this.height < height || this.width < width)
                return false;
            if (this.height > height || this.width > width)
            {
                for(int i = 0; i < this.height; i++)
                {
                    for(int j = 0; j < this.width; j++)
                    {
                        if (i >= height || j >= width)
                            this.screen[i, j] = ' ';
                        else
                            this.screen[i, j] = screen[i, j];
                    }

                }
                return true;
            }
            this.screen = screen;
            return true;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }



        public int getHeight()
        {
            return height;
        }

        public int getWidth()
        {
            return width;
        }

        public char[,] getScreen()
        {
            return screen;

        }

        public void alignVertical()
        {
            y = (CAFe.height/2 - height/2);
        }

        public void alignHorizontal()
        {
            x = (CAFe.width/2 - width / 2);
        }

        public void center()
        {
            alignVertical();
            alignHorizontal();
        }

        public void leftBottom()
        {
            x = 0;
            y = CAFe.height - height;
        }

        public void rightBottom()
        {
            x = CAFe.width - width;
            y = CAFe.height - height;
        }

        public void leftTop()
        {
            x = 0;
            y = 0;
        }

        public void rightTop()
        {
            x = CAFe.width - width;
            y = 0;
        }
    }

    class console
    {
        public static bool log(int type, string message)
        {
            try
            {
                System.IO.TextWriter textOut =
                new System.IO.StreamWriter("log.txt", true);

                string header;

                switch (type)
                {
                    case 1:
                        header = "LOG: ";
                        break;
                    case 2:
                        header = "WARNING: ";
                        break;
                    case 3:
                        header = "ERROR: ";
                        break;
                    case 4:
                        header = "MESSAGE: ";
                        break;
                    default:
                        header = "";
                        break;
                }

                textOut.WriteLine(DateTime.Now + " " + header + message);
                textOut.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
