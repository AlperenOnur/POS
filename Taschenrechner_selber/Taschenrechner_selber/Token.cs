using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taschenrechner_selber
{
    public enum TokenType { Number, Variable, Bracket, pmOperator, mdOperator, hOperator, Komma, Error};
    public class Token
    {
        public TokenType Type = TokenType.Error;
        public string Text = "Error";
    }
}
