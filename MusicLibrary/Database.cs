using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary
{
    class Database
    {
        internal List<Song> GetSongsFromLib()
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstSongs = (from s in ctx.Songs select s).ToList<Song>();
                foreach (var s in lstSongs)
                {
                    ctx.Songs.Add(s);
                    Console.WriteLine("S: {0}, {1}, {2}", s.Id, s.Title, s.ArtistName);
                }
                return lstSongs;
            }
        }

        internal void SaveSongsToLib()
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstMusic = (from r in ctx.Songs select r).ToList<Song>();
                foreach (var s in lstMusic)
                {
                    ctx.Songs.Add(s);
                    ctx.SaveChanges();
                    Console.WriteLine("Song: {0}, {1}, {2}", s.Id, s.Title, s.ArtistName);
                }
            }
        }

        internal List<PlayList> GetPlaylists()
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstPlaylist = (from pl in ctx.PlayLists select pl).ToList<PlayList>();
                foreach (var pl in lstPlaylist)
                {
                    //ctx.PlayLists.Add(pl);
                    Console.WriteLine("Playlist: {0}, {1}, {2}", pl.Id, pl.PlayListName, pl.SongId);
                }
                return lstPlaylist;
            }
        }

        internal List<String> GetPlaylistName()
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstPlName = (from pl in ctx.PlayLists select pl.PlayListName).ToList<String>();
                foreach (var pl in lstPlName)
                {
                    //ctx.PlayLists.Add(pl);
                    Console.WriteLine("Playlist Name: " + pl);
                }
                return lstPlName;
            }
        }

        internal void SavePlaylists()
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstMusic = (from pl in ctx.PlayLists select pl).ToList<PlayList>();
                foreach (var pl in lstMusic)
                {
                    ctx.PlayLists.Add(pl);
                    Console.WriteLine("Playlist: {0}, {1}, {2}", pl.Id, pl.PlayListName, pl.SongId);
                }
            }
        }

        internal List<Song> GetMusicByPlaylistName(string plName)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstMusic = (from s in ctx.Songs
                                from pl in ctx.PlayLists
                                where pl.PlayListName == plName
                                select s).ToList<Song>();
                foreach (var s in lstMusic)
                {
                    Console.WriteLine("S: {0}, {1}", s.Title, s.ArtistName);
                }
                return lstMusic;
            }
        }

        internal List<PlayList> GetPlaylistByName(string plName)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstMusic = (from pl in ctx.PlayLists
                                where pl.PlayListName == plName
                                select pl).ToList<PlayList>();
                foreach (var pl in lstMusic)
                {
                    Console.WriteLine("PlayList: {0}, {1} {2}", pl.Id, pl.SongId, pl.PlayListName);
                }
                return lstMusic;
            }
        }
    }
}
