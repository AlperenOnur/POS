using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taschenrechner_selber
{

    public class PlusMinus : Anweisung
    {
        List<Anweisung> pm_Fortsetzungs = new List<Anweisung>();
        List<Token> operators = new List<Token>();

        public override void Parse(ref List<Token> tokenList)
        {
            MalDividiert md = new MalDividiert();
            md.Parse(ref tokenList);
            pm_Fortsetzungs.Add(md);

            while (tokenList.Count > 0)
            {
                if (tokenList[0].Text.Equals(")"))
                {
                    tokenList.RemoveAt(0);
                    return;
                }

                if (tokenList[0].Type != TokenType.pmOperator)
                {
                    break;
                }
                operators.Add(tokenList[0]);
                tokenList.RemoveAt(0);

                md = new MalDividiert();
                md.Parse(ref tokenList);

                pm_Fortsetzungs.Add(md);
            }
        }
        public override double Run()
        {
            double val = 0;
            val = pm_Fortsetzungs[0].Run();

            for(int i = 1; i < pm_Fortsetzungs.Count; i++)
            {
                if (operators[i - 1].Text.Equals("+"))
                {
                    val += pm_Fortsetzungs[i].Run();
                }
                else if (operators[i - 1].Text.Equals("-"))
                {
                    val -= pm_Fortsetzungs[i].Run();
                }
            }


            return val;

        }
    }


}
