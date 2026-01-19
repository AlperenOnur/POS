using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uebung_1.PA
{
    internal class Move : Anweisung
    {
        
        RobotField.Direction dir;
        public override void Parse(ref List<Token> tokenList)
        {
            tokenList.RemoveAt(0);

            if (tokenList.Count < 0)
            {
                Anweisung.Errors.Add("Direction fehlt");
            }
            Token t = tokenList[0];
            if(t.type != Token.Type.Direction)
            {
                Anweisung.Errors.Add("Direction fehlt");
                if (tokenList[0].type == Token.Type.Error)
                {
                    Anweisung.Errors.Add("Unexpected: " + tokenList[0].text);

                    tokenList.RemoveAt(0);
                }
            }
            else
            {
                
                switch (t.text)
                {
                    case "UP":
                        dir = RobotField.Direction.Up; break;
                    case "DOWN":
                        dir = RobotField.Direction.Down; break;
                    case "LEFT":
                        dir = RobotField.Direction.Left; break;
                    case "RIGHT":
                        dir = RobotField.Direction.Right; break;
                }
                   
            }
            tokenList.RemoveAt(0);

        }

        public override bool Run(MainWindow mw)
        {
            return mw.robotField.Move(dir);
        }
    }
}
