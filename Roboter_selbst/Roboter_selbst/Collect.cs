using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter_selbst
{
    internal class Collect : Anweisung
    {
        public override void Parse(ref List<Token> tokenList)
        {
            tokenList.RemoveAt(0);
        }

        public override bool Run(MainWindow mw)
        {
            mw.robotField.Collect();
            return true;
        }
    }
}
