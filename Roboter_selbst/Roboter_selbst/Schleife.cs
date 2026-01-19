using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Roboter_selbst
{
    internal class Schleife : Anweisung
    {
        RobotField.Direction dir;
        string l;
        bool obstacle = false;
        Block b = new Block();
        public override void Parse(ref List<Token> tokenList)
        {
            tokenList.RemoveAt(0);
            Token t = tokenList[0];
            if (t.type != Token.Type.Direction)
            {
                Anweisung.Errors.Add("Direction fehlt");
                if (t.type == Token.Type.Error)
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
                tokenList.RemoveAt(0);
            }

            t = tokenList[0];

            if (!t.text.Equals("IS-A"))
            {
                Anweisung.Errors.Add("IS-A fehlt");
                if (tokenList[0].type == Token.Type.Error)
                {
                    Anweisung.Errors.Add("Unexpected: " + tokenList[0].text);

                    tokenList.RemoveAt(0);
                }
            }
            else
            {
                tokenList.RemoveAt(0);
                t = tokenList[0];
            }
            if (t.type == Token.Type.Obstacle)
            {
                obstacle = true;
                tokenList.RemoveAt(0);
            }
            else if (t.type == Token.Type.Letter)
            {
                l = t.text;
                tokenList.RemoveAt(0);
            }
            else
            {
                Anweisung.Errors.Add("Obstacle oder Letter fehlt");
                if (t.type == Token.Type.Error)
                {
                    Anweisung.Errors.Add("Unexpected: " + tokenList[0].text);

                    tokenList.RemoveAt(0);
                }
            }


            b.Parse(ref tokenList);

        }

        public override bool Run(MainWindow mw)
        {
            if ((obstacle))
            {
                while (!mw.robotField.IsObstacle(dir))
                {
                    if (!b.Run(mw))
                    {
                        return false;
                    }
                }
            }
            else
            {
                while (!mw.robotField.IsLetter(l, dir))
                {
                    if (!b.Run(mw))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
