using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Taschenrechner_selber
{
    /// <summary>
    /// Interaktionslogik für VariableDialog.xaml
    /// </summary>
    public partial class VariableDialog : Window
    {
        private Dictionary<string, TextBox> inputs  = new();
        public Dictionary<string, double> Result { get; } = new();
        public VariableDialog(IEnumerable<string> variables)
        {
            InitializeComponent();
            
            foreach(var v in variables)
            {
                var row = new StackPanel { Orientation = Orientation.Horizontal };

                row.Children.Add(new Label { Content = v, Width = 60 });

                TextBox tb = new TextBox
                {
                    Width = 100,
                    Text = "0"
                };
                row.Children.Add(tb);

                Panel.Children.Insert(Panel.Children.Count - 1, row);
                inputs[v] = tb;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach(var kv in inputs)
            {
                if(string.IsNullOrWhiteSpace(kv.Value.Text))
                {
                    MessageBox.Show("Eingabe darf nicht leer sein");
                    return;
                }

                if(!double.TryParse(kv.Value.Text, out double value))
                {
                    MessageBox.Show($"Ungültiger Wert für {kv.Key}");
                    return;
                }
                Result[kv.Key] = value;
            }
            DialogResult = true;
            Close();
        }
    }
}
