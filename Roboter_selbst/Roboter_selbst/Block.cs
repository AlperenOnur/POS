using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter_selbst
{
    internal class Block : Anweisung
    {
        Program p = new Program();

        public override void Parse(ref List<Token> tokenList)
        {
            if (tokenList.Count == 0 || tokenList[0].text != "{")
            {
                Anweisung.Errors.Add("{ fehlt");
                return;
            }

            tokenList.RemoveAt(0); // {

            p.Parse(ref tokenList);

            // Program.Parse entfernt die }
        }

        public override bool Run(MainWindow mw)
        {
            return p.Run(mw);
        }
    }
}
