using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taschenrechner_selber
{
    public class MalDividiert : Anweisung
    {
        List<Anweisung> md_Fortsetzungs = new List<Anweisung>();
        List<Token> operators = new List<Token>();

        public override void Parse(ref List<Token> tokenList)
        {
            Hoch h = new Hoch();
            h.Parse(ref tokenList);
            md_Fortsetzungs.Add(h);

            while (tokenList.Count > 0)
            {
                if (tokenList[0].Type != TokenType.mdOperator)
                {
                    break;
                }
                operators.Add(tokenList[0]);
                tokenList.RemoveAt(0);

                h = new Hoch();
                h.Parse(ref tokenList);
                md_Fortsetzungs.Add(h);
            }
        }

        public override double Run()
        {
            double val = 0;
            val = md_Fortsetzungs[0].Run();

            for (int i = 1; i < md_Fortsetzungs.Count; i++)
            {
                if(operators[i - 1].Text.Equals("*"))
                {
                    val = val * md_Fortsetzungs[i].Run();
                }
                else if (operators[i - 1].Text.Equals("/"))
                {
                    val = val / md_Fortsetzungs[i].Run();
                }

            }


            return val;
        }
    }
}
