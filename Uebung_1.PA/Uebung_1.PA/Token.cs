using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uebung_1.PA
{
    public class Token
    {
        public enum Type {Keyword, Direction, Letter, Obstacle, Nummer, Error, Klammer};

        public Type type = Type.Error;
        public string text = "Error";

    }
}
