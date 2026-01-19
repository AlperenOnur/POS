using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uebung_1.PA
{
    internal class Repeat : Anweisung
    {
        int number = 0;
        Block b;
        public override void Parse(ref List<Token> tokenList)
        {
            tokenList.RemoveAt(0);

            if (tokenList.Count < 0)
            {
                Anweisung.Errors.Add("Direction fehlt");
            }
            Token t = tokenList[0];
            if (t.type != Token.Type.Nummer)
            {
                Anweisung.Errors.Add("Nummer fehlt");
                if (tokenList[0].type == Token.Type.Error)
                {
                    Anweisung.Errors.Add("Unexpected: " + tokenList[0].text);

                    tokenList.RemoveAt(0);
                }
            }
            else
            {

                number = int.Parse(t.text);
                tokenList.RemoveAt(0);
                b = new Block();
                b.Parse(ref tokenList);
            }
        }

        public override bool Run(MainWindow mw)
        {
            for (int i = 0; i < number; i++)
            {
                if(!b.Run(mw))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
