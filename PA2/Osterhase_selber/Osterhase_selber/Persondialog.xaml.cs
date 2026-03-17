using DataModel;
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

namespace Osterhase_selber
{
    /// <summary>
    /// Interaktionslogik für Persondialog.xaml
    /// </summary>
    public partial class PersonDialog : Window
    {
        public Person p = new Person();
        public PersonDialog()
        {
            InitializeComponent();
            DataContext = p;
        }

        private void OnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnReg_Click(object sender, RoutedEventArgs e)
        {
            if (p.Latitude != null && p.Longitude != null && !string.IsNullOrWhiteSpace(p.Name))
            {
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
            }
            Close();
        }
    }
}
