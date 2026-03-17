using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaktionslogik für NewAlbum.xaml
    /// </summary>
    public partial class NewAlbum : Window
    {
        public string _albumname { get; set; }


        private DirectoryInfo d;

        public NewAlbum(DirectoryInfo d)
        {
            InitializeComponent();
            this.d = d;
            DataContext = this;
            Labelshow.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _albumname = Albumname.Text;
            this.DialogResult = true;
        }

        private void Albumname_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool hit = false;

            foreach(DirectoryInfo dir in d.GetDirectories())
            {
                if(dir.Name.Equals(Albumname.Text))
                {
                    hit = true; break;
                }
            }

            if(hit)
            {
                Labelshow.Visibility = Visibility.Visible;
                Add_Button.IsEnabled = false;
            } 
            else
            {
                Labelshow.Visibility = Visibility.Hidden;
                Add_Button.IsEnabled = true;
            }
        }
    }
}
