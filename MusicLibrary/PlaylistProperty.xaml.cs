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
    /// Interaction logic for PlaylistProperty.xaml
    /// </summary>
    public partial class PlaylistProperty : Window
    {
        private Database db = new Database();
        string oldName;
        public PlaylistProperty(string name)
        {
            InitializeComponent();
            oldName = name;
            tbPlayListName.Text = oldName;
            PlayList pl = db.GetPlaylistByName(oldName);
            tbDescription.Text = pl.Description;
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            db.UpdatePlaylistbyName(oldName,tbPlayListName.Text,tbDescription.Text);
            Console.WriteLine("Update Playlist {0}, {1}, {2}", oldName, tbPlayListName.Text, tbDescription.Text);
            this.Close();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
