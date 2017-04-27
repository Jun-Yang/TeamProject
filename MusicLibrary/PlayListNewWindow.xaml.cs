using System;
using System.Collections.Generic;
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

namespace MusicLibrary
{
    /// <summary>
    /// Interaction logic for PlayListNewWindow.xaml
    /// </summary>
    public partial class PlayListNewWindow : Window
    {
        private List<PlayList> lsPlayList = new List<PlayList>();
        private Database db;
        public PlayListNewWindow()
        {
            InitializeComponent();
            db = new Database();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            String plName = tbPlayListName.Text;
            String plDescription = tbDescription.Text;
            int playListId = db.GetPlaylistsMaxId()+1;
            PlayList pl = new PlayList(playListId, 2000, plName, plDescription);
            //Playlist: {0}, {1}, {2}, {3}", pl.Id, pl.SongId, pl.PlayListName, pl.Description
            lsPlayList.Add(pl);
            db.SaveOnePlaylistsToLib(lsPlayList);
            MessageBox.Show("New Playlist Added into Remote Database MusicLibrary , playlist id is"+ playListId);

        }
    }
}
