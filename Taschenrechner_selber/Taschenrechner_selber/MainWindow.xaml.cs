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

namespace Taschenrechner_selber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Equals("="))
            {
                ButtonGleich_Click(null, null);
                e.Handled = true; // Event als verarbeitet markieren
                return;
            }

            int caret = Input.CaretIndex;
            Input.Text = Input.Text.Insert(caret, e.Text);
            Input.CaretIndex = caret + e.Text.Length;

            e.Handled = true; // wichtig, damit das TextBox-Standardverhalten nicht noch einmal eingreift
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Back)
            {
                ButtonReturn_Click(null, null);
                e.Handled = true;
            }
        }

        private void ButtonKAUF_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "("; 
        }

        private void ButtonKZU_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += ")";
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "7";
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "4";
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "1";
        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "8";
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "5";
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "2";
        }

        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "0";
        }

        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "9";
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "6";
        }

        private void ButtonHoch_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "^";
        }


        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "3";
        }

        private void ButtonKomma_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += ",";
        }

        private void ButtonDividiert_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "/";
        }

        private void ButtonMal_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "*";
        }

        private void ButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "-";
        }

        private void ButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += "+";
        }

        private void ButtonGleich_Click(object sender, RoutedEventArgs e)
        {
            List<Token> tokens = new List<Token>();
            Regex globaleRegex = new Regex(@"\d[A-Za-z]|[A-Za-z]\d|[A-Za-z]+|(\d+(,\d+)?)|[A-Za-z]|(\+|\-)|(\*|\/)|(\^)|(\(|\))|\s|.");
            Regex numberRegex = new Regex(@"(\d+(,\d+)?)");
            Regex bracketRegex = new Regex(@"(\(|\))");
            Regex variableRegex = new Regex("[A-Za-z]");
            Regex wsRegex = new Regex("\\s");
            Regex pmRegex = new Regex(@"(\+|\-)");
            Regex mdRegex = new Regex(@"(\*|\/)");
            Regex hochRegex = new Regex(@"(\^)");
            Regex invalidNumberVariable = new Regex(@"\d[A-Za-z]|[A-Za-z]\d|[A-Za-z]{2,}");

            MatchCollection coll = globaleRegex.Matches(Input.Text);

            foreach (Match m in coll)
            {
                Token t = new Token();
                if (invalidNumberVariable.Match(m.Value).Success)
                {
                    t.Type = TokenType.Error;
                }
                else if (numberRegex.Match(m.Value).Success)
                {
                    t.Type = TokenType.Number;
                }
                else if (bracketRegex.Match(m.Value).Success)
                {
                    t.Type = TokenType.Bracket;
                }
                else if (variableRegex.Match(m.Value).Success)
                {
                    t.Type = TokenType.Variable;
                }
                else if (pmRegex.Match(m.Value).Success)
                {
                    t.Type = TokenType.pmOperator;
                }
                else if (mdRegex.Match(m.Value).Success)
                {
                    t.Type = TokenType.mdOperator;
                }
                else if (hochRegex.Match(m.Value).Success)
                {
                    t.Type = TokenType.hOperator;
                }
                else if(wsRegex.Match(m.Value).Success)
                {
                    continue;
                }
                else
                {
                    t.Type = TokenType.Error;
                }
                t.Text = m.Value;
                tokens.Add(t);
            }
            PlusMinus term = new PlusMinus();
            term.Parse(ref tokens);

            if (Anweisung.errors.Count != 0)
            {
                string message = string.Join(Environment.NewLine, Anweisung.errors);

                MessageBox.Show(
                    message,
                    "Fehler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                Anweisung.errors.Clear();
                return;
            }
            if (Anweisung.variables.Count != 0)
            {
                var dlg = new VariableDialog(Anweisung.variables.Keys)
                {
                    Owner = Application.Current.MainWindow
                };
                if(dlg.ShowDialog() == true)
                {
                    foreach(var v in dlg.Result)
                    {
                        Anweisung.variables[v.Key] = v.Value;
                    }
                }
            }

            var x = term.Run();

            Input.Text = x.ToString();
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            int caret = Input.CaretIndex;
            if (caret > 0)
            {
                Input.Text = Input.Text.Remove(caret - 1, 1);
                Input.CaretIndex = caret - 1;
            }
        }
    }
}