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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //WindowStartupLocation = WindowStartupLocation.CenterScreen;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void MiOpen_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MiSave_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MiSaveAs_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MiExit_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Populate(string header, string tag, TreeView _root, TreeViewItem _child, bool isfile)
        {
            TreeViewItem _driitem = new TreeViewItem();
            _driitem.Tag = tag;
            _driitem.Header = header;
            _driitem.Expanded += new RoutedEventHandler(_driitem_Expanded);
            if (!isfile)
                _driitem.Items.Add(new TreeViewItem());

            if (_root != null)
            { _root.Items.Add(_driitem); }
            else { _child.Items.Add(_driitem); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (DriveInfo driv in DriveInfo.GetDrives())
            {
                if (driv.IsReady)
                    Populate(driv.VolumeLabel + "(" + driv.Name + ")", driv.Name, lvDirectory, null, false);
            }
        }


        void _driitem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem _item = (TreeViewItem)sender;
            if (_item.Items.Count == 1 && ((TreeViewItem)_item.Items[0]).Header == null)
            {
                _item.Items.Clear();
                foreach (string dir in Directory.GetDirectories(_item.Tag.ToString()))
                {
                    DirectoryInfo _dirinfo = new DirectoryInfo(dir);
                    Populate(_dirinfo.Name, _dirinfo.FullName, null, _item, false);
                }

                foreach (string dir in Directory.GetFiles(_item.Tag.ToString()))
                {
                    FileInfo _dirinfo = new FileInfo(dir);
                    Populate(_dirinfo.Name, _dirinfo.FullName, null, _item, true);
                }

            }
        }

        private void folders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is TreeViewItem && ((TreeViewItem)e.Source).IsSelected)
            {

            }
        }

        private void BalanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }

    public class ImageToHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string tag = (string)value;
            if (File.Exists(tag))
            {
                return "file.png";
            }
            if (tag.Length > 3)
            {
                return "folder.png";
            }
            else
            {
                return "HardDrive.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        
    }


}
