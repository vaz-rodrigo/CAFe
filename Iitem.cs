using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Adventure_Framework
{
    public interface Iitem
    {

        string getId();
        int getValue();
        string getName();
        string getDescription();
        string itemType();

    }
}
