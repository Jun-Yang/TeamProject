using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MusicLibrary
{

    public partial class MainWindow : Window
    {

        private static string currentFile = "";
        private static int indexbeforeAdd = 0;
        private bool userIsDraggingSlider = false;
        private static bool isLibrary = false;
        private static bool isPlaylist = false;
        private Database db;

        internal static List<Song> ListMusicLibrary = new List<Song>();
        internal static List<Song> ListPlaying = new List<Song>();
        internal static List<PlayList> ListPl = new List<PlayList>();

        public MainWindow()
        {
            InitializeComponent();            
            InitTimer();
            db = new Database();
            ResetAllFields();
            LvLibrary.ItemsSource = ListMusicLibrary;
            LvPlay.ItemsSource = ListPlaying;
            LoadAllSongs();
            RefreshMusicLibrary();
        }

        private void RefreshMusicLibrary()
        {
            LvLibrary.ItemsSource = ListMusicLibrary;
            if (LvLibrary.Items.Count == 0)
            {
                DisablePlayControl();
            }
            else
            {
                LvLibrary.Focus();
                LvLibrary.SelectedIndex = indexbeforeAdd;
                LvLibrary.Items.Refresh();
                EnablePlayControl();
            }
        }

        private void ResetAllFields()
        {
            SliVolume.Value = PlayControl.mediaPlayer.Volume * 4;
        }

        private void DisablePlayControl()
        {
            MiPlay.IsEnabled = false;
            MiStop.IsEnabled = false;
            MiPause.IsEnabled = false;
            MiPrevious.IsEnabled = false;
            MiNext.IsEnabled = false;
            BtPlay.IsEnabled = false;
            BtStop.IsEnabled = false;
            BtNext.IsEnabled = false;
            BtPrevious.IsEnabled = false;
            SliProgress.IsEnabled = false;
        }

        private void EnablePlayControl()
        {
            MiPlay.IsEnabled = true;
            MiStop.IsEnabled = true;
            MiPause.IsEnabled = true;
            MiPrevious.IsEnabled = true;
            MiNext.IsEnabled = true;
            BtPlay.IsEnabled = true;
            BtStop.IsEnabled = true;
            BtNext.IsEnabled = true;
            BtPrevious.IsEnabled = true;
            SliProgress.IsEnabled = true;
        }

        void InitTimer()
        {
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((PlayControl.mediaPlayer.Source != null) && (PlayControl.mediaPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                SliProgress.Minimum = 0;
                SliProgress.Maximum = PlayControl.mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                SliProgress.Value = PlayControl.mediaPlayer.Position.TotalSeconds;
            }

            if (PlayControl.mediaPlayer.Source != null)
                try
                {
                    LblStatus.Content = String.Format("{0} / {1}", PlayControl.mediaPlayer.Position.ToString(@"mm\:ss"), PlayControl.mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                }
                catch (System.InvalidOperationException ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            else
            {
                LblStatus.Content = "";
                return;
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
            TreeViewItem rootItem, childItem;
            int index = 1;

            rootItem = new TreeViewItem()
            {
                Tag = "Playlists",
                Header = "Playlists"
            };
            TvPlaylists.Items.Add(rootItem);
            rootItem.Expanded += new RoutedEventHandler(PlaylistsExpanded);

            foreach (var n in db.GetPlaylistName())
            {
                childItem = new TreeViewItem()
                {
                    Tag = "Playlist" + index
                };
                index++;
                childItem.Header = n;
                rootItem.Items.Add(childItem);
            }
            rootItem.ExpandSubtree();
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
                    MessageBoxEx.Show("Cannot access this directory" + ex.StackTrace);
                }
            }
        }

        private void TvPlaylists_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item = (TreeViewItem)TvPlaylists.SelectedItem;
            string plName = (string)item.Header;
            db.GetPlaylistByName(plName);
            LvPlay.ItemsSource = db.GetMusicByPlaylistName(plName);
        }

        private void PopulateDirectory(string header, string tag, TreeView root, TreeViewItem child, bool isfile)
        {
            TreeViewItem driItem = new TreeViewItem()
            {
                Tag = tag,
                Header = header
            };
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
                    MessageBoxEx.Show("Cannot access this directory" + ex.StackTrace);
                }
            }
        }

        private void TbFilter_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = TbFilter.Text.ToLower();
            if (filter == "")
            {
                LvLibrary.ItemsSource = db.GetAllSongsFromLib();
            }
            else
            {
                List<Song> list = db.GetSongsByTitleArtist(filter);
                ///* var filteredList = list.Where(b => b.Title.ToLower().Contains(filter)
                //                                   || b.Author.ToLower().Contains(filter)); */
                //var filteredList = from s in db.GetAllSongsFromLib()
                //                   where (s.title.ToLower().Contains(filter) || s.name.ToLower().Contains(filter))
                //                   select s;

                LvLibrary.ItemsSource = db.GetSongsByTitleArtist(filter); ;
            }
        }

        private void MiOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                currentFile = openFileDialog.FileName;
                AddMusicToLibrary(currentFile);
                BtStop_Click(null, null);
                PlayControl.mediaPlayer.Open(new Uri(currentFile));
                BtPlay_Click(null, null);
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

            var result = MessageBoxEx.Show("Do you want to save changes to PlayList?", "MusicPlayer", MessageBoxButton.YesNoCancel);
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
                    if (isLibrary)
                    {
                        if (LvLibrary.SelectedItem != null)
                        {
                            LvLibrary.Focus();
                            PlayControl.Play(ImagePlay);
                        }
                        else
                        {
                            MessageBoxEx.Show("You should select a music");
                        }
                    }
                    else if (isPlaylist)
                    {
                        if (LvPlay.SelectedItem != null)
                        {
                            LvPlay.Focus();
                            PlayControl.Play(ImagePlay);
                        }
                        else
                        {
                            MessageBoxEx.Show("You should select a music");
                        }
                    }
                    else
                    {
                        PlayControl.Pause(ImagePlay);
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBoxEx.Show("You should select a music" + ex.StackTrace);
            }
        }

        private void BtStop_Click(object sender, RoutedEventArgs e)
        {
            PlayControl.Stop(ImagePlay);
        }

        private void BtNext_Click(object sender, RoutedEventArgs e)
        {
            if (isLibrary)
            {
                if (LvLibrary.SelectedIndex < LvLibrary.Items.Count - 1)
                {
                    LvLibrary.SelectedIndex++;
                }
                LvLibrary.Focus();
            }
            else if (isPlaylist)
            {
                if (LvPlay.SelectedIndex < LvPlay.Items.Count - 1)
                {
                    LvPlay.SelectedIndex++;
                }
                LvPlay.Focus();
            }
            else
            {
                MessageBox.Show("Internal Error");
                return;
            }
            
            PlayControl.mediaPlayer.Open(new Uri(currentFile));
            PlayControl.Play(ImagePlay);
        }

        private void BtPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (isLibrary)
            {
                if (LvLibrary.SelectedIndex > 0)
                {
                    LvLibrary.SelectedIndex--;
                }
                LvLibrary.Focus();
            }
            else if (isPlaylist)
            {
                if (LvPlay.SelectedIndex > 0)
                {
                    LvPlay.SelectedIndex--;
                }
                LvPlay.Focus();
            }
            else
            {
                MessageBox.Show("Internal Error");
                return;
            }
            PlayControl.mediaPlayer.Open(new Uri(currentFile));
            PlayControl.Play(ImagePlay);
        }

        private void BtSpeaker_Click(object sender, RoutedEventArgs e)
        {
            PlayControl.Speaker(ImageSpeaker);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PlayControl.SetVolume(SliVolume.Value / 4);
        }

        //0420 adding by cc
        private void MenuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxEx.Show("Delete this item?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                if (LvLibrary.SelectedIndex == -1) return;
                else { 
                    ListMusicLibrary.RemoveAt(LvLibrary.SelectedIndex);
                    RefreshMusicLibrary();
                }
            }
        }

        private void MenuItemPlay_Click(object sender, RoutedEventArgs e)
        {
            if (LvLibrary.SelectedIndex != -1)
            {
                EnablePlayControl();
                //currentFile = (String)ListMusicLibrary[LvLibrary.SelectedIndex].PathToFile;
                BtStop_Click(null, null);
                PlayControl.mediaPlayer.Open(new Uri(currentFile));
                BtPlay_Click(null, null);
            }
        }

        //treeview select one song right click -play song
        private void TvMenuPlayMedia_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Play Media?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                TreeViewItem item = (TreeViewItem)tvDirectory.SelectedItem;
                if (File.Exists(item.Tag.ToString()))
                {
                    FileInfo fileInfo = new FileInfo(item.Tag.ToString());
                    AddMusicToLibrary(fileInfo.FullName);
                    if (LvLibrary.SelectedIndex != -1)
                    {
                        currentFile = (String)ListMusicLibrary[LvLibrary.SelectedIndex].PathToFile;
                        EnablePlayControl();
                        BtStop_Click(null, null);
                        PlayControl.mediaPlayer.Open(new Uri(currentFile));
                        BtPlay_Click(null, null);
                    }
                }
                //tvDirectory.SelectedItem.ToString();
            }
        }

        private void TvMenuImportToLibrary_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Import to Library?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                ImportToLibrary_Click(sender, e);
            }
        }

        private void MiImportToLibrary_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.ValidateNames = false;
            //openFileDialog.CheckFileExists = false;
            //openFileDialog.CheckPathExists = true;
            //openFileDialog.FileName = "Folder Selection.";

            if (openFileDialog.ShowDialog() == true)
            {
                currentFile = openFileDialog.FileName;
                AddMusicToLibrary(currentFile);
            }
        }

        private void ImportToLibrary_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)tvDirectory.SelectedItem;
            try
            {
                if (File.Exists(item.Tag.ToString()))
                {
                    FileInfo fileInfo = new FileInfo(item.Tag.ToString());
                    if (IsMusicFile(fileInfo))
                    {
                        AddMusicToLibrary(fileInfo.FullName);
                    }
                    else
                    {
                        MessageBox.Show("Please select a music file");
                        return;
                    }
                }
                else
                {
                    int index = indexbeforeAdd;
                    foreach (string file in Directory.GetFiles(item.Tag.ToString()))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        Console.WriteLine(fileInfo.Name, fileInfo.FullName);
                        if (IsMusicFile(fileInfo))
                        {
                            AddMusicToLibrary(fileInfo.FullName);
                        }
                    }
                    indexbeforeAdd = index;
                }
                if (ListMusicLibrary.Count > 0)
                {
                    currentFile = (String)ListMusicLibrary[indexbeforeAdd].PathToFile;
                    EnablePlayControl();
                    //BtStop_Click(null, null);
                    //PlayControl.mediaPlayer.Open(new Uri(currentFile));
                    //BtPlay_Click(null, null);
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
            RefreshMusicLibrary();
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
            indexbeforeAdd = LvLibrary.Items.Count;
            Song song = new Song(title, strArtist, albumId, (int)sequenceId, description, filePath, year, strGenre, rating);
            ListMusicLibrary.Add(song);
            RefreshMusicLibrary();
        }

        internal static bool IsMusicFile(FileInfo info)
        {
            string type = info.Extension;
            switch (type.ToUpper())
            {
                case ".MP3":
                case ".WMA": return true;
                default: return false;
            }
        }

        private void LvLibrary_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LvLibrary.SelectedIndex != -1)
            {
                currentFile = (String)ListMusicLibrary[LvLibrary.SelectedIndex].PathToFile;
                isLibrary = true;
                isPlaylist = false;
                PlayControl.mediaPlayer.Open(new Uri(currentFile));
                PlayControl.Play(ImagePlay);
            }
        }

        private void LvLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (LvLibrary.SelectedIndex != -1)
            {
                try
                {
                    currentFile = (String)ListMusicLibrary[LvLibrary.SelectedIndex].PathToFile;
                    isLibrary = true;
                    isPlaylist = false;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Music Library Selection Changed Error", ex.StackTrace);
                }
            }
        }

        private void LvDirectoryMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void SliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            PlayControl.mediaPlayer.Position = TimeSpan.FromSeconds(SliProgress.Value);
        }

        private void SliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PlayControl.mediaPlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void MiEditClear_Click(object sender, RoutedEventArgs e)
        {
            LvLibrary.ItemsSource = null;
            LvLibrary.Items.Clear();
        }

        private void MiPlayBackPlay_Click(object sender, RoutedEventArgs e)
        {
            BtPlay_Click(sender, e);
            //if (LvLibrary.SelectedIndex != -1)
            //{
            //    currentFile = (String)ListMusicLibrary[LvLibrary.SelectedIndex].PathToFile;
            //    PlayControl.mediaPlayer.Open(new Uri(currentFile));
            //    PlayControl.Play(ImagePlay);
            //}
        }

        private void MiPlaybackPause_Click(object sender, RoutedEventArgs e)
        {
            PlayControl.Pause(ImagePlay);
        }

        private void MiPlaybackStop_Click(object sender, RoutedEventArgs e)
        {
            PlayControl.Stop(ImagePlay);
        }

        private void MiPlaybackNext_Click(object sender, RoutedEventArgs e)
        {
            BtNext_Click(sender,e);
        }

        private void MiPlaybackPrevious_Click(object sender, RoutedEventArgs e)
        {
            BtPrevious_Click(sender,e);
           
        }

        //Adding by Chen 0426
        private void MenuItemProperty_Click(object sender, RoutedEventArgs e)
        {
            if (LvLibrary.SelectedIndex != -1)
            {

                Song song = (Song)ListMusicLibrary[LvLibrary.SelectedIndex];
                MediaProperty mediaProperty = new MediaProperty(song);

                mediaProperty.Top = (this.Top + (this.Height / 2)) - mediaProperty.Height / 2;
                mediaProperty.Left = (this.Left + (this.Width / 2)) - mediaProperty.Width / 2;
                mediaProperty.Show();
            }

        }

        //add by chenchen 0423
        private void LoadAllSongs()
        {
            foreach (Song s in db.GetAllSongsFromLib())
            {
                ListMusicLibrary.Add(s);
            }
        }

        private void MiEditSort_Click(object sender, RoutedEventArgs e)
        {
            //read media information from database 
            LoadAllSongs();
            RefreshMusicLibrary();
        }

        // Drag and drop from directory to music library
        private void TvDirectory_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Get the dragged TreeViewItem
                TreeView treeView = sender as TreeView;
                TreeViewItem item = (TreeViewItem)tvDirectory.SelectedItem;

                try
                {
                    string fileName = (string)item.Header;
                    FileInfo fileInfo = new FileInfo(item.Tag.ToString());
                    if (IsMusicFile(fileInfo))
                    {
                        Console.WriteLine("drag filename is " + fileName + " fileInfo.fileName is " + fileInfo.Name);
                        DataObject dataObject = new DataObject("myFormat", item);
                        DragDrop.DoDragDrop(item, dataObject, DragDropEffects.Move);
                    }
                    else {
                        MessageBox.Show("Please choose a music file");
                    }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine("Drag drop exception 770 " + ex.StackTrace);
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine("TvDirectory_MouseMove ArgumentNullException 801" + ex.StackTrace);
                }
            }
        }
        
        private void LvLibrary_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void LvLibrary_Drop(object sender, DragEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)tvDirectory.SelectedItem;
            
            if (e.Data.GetDataPresent("myFormat"))
            {
                try
                {
                    if (File.Exists(item.Tag.ToString()))
                    {
                        FileInfo fileInfo = new FileInfo(item.Tag.ToString());
                        Console.WriteLine(fileInfo.Name, fileInfo.FullName);
                        AddMusicToLibrary(fileInfo.FullName);

                        if (LvLibrary.SelectedIndex != -1)
                        {
                            currentFile = (String)ListMusicLibrary[LvLibrary.SelectedIndex].PathToFile;
                            RefreshMusicLibrary();
                        }
                    }
                }
                catch (System.NullReferenceException ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                
            }
        }

        // Drag and drop from music library to playlist

        private void LvLibrary_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)                
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);

                // Find the data behind the ListViewItem
                if (listViewItem != null)
                {
                    Song song = (Song)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("myFormat", song);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void LvPlay_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void LvPlay_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Song song = e.Data.GetData("myFormat") as Song;
                ListPlaying.Add(song);
                LvPlay.Items.Refresh();
            }
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            using (AboutBox box = new AboutBox())
            {
                box.ShowDialog();
            }
        }

        private void LvPlay_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LvPlay.SelectedIndex != -1)
            {
                currentFile = (String)ListPlaying[LvPlay.SelectedIndex].PathToFile;
                isPlaylist = true;
                isLibrary = false;
                PlayControl.mediaPlayer.Open(new Uri(currentFile));
                PlayControl.Play(ImagePlay);
            }
        }

        private void LvPlay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvPlay.SelectedIndex != -1)
            {
                try
                {
                    currentFile = (String)ListPlaying[LvPlay.SelectedIndex].PathToFile;
                    isPlaylist = true;
                    isLibrary = false;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Music Library Selection Changed Error", ex.StackTrace);
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
                string fileExt = Path.GetExtension(tag).ToUpper();
                if (fileExt == ".MP3" || fileExt == ".WMA")
                {
                    return "pack://application:,,,/image/music.png";
                }
                else
                {
                    return "pack://application:,,,/image/file.png";
                }
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
