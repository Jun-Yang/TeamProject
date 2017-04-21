using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MusicLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        bool isPlaying = false;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private static string currentFile = "";

        //private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        static List<Song> ListMusicLibrary = new List<Song>();
        static List<Song> PlayingList = new List<Song>();
        static List<PlayList> PList = new List<PlayList>();


        public MainWindow()
        {

            InitializeComponent();
            lvLibrary.ItemsSource = ListMusicLibrary;
            RefreshMusicLibrary();

            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void GetSongsFromLib()
        {
            //using (RemoteLibrary ctx = new RemoteLibrary())
            //{
            //    ctx.ListMusicLibrary.Add(s);
            //    ctx.SaveChanges();

            //    var lstMusic = (from r in ctx.ListMusicLibrary select r).ToList<Song>();
            //    foreach (var s in lstMusic)
            //    {
            //        Console.WriteLine("P: {0}, {1}, {2}", s.Id, s.Title, s.);
            //    }

            //}
        }

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

        private void RefreshMusicLibrary()
        {
            lvLibrary.ItemsSource = ListMusicLibrary;
            lvLibrary.Items.Refresh();
            //ResetAllFields();
        }

        private void TbFilter_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //string filter = tbFilter.Text.ToLower();
            //if (filter == "")
            //{
            //    lvLibrary.ItemsSource = GetAllSongs();
            //}
            //else
            //{
            //    List<Song> list = GetAllSongs();
            //    /* var filteredList = list.Where(b => b.Title.ToLower().Contains(filter)
            //                                       || b.Author.ToLower().Contains(filter)); */
            //    var filteredList = from b in list where b.Description.ToLower().Contains(filter) select b;

            //    lvLibrary.ItemsSource = filteredList;
            //}
        }

        private List<Song> GetAllSongs()
        {
            throw new NotImplementedException();
        }

        private void MiOpenAudioFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                currentFile = openFileDialog.FileName;
                mediaPlayer.Open(new Uri(currentFile));
                Play(currentFile);
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
                    currentFile = ofdlg.FileName;
                    this.Title = "  File Open  ";
                    mediaPlayer.Open(new Uri(currentFile));
                    Play(currentFile);
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
                if (!isPlaying)
                {
                    if (lvLibrary.SelectedItem != null)
                    {
                        currentFile = (String)ListMusicLibrary[lvLibrary.SelectedIndex].PathToFile;
                        Play(currentFile);
                    }
                    else
                    {
                        MessageBox.Show("You should select a music");
                    }
                }
                else
                {
                    Pause();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("You should select a music" + ex.StackTrace);
            }
        }

        private void Play(String fileName)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("pack://application:,,,/image/pause.png");
            img.EndInit();
            ImagePlay.Source = img;
            mediaPlayer.Play();
            isPlaying = true;
            return;
        }

        private void Pause()
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("pack://application:,,,/image/play.png");
            img.EndInit();
            ImagePlay.Source = img;
            mediaPlayer.Pause();
            isPlaying = false;
            return;
        }

        private void BtStop_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("pack://application:,,,/image/play.png");
            img.EndInit();
            ImagePlay.Source = img;
            isPlaying = false;
            mediaPlayer.Stop();
        }

        private void BtForward_Click(object sender, RoutedEventArgs e)
        {
            if (lvLibrary.SelectedIndex < lvLibrary.Items.Count - 1)
            {
                lvLibrary.SelectedIndex++;
            }
            currentFile = (String)ListMusicLibrary[lvLibrary.SelectedIndex].PathToFile;
            Play(currentFile);
        }

        private void BtBackward_Click(object sender, RoutedEventArgs e)
        {
            if (lvLibrary.SelectedIndex > 0)
            {
                lvLibrary.SelectedIndex--;
            }
            currentFile = (String)ListMusicLibrary[lvLibrary.SelectedIndex].PathToFile;

            Play(currentFile);
        }

        private void BtLoud_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("pack://application:,,,/image/play.png");
            img.EndInit();
            ImagePlay.Source = img;
            isPlaying = false;
            mediaPlayer.Stop();
        }
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = VolumeSlider.Value / 4;
        }

        //0420 adding by cc
        private void MenuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete this item?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                if (lvLibrary.SelectedIndex == -1) return;

                lvLibrary.Items.RemoveAt(lvLibrary.SelectedIndex);
            }
        }

        private void MiImportToLibrary_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)lvDirectory.SelectedItem;
            try
            {
                foreach (string file in Directory.GetFiles(item.Tag.ToString()))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    Console.WriteLine(fileInfo.Name, fileInfo.FullName);
                    if (IsMusicFile(fileInfo))
                    {
                        var musicFile = TagLib.File.Create(fileInfo.FullName);

                        string title = musicFile.Tag.Title;
                        string[] artist = musicFile.Tag.AlbumArtists;
                        string album = musicFile.Tag.Album;
                        int albumId = 1;
                        uint sequenceId = musicFile.Tag.Track;
                        string description = musicFile.Tag.Comment;
                        string filePath = fileInfo.FullName;
                        uint year = musicFile.Tag.Year;
                        string[] genre = musicFile.Tag.Genres;
                        int rating = 0;
                        Song song = new Song(title, "", albumId, (int)sequenceId, description, filePath, 2000, "", rating);
                        ListMusicLibrary.Add(song);
                    }
                }
                RefreshMusicLibrary();
                currentFile = (String)ListMusicLibrary[0].PathToFile;
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

        private bool IsMusicFile(FileInfo info)
        {
            string type = info.Extension;
            switch (type.ToUpper())
            {
                case ".MP3":
                case ".WMA": return true;
                default: return false;
            }
        }

        private void lvLibrary_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            currentFile = (String)ListMusicLibrary[lvLibrary.SelectedIndex].PathToFile;
            mediaPlayer.Open(new Uri(currentFile));
            Play(currentFile);
        }

        private void lvLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lvDirectoryMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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
            //lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mediaPlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void Populate(string header, string tag, TreeView root, TreeViewItem child, bool isfile)
        {
            TreeViewItem driItem = new TreeViewItem();
            driItem.Tag = tag;
            driItem.Header = header;
            driItem.Expanded += new RoutedEventHandler(DriItemExpanded);
            if (!isfile)
                driItem.Items.Add(new TreeViewItem());

            if (root != null)
            {
                root.Items.Add(driItem);
            }
            else
            {
                child.Items.Add(driItem);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (DriveInfo driv in DriveInfo.GetDrives())
            {
                if (driv.IsReady)
                    Populate(driv.VolumeLabel + "(" + driv.Name + ")", driv.Name, lvDirectory, null, false);
            }
        }

        void DriItemExpanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && ((TreeViewItem)item.Items[0]).Header == null)
            {
                item.Items.Clear();
                try
                {
                    foreach (string dir in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(dir);
                        Populate(dirInfo.Name, dirInfo.FullName, null, item, false);
                    }

                    foreach (string file in Directory.GetFiles(item.Tag.ToString()))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        Populate(fileInfo.Name, fileInfo.FullName, null, item, true);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Cannot access this directory" + ex.StackTrace);
                }
            }
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
