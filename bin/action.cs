using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{
    class action
    {
        private string keyword;
        private bool single;
        private List<string> param;
        private ActionChoice act = null;
        public bool visible = true;
        public static Dictionary<string, int> reset = new Dictionary<string, int>();


        public delegate void ActionChoice(string par);

        public action(string keyword, bool single, List<string> param, ActionChoice act)
        {
            this.keyword = keyword;
            this.single = single;
            this.param = param;
            this.act = act;
        }

        public void setVisible(bool state)
        {
            visible = state;
        }

        public bool getVisible()
        {
            return visible;
        }

        public string getKeyword()
        {
            return keyword.ToUpper();
        }

        public bool hasParam(string par)
        {
            if (single)
                return false;
            par = par.ToUpper();
            return param.Contains(par);
        }

        public void clearParam()
        {
            param = new List<string>();
        }

        public void addParam(string par)
        {
            par = par.ToUpper();
            if(param.Contains(par))
            {
                console.log(2, "Tried to add duplicated parameter \"" + par + "\" to action " + keyword);
                return;
            }
            param.Add(par);
        }

        public void changeAct( ActionChoice act)
        {
            this.act = act;
        }

        public ActionChoice getAct()
        {
            return act;
        }

        public void perform(string param)
        {
            if(param != null)
                param = param.ToUpper();
            if (act != null)
                act(param);
        }

        public bool getParam()
        {
            return !single;
        }

        public List<string> getParamList()
        {
            return param;
        }

    }
}
