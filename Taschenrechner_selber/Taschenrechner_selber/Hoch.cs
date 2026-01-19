using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taschenrechner_selber
{
    public class Hoch : Anweisung
    {
        List<Anweisung> h_Fortsetzungs = new List<Anweisung>();
        Token value = new Token();

        public override void Parse(ref List<Token> tokenList)
        {
            if (tokenList[0].Type == TokenType.Number || tokenList[0].Type == TokenType.Variable)
            {
                if (tokenList[0].Type == TokenType.Variable && !Anweisung.variables.ContainsKey(tokenList[0].Text))
                {
                    Anweisung.variables.Add(tokenList[0].Text, 0);
                }
                value = tokenList[0];
                tokenList.RemoveAt(0);
            }
            else if (tokenList[0].Text.Equals("("))
            {
                tokenList.RemoveAt(0);

                // Prüfen, ob die Klammer leer ist
                if (tokenList.Count == 0 || tokenList[0].Text == ")")
                {
                    Anweisung.errors.Add("Leere Klammern sind nicht erlaubt");
                    if (tokenList.Count > 0)
                        tokenList.RemoveAt(0); // schließe die Klammer
                    return;
                }

                PlusMinus h = new PlusMinus();
                h.Parse(ref tokenList);
                h_Fortsetzungs.Add(h);
            }
            else if (tokenList[0].Type == TokenType.Error)
            {
                Anweisung.errors.Add("Unexpected: " + tokenList[0].Text);
            }
            while (tokenList.Count > 0)
            {
                if (tokenList[0].Type == TokenType.hOperator)
                {
                    tokenList.RemoveAt(0);

                    if (tokenList.Count == 0)
                    {
                        Anweisung.errors.Add("Operator ^ ohne rechten Operand");
                        break;
                    }

                    if (tokenList[0].Text.Equals("("))
                    {
                        tokenList.RemoveAt(0);

                        // Prüfen auf leere Klammern nach ^
                        if (tokenList.Count == 0 || tokenList[0].Text == ")")
                        {
                            Anweisung.errors.Add("Leere Klammern sind nicht erlaubt nach ^");
                            if (tokenList.Count > 0)
                                tokenList.RemoveAt(0); // schließe die Klammer
                            break;
                        }

                        PlusMinus h = new PlusMinus();
                        h.Parse(ref tokenList);
                        h_Fortsetzungs.Add(h);
                    }
                    else if (tokenList[0].Type == TokenType.hOperator)
                    {
                        Anweisung.errors.Add("Mehrfachoperator ^ nicht erlaubt");
                        break;
                    }
                    else
                    {
                        Hoch h = new Hoch();
                        h.Parse(ref tokenList);
                        h_Fortsetzungs.Add(h);
                    }
                }
                else if (tokenList[0].Type == TokenType.Error)
                {
                    Anweisung.errors.Add("Unexpected: " + tokenList[0].Text);
                    tokenList.RemoveAt(0);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        public override double Run()
        {
            double val = 0;
            int j = 0;
            if(value.Type == TokenType.Error)
            {
                val = h_Fortsetzungs[0].Run();
                j += 1;
            }
            else
            {
                if (value.Type == TokenType.Variable)
                {
                    val = Anweisung.variables.GetValueOrDefault(value.Text);
                }
                else
                {
                    val = double.Parse(value.Text);
                }
            }
            for (int i = j; i < h_Fortsetzungs.Count; i++)
            {
                val = Math.Pow(val, h_Fortsetzungs[i].Run());
            }

            return val;
        }
    }
}
