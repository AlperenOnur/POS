using Microsoft.Win32;
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
    /// Interaktionslogik für AddPics.xaml
    /// </summary>
    public partial class AddPics : Window
    {
        public FileInfo zipfile;
        public string chosendir;

        private DirectoryInfo d;

        public AddPics(DirectoryInfo d)
        {
            InitializeComponent();
            this.d = d;

            foreach(DirectoryInfo dir in d.GetDirectories())
            {
                ComboAlben.Items.Add(dir.Name);
            }
        }

        private void choosezipbutton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ZIP Folders (.ZIP)|*.zip";


            if(ofd.ShowDialog() == true)
            {
                zipfile = new FileInfo(ofd.FileName);

                showlabel.Content = "Gewählter Zip-Ordner: " + zipfile.Name;
            }
        }

        private void finishbutton_Click(object sender, RoutedEventArgs e)
        {
            if(ComboAlben.SelectedItem == null)
            {
                MessageBox.Show("Wählen Sie bitte ein Zielalbum aus!");
                return;
            }
            chosendir = (string)ComboAlben.SelectedItem;
            this.DialogResult = true;
        }
    }
}
