using System;
using System.Windows;

namespace MusicLibrary
{
    /// <summary>
    /// Interaction logic for MediaProperty.xaml
    /// </summary>
    public partial class MediaProperty : Window
    {
        Song song;
        public MediaProperty(Song s)
        {
            InitializeComponent();
            song = s;
        }

        private void MediaProperty_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //var result = MessageBoxEx.Show("Do you want to save change?", "MusicPlayer", MessageBoxButton.YesNoCancel);
            //switch (result)
            //{
            //    case MessageBoxResult.Yes:
            //        //BtSave_Click(null, null);
            //        break;
            //    case MessageBoxResult.No:
            //        break;
            //    case MessageBoxResult.Cancel:
            //        e.Cancel = true;
            //        return;
            //}
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PropertyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //song = (Song)ListMusicLibrary[lvLi];
            if (song == null)
            {
                MessageBox.Show("PropertyWindow load Error media is null");
                return;
            }
            else
            {
                tbSongTitle.Text = song.Title;
                tbArtistName.Text = song.ArtistName;
                tbAlbumName.Text = song.AlbumId + "";
                tbAlbumId.Text = song.AlbumId + "";
                tbSequenceId.Text = song.SequenceId + "";
                tbPath.Text = song.PathToFile;
                string date = song.Year.ToString("yyyy-MM-dd");
                tbYear.Text = date;
                tbGenre.Text = song.Genre;
                tbRating.Text = song.Rating + "";
                tbDescription.Text = song.Description;
            }

        }
    }
}
