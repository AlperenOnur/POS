using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Uebung_1.PA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            MapBox.SelectedValuePath = "Value";
            MapBox.DisplayMemberPath = "Key";
            
            MapBox.Items.Add(new KeyValuePair<string, string>("Map 1", "./ressources/Map1.xml"));
            MapBox.Items.Add(new KeyValuePair<string, string>("Map 2", "./ressources/Map2.xml"));
            MapBox.SelectedIndex = 0;


            CodeBox.SelectedValuePath = "Value";
            CodeBox.DisplayMemberPath = "Key";

            CodeBox.Items.Add(new KeyValuePair<string, string>("Code 1", "pack://application:,,,/ressources/Code1.txt"));
            CodeBox.Items.Add(new KeyValuePair<string, string>("Code 2", "pack://application:,,,/ressources/Code2.txt"));
            CodeBox.SelectedIndex = 0;
        }

        private void MapBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            robotField.LoadField(((KeyValuePair<string, string>)MapBox.SelectedItem).Value);
        }

        private void CodeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var uri = new Uri(
                ((KeyValuePair<string, string>)CodeBox.SelectedItem).Value);
            var resource = Application.GetResourceStream(uri);
            using (var reader = new StreamReader(resource.Stream))
            {
                Input.Text = reader.ReadToEnd();
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            robotField.LoadField(((KeyValuePair<string, string>)MapBox.SelectedItem).Value);

            string input = Input.Text;

            var global = new Regex(@"(\bMOVE\b|\bCOLLECT\b|\bREPEAT\b|\bUNTIL\b|\bIS-A\b|\bIF\b)|(\bDOWN\b|\bUP\b|\bRIGHT\b|\bLEFT\b)|\bOBSTACLE\b|\b[A-Z]\b|(\{|\})|\b\d+\b|\s+|\S+");
            var direction = new Regex(@"(\bDOWN\b|\bUP\b|\bRIGHT\b|\bLEFT\b)");
            var letter = new Regex(@"\b[A-Z]\b");
            var obstacle = new Regex(@"\bOBSTACLE\b");
            var klammer = new Regex(@"(\{|\})");
            var keyword = new Regex(@"(\bMOVE\b|\bCOLLECT\b|\bREPEAT\b|\bUNTIL\b|\bIS-A\b|\bIF\b)");
            var number = new Regex(@"\b(\d+)\b");
            var ws = new Regex(@"\s");

            var collection = global.Matches(input);
            List<Token> tokenlist = new List<Token>();
            foreach (Match match in collection)
            {
                Token t = new Token();
                if (ws.Match(match.Value).Success)
                {
                    continue;
                }
                else if(keyword.Match(match.Value).Success)
                {
                    t.type = Token.Type.Keyword;
                }
                else if (direction.Match(match.Value).Success)
                {
                    t.type = Token.Type.Direction;
                }
                else if (klammer.Match(match.Value).Success)
                {
                    t.type = Token.Type.Klammer;
                }
                else if (obstacle.Match(match.Value).Success)
                {
                    t.type = Token.Type.Obstacle;
                }
                else if (letter.Match(match.Value).Success)
                {
                    t.type = Token.Type.Letter;
                }
                else if (number.Match(match.Value).Success)
                {
                    t.type = Token.Type.Nummer;
                }
                else 
                {
                    t.type = Token.Type.Error;
                }
                t.text = match.Value;
                tokenlist.Add(t);
            }
            Program program = new Program();

            program.Parse(ref tokenlist);
            if (Anweisung.Errors.Count > 0)
            {

                string message = string.Join(Environment.NewLine, Anweisung.Errors);

                MessageBox.Show(
                    message,
                    "Fehler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                Anweisung.Errors.Clear();
                return;
            }
            else
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    if (!program.Run(this))
                    {
                        MessageBox.Show(
                            "Gegen Hindernis gestoßen",
                            "Fehler",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                });
                
            }

        }
    }
}