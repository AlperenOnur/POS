using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taschenrechner_selber
{
    public class Anweisung
    {
        public virtual void Parse(ref List<Token> tokenList) { }
        public virtual double Run() { return 0; }
        public static List<string> errors = new List<string>();
        public static Dictionary<string, double> variables = new Dictionary<string, double>();
    }
}
