using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MusicLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MediaPlayer mediaPlayer = new MediaPlayer();

        //private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;

        public MainWindow()
        {

            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

        }

        private void MiOpenAudioFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(openFileDialog.FileName));
                mediaPlayer.Play();
            }
        }

        private void MiOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog ofdlg = new Microsoft.Win32.OpenFileDialog();

                ofdlg.DefaultExt = ".mp3";
                ofdlg.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
                Nullable<bool> result = ofdlg.ShowDialog();

                if (result == true)
                {
                    // Open document 
                    String fileName = ofdlg.FileName;
                    this.Title = "  File Open  ";
                    Play(fileName);
                }
            }
            catch (ArgumentException ep)
            {
                Console.WriteLine(ep.StackTrace);
                MessageBox.Show("File open error" + ep.StackTrace);
            }
        }

        private void MiAddToPlayList_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MiExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //if (!_fileChanged) return;
            var result = MessageBox.Show("Do you want to save changes to PlayList?", "MusicPlayer", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    //MiSave_Click(null, null);
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }

        private void BtPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String fileName = (String)lvLibrary.SelectedItem;
                Play(fileName);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("You should select a music" + ex.StackTrace);
            }
        }

        private void Play(String fileName)
        {
            mediaPlayer.Open(new Uri(fileName));
            mediaPlayer.Play();
        }

        private void BtPause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void BtStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

        //0419 adding by cc

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = mediaPlayer.Position.TotalSeconds;
            }

            if (mediaPlayer.Source != null)
                try
                {
                    lblStatus.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                }
                catch (System.InvalidOperationException ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            else
            {
                lblStatus.Content = "";
            }
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mediaPlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
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
                try
                {
                    foreach (string dir in Directory.GetDirectories(_item.Tag.ToString()))
                    {
                        DirectoryInfo _dirinfo = new DirectoryInfo(dir);
                        Populate(_dirinfo.Name, _dirinfo.FullName, null, _item, false);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Cannot access this directory" + ex.StackTrace);
                }

                foreach (string file in Directory.GetFiles(_item.Tag.ToString()))
                {
                    FileInfo _fileinfo = new FileInfo(file);
                    Populate(_fileinfo.Name, _fileinfo.FullName, null, _item, true);
                }
            }
        }

        private void MiImportToLibrary_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem _item = (TreeViewItem)lvDirectory.SelectedItem;
            try
            {
                foreach (string file in Directory.GetFiles(_item.Tag.ToString()))
                {
                    FileInfo _fileinfo = new FileInfo(file);
                    Console.WriteLine(_fileinfo.Name, _fileinfo.FullName);
                    lvLibrary.Items.Add(_fileinfo.FullName);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Invalid directory" + ex.StackTrace);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("You must select a directory" + ex.StackTrace);
            }
        }

        private void lvLibrary_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            String fileName = (String)lvLibrary.SelectedItem;
            Play(fileName);
        }
    }

    public class ImageToHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string tag = (string)value;
            if (File.Exists(tag))
            {
                return "pack://application:,,,/image/file.png";
            }
            if (tag.Length > 3)
            {
                return "pack://application:,,,/image/folder.png";
            }
            else
            {
                return "pack://application:,,,/image/hardDrive.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
