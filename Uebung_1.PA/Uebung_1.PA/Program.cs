using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Uebung_1.PA
{
    public class Program : Anweisung
    {
        List<Anweisung> Anweisungen = new List<Anweisung>();

        public override void Parse(ref List<Token> tokenList)
        {
            while (tokenList.Count > 0)
            {
                Token t = tokenList[0];

                if (t.type == Token.Type.Klammer && t.text == "}")
                {
                    tokenList.RemoveAt(0); // }
                    return;
                }

                if (t.type == Token.Type.Error)
                {
                    Errors.Add("Unexpected: " + t.text);
                    tokenList.RemoveAt(0);
                    continue;
                }

                if (t.type != Token.Type.Keyword)
                {
                    Errors.Add("Unexpected: " + t.text);
                    tokenList.RemoveAt(0);
                    continue;
                }

                Anweisung aw;

                switch (t.text)
                {
                    case "REPEAT": aw = new Repeat(); break;
                    case "COLLECT": aw = new Collect(); break;
                    case "MOVE": aw = new Move(); break;
                    case "UNTIL": aw = new Schleife(); break;
                    case "IF": aw = new Bedingung(); break;
                    default:
                        Errors.Add("Unknown keyword: " + t.text);
                        tokenList.RemoveAt(0);
                        continue;
                }

                aw.Parse(ref tokenList);
                Anweisungen.Add(aw);
            }
        }


        public override bool Run(MainWindow mw)
        {
            foreach (Anweisung a in Anweisungen)
            {
                if(!a.Run(mw))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
