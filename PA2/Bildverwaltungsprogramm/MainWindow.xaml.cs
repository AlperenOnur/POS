using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;

namespace Bildverwaltungsprogramm
{
    public partial class MainWindow : Window
    {
        private DirectoryInfo d;
        private readonly string MAIN_DIR = "../../../Images";

        public MainWindow()
        {
            InitializeComponent();
            UpdateCombobox();
        }

        private void UpdateCombobox()
        {
            d = new DirectoryInfo(MAIN_DIR);
            ComboboxAlben.Items.Clear();
            foreach (DirectoryInfo dir in d.GetDirectories())
            {
                ComboboxAlben.Items.Add(dir.Name);
            }
            if (ComboboxAlben.Items.Count > 0)
            {
                ComboboxAlben.SelectedIndex = 0;
                LoadGalleryImages((string)ComboboxAlben.SelectedItem);
            }
            else
            {
                Gallery.Visibility = Visibility.Hidden;
                Labelleer.Visibility = Visibility.Visible;
            }
        }

        private void LoadGalleryImages(string albumName)
        {
            var dir = new DirectoryInfo(MAIN_DIR + "/" + albumName);
            var images = dir.GetFiles()
                            .Where(f => f.Extension == ".jpg" || f.Extension == ".png")
                            .Select(f => new { FileInfo = f, Image = LoadImage(f.FullName) })
                            .ToList();
            if (images.Count == 0)
            {
                Gallery.Visibility = Visibility.Hidden;
                Labelleer.Visibility = Visibility.Visible;
            }
            else
            {
                Gallery.Visibility = Visibility.Visible;
                Labelleer.Visibility = Visibility.Hidden;
            }
            DataContext = images;
        }

        private void CommandBinding_Executed_New(object sender, ExecutedRoutedEventArgs e)
        {
            d = new DirectoryInfo(MAIN_DIR);
            NewAlbum a = new NewAlbum(d);
            if (a.ShowDialog() == true)
            {
                d.CreateSubdirectory(a._albumname);
                UpdateCombobox();
                LoadGalleryImages(a._albumname);
            }
        }

        private void CommandBinding_Executed_Add(object sender, ExecutedRoutedEventArgs e)
        {
            d = new DirectoryInfo(MAIN_DIR);
            AddPics a = new AddPics(d);
            if (a.ShowDialog() == true)
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(a.zipfile.FullName, MAIN_DIR + "/" + a.chosendir, true);
                DirectoryInfo fixinfo = new DirectoryInfo(MAIN_DIR + "/" + a.chosendir);
                foreach (FileInfo f in fixinfo.GetFiles())
                {
                    if (!(f.Extension.Equals(".png") || f.Extension.Equals(".jpg")))
                    {
                        f.Delete();
                    }
                }
                UpdateCombobox();
                LoadGalleryImages(a.chosendir);
            }
        }

        private void CommandBinding_Executed_Move(object sender, ExecutedRoutedEventArgs e)
        {
            d = new DirectoryInfo(MAIN_DIR);
            var selection = Gallery.SelectedItems.Cast<dynamic>().Select(x => (FileInfo)x.FileInfo).ToList();
            MovePics a = new MovePics(d, (string)ComboboxAlben.SelectedItem);
            if (a.ShowDialog() == true)
            {
                DataContext = null;
                foreach (FileInfo f in selection)
                {
                    File.Move(MAIN_DIR + "/" + (string)ComboboxAlben.SelectedItem + "/" + f.Name, MAIN_DIR + "/" + a.chosendir + "/" + f.Name);
                }
                UpdateCombobox();
                LoadGalleryImages((string)ComboboxAlben.SelectedItem);
            }
        }

        private void CommandBinding_Executed_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            var selection = Gallery.SelectedItems.Cast<dynamic>().Select(x => (FileInfo)x.FileInfo).ToList();
            foreach (FileInfo f in selection)
            {
                f.Delete();
            }
            UpdateCombobox();
            LoadGalleryImages((string)ComboboxAlben.SelectedItem);
        }

        private void CommandBinding_Executed_RotatePlus90(object sender, ExecutedRoutedEventArgs e)
        {
            var selection = Gallery.SelectedItems.Cast<dynamic>().Select(x => (FileInfo)x.FileInfo).ToList();
            foreach (FileInfo f in selection)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = new Uri(f.FullName, UriKind.RelativeOrAbsolute);
                bmp.EndInit();
                bmp.Freeze();

                TransformedBitmap tb = new TransformedBitmap();
                tb.BeginInit();
                tb.Source = bmp;
                tb.Transform = new RotateTransform(90);
                tb.EndInit();
                tb.Freeze();

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(tb));
                using (var stream = new FileStream(f.FullName, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(stream);
                }
            }
            LoadGalleryImages((string)ComboboxAlben.SelectedItem);
        }

        private void CommandBinding_Executed_RotateMinus90(object sender, ExecutedRoutedEventArgs e)
        {
            var selection = Gallery.SelectedItems.Cast<dynamic>().Select(x => (FileInfo)x.FileInfo).ToList();
            foreach (FileInfo f in selection)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = new Uri(f.FullName, UriKind.RelativeOrAbsolute);
                bmp.EndInit();
                bmp.Freeze();

                TransformedBitmap tb = new TransformedBitmap();
                tb.BeginInit();
                tb.Source = bmp;
                tb.Transform = new RotateTransform(-90);
                tb.EndInit();
                tb.Freeze();

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(tb));
                using (var stream = new FileStream(f.FullName, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(stream);
                }
            }
            LoadGalleryImages((string)ComboboxAlben.SelectedItem);
        }

        private void CommandBinding_Executed_Rotate180(object sender, ExecutedRoutedEventArgs e)
        {
            var selection = Gallery.SelectedItems.Cast<dynamic>().Select(x => (FileInfo)x.FileInfo).ToList();
            foreach (FileInfo f in selection)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = new Uri(f.FullName, UriKind.RelativeOrAbsolute);
                bmp.EndInit();
                bmp.Freeze();

                TransformedBitmap tb = new TransformedBitmap();
                tb.BeginInit();
                tb.Source = bmp;
                tb.Transform = new RotateTransform(180);
                tb.EndInit();
                tb.Freeze();

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(tb));
                using (var stream = new FileStream(f.FullName, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(stream);
                }
            }
            LoadGalleryImages((string)ComboboxAlben.SelectedItem);
        }

        private void ComboboxAlben_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboboxAlben.Items.Count > 0)
            {
                LoadGalleryImages((string)ComboboxAlben.SelectedItem);
            }
        }

        private BitmapImage LoadImage(string path)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bmp.EndInit();
            bmp.Freeze();
            return bmp;
        }
    }
}