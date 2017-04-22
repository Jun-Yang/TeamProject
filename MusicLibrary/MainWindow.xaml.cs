﻿using System;
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

        private static string currentFile = "";

        private bool userIsDraggingSlider = false;
        static List<Song> ListMusicLibrary = new List<Song>();
        static List<Song> PlayingList = new List<Song>();
        static List<PlayList> PList = new List<PlayList>();
        private Database db;

        public MainWindow()
        {

            InitializeComponent();
            lvLibrary.ItemsSource = ListMusicLibrary;
            RefreshMusicLibrary();
            InitTimer();
            db = new Database();
        }

        private void RefreshMusicLibrary()
        {
            lvLibrary.ItemsSource = ListMusicLibrary;
            lvLibrary.Items.Refresh();
            //ResetAllFields();
        }

        void InitTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((PlayControl.mediaPlayer.Source != null) && (PlayControl.mediaPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = PlayControl.mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = PlayControl.mediaPlayer.Position.TotalSeconds;
            }

            if (PlayControl.mediaPlayer.Source != null)
                try
                {
                    lblStatus.Content = String.Format("{0} / {1}", PlayControl.mediaPlayer.Position.ToString(@"mm\:ss"), PlayControl.mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
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
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (DriveInfo driv in DriveInfo.GetDrives())
            {
                if (driv.IsReady)
                    PopulateDirectory(driv.VolumeLabel + "(" + driv.Name + ")", driv.Name, tvDirectory, null, false);
            }

            PopulatePlaylists();
        }

        private void PopulatePlaylists()
        {
            TreeViewItem rootItem,childItem;
            int index = 1;

            rootItem = new TreeViewItem();
            rootItem.Tag = "Playlists";
            rootItem.Header = "Playlists";
            tvPlaylists.Items.Add(rootItem);
            rootItem.Expanded += new RoutedEventHandler(PlaylistsExpanded);
         
            foreach (var n in db.GetPlaylistName())
            {
                childItem = new TreeViewItem();
                childItem.Tag = "Playlist" + index;
                index++;
                childItem.Header = n;
                rootItem.Items.Add(childItem);
            }
        }

        void PlaylistsExpanded(object sender, RoutedEventArgs e)
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
                        PopulateDirectory(dirInfo.Name, dirInfo.FullName, null, item, false);
                    }

                    foreach (string file in Directory.GetFiles(item.Tag.ToString()))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        PopulateDirectory(fileInfo.Name, fileInfo.FullName, null, item, true);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Cannot access this directory" + ex.StackTrace);
                }
            }
        }

        private void tvPlaylists_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item = (TreeViewItem)tvPlaylists.SelectedItem;
            string plName = (string)item.Header;
            lvPlay.ItemsSource = db.GetMusicByPlaylistName(plName);
        }

        private void PopulateDirectory(string header, string tag, TreeView root, TreeViewItem child, bool isfile)
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
                        PopulateDirectory(dirInfo.Name, dirInfo.FullName, null, item, false);
                    }

                    foreach (string file in Directory.GetFiles(item.Tag.ToString()))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        PopulateDirectory(fileInfo.Name, fileInfo.FullName, null, item, true);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Cannot access this directory" + ex.StackTrace);
                }
            }
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

        private void MiOpenAudioFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                currentFile = openFileDialog.FileName;
                AddMusicToLibrary(currentFile);
                BtStop_Click(null,null);
                PlayControl.mediaPlayer.Open(new Uri(currentFile));
                BtPlay_Click(null, null);
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
                    AddMusicToLibrary(currentFile);
                    BtStop_Click(null, null);
                    PlayControl.mediaPlayer.Open(new Uri(currentFile));
                    BtPlay_Click(null, null);
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
                if (!PlayControl.isPlaying)
                {
                    if (lvLibrary.SelectedItem != null)
                    {
                        PlayControl.Play(ImagePlay);
                    }
                    else
                    {
                        MessageBox.Show("You should select a music");
                    }
                }
                else
                {
                    PlayControl.Pause(ImagePlay);
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("You should select a music" + ex.StackTrace);
            }
        }

        private void BtStop_Click(object sender, RoutedEventArgs e)
        {
            PlayControl.Stop(ImagePlay);
        }

        private void BtForward_Click(object sender, RoutedEventArgs e)
        {
            if (lvLibrary.SelectedIndex < lvLibrary.Items.Count - 1)
            {
                lvLibrary.SelectedIndex++;
            }
            lvLibrary.Focus();
            PlayControl.mediaPlayer.Open(new Uri(currentFile));
            PlayControl.Play(ImagePlay);
        }

        private void BtBackward_Click(object sender, RoutedEventArgs e)
        {
            if (lvLibrary.SelectedIndex > 0)
            {
                lvLibrary.SelectedIndex--;
            }
            lvLibrary.Focus();
            PlayControl.mediaPlayer.Open(new Uri(currentFile));
            PlayControl.Play(ImagePlay);
        }

        private void BtSpeaker_Click(object sender, RoutedEventArgs e)
        {
            PlayControl.Speaker(ImageSpeaker);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PlayControl.SetVolume(VolumeSlider.Value / 4);
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
                ListMusicLibrary.RemoveAt(lvLibrary.SelectedIndex);
                RefreshMusicLibrary();
            }
        }

        private void ctMenuPlayMedia_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Play Media?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                MiImportToLibrary_Click(sender, e);
            }
        }

        private void ctMenuImportToLibrary_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Import to Library?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                MiImportToLibrary_Click(sender, e);
            }
        }

        private void MiImportToLibrary_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)tvDirectory.SelectedItem;
            try
            {
                foreach (string file in Directory.GetFiles(item.Tag.ToString()))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    Console.WriteLine(fileInfo.Name, fileInfo.FullName);
                    if (IsMusicFile(fileInfo))
                    {
                        AddMusicToLibrary(fileInfo.FullName);
                    }
                }
                if (ListMusicLibrary.Count > 0)
                {
                    RefreshMusicLibrary();
                    currentFile = (String)ListMusicLibrary[0].PathToFile;
                }
                else
                {
                    MessageBox.Show("No music file found in directory");
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

        private void AddMusicToLibrary(string filename)
        {
            var musicFile = TagLib.File.Create(filename);

            string title = musicFile.Tag.Title;
            string[] artist = musicFile.Tag.AlbumArtists;
            string strArtist = null;
            if (artist == null || artist.Length == 0)
            {
                strArtist = "";
            }
            else
            {
                strArtist = string.Join(",", artist);
            }
            string album = musicFile.Tag.Album;
            int albumId = 1;
            uint sequenceId = musicFile.Tag.Track;
            string description = musicFile.Tag.Comment;
            string filePath = filename;
            uint year = musicFile.Tag.Year;
            string[] genre = musicFile.Tag.Genres;
            string strGenre = null;
            if (strGenre == null || strGenre.Length == 0)
            {
                strGenre = "";
            }
            else
            {
                strGenre = string.Join(",", genre);
            }
            int rating = 0;
            Song song = new Song(title, strArtist, albumId, (int)sequenceId, description, filePath, year, strGenre, rating);
            ListMusicLibrary.Add(song);
            lvLibrary.Focus();
            lvLibrary.SelectedIndex = lvLibrary.Items.Count - 1; 
            lvLibrary.Items.Refresh();
            
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
            PlayControl.mediaPlayer.Open(new Uri(currentFile));
            PlayControl.Play(ImagePlay);
        }

        private void lvLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentFile = (String)ListMusicLibrary[lvLibrary.SelectedIndex].PathToFile;
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
            PlayControl.mediaPlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PlayControl.mediaPlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
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
