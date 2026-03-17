using System;
using System.Collections.Generic;
using System.IO;
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

namespace Bildverwaltungsprogramm
{
    /// <summary>
    /// Interaktionslogik für MovePics.xaml
    /// </summary>
    public partial class MovePics : Window
    {
        private DirectoryInfo d;
        private string alreadychosen;

        public string chosendir;


        public MovePics(DirectoryInfo d, string alreadychosen)
        {
            InitializeComponent();
            this.d = d;
            this.alreadychosen = alreadychosen;
            foreach (DirectoryInfo dir in d.GetDirectories())
            {
                if (!dir.Name.Equals(alreadychosen))
                {
                    Comboboxalben.Items.Add(dir.Name);
                }
            }
            Comboboxalben.SelectedIndex = 0;
        }

        private void finishbutton_Click(object sender, RoutedEventArgs e)
        {
            chosendir = (string)Comboboxalben.SelectedItem;
            this.DialogResult = true;
        }
    }
}
