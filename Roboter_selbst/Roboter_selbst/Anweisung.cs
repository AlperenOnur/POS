using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter_selbst
{
    public abstract class Anweisung
    {
        public Anweisung() { }

        public abstract void Parse(ref List<Token> tokenList);
        public abstract bool Run(MainWindow mw);

        public static List<string> Errors = new List<string>();
    }
}
