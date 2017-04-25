using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary
{
    class Database
    {
        internal List<Song> GetAllSongsFromLib()
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstSongs = (from s in ctx.Songs select s).ToList<Song>();
                foreach (var s in lstSongs)
                {
                    Console.WriteLine("S: {0}, {1}, {2},{3}", s.Id, s.Title, s.ArtistName, s.PathToFile);
                }
                return lstSongs;
            }
        }

        internal List<Song> GetSongsByTitleArtist(string filter)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstSongs = (from s in ctx.Songs
                                where s.Title.Contains(filter) || s.ArtistName.Contains(filter)
                                select s).ToList<Song>();
                foreach (var s in lstSongs)
                {
                    Console.WriteLine("S: {0}, {1}, {2},{3}", s.Id, s.Title, s.ArtistName, s.PathToFile);
                }
                return lstSongs;
            }
        }

        internal void SaveSongsToLib(List<Song> lstMusic)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                foreach (var s in lstMusic)
                {
                    ctx.Songs.Add(s);
                    ctx.SaveChanges();
                    Console.WriteLine("Song: {0}, {1}, {2}", s.Id, s.Title, s.ArtistName);
                }
            }
        }

        internal void DeleteSongsFromLib(List<Song> lstMusic)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                foreach (var s in lstMusic)
                {
                    ctx.Songs.Remove(s);
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
                var lstPlName = (from pl in ctx.PlayLists select pl.PlayListName).Distinct().ToList<String>();
                foreach (var pl in lstPlName)
                {
                    Console.WriteLine("Playlist Name: " + pl);
                }
                return lstPlName;
            }
        }

        internal void UpdataPlaylistbyName(String oldName,string newName,string desc)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstPlaylist = (from pl in ctx.PlayLists
                                   where pl.PlayListName == oldName
                                   select pl 
                                   ).ToList<PlayList>();
                foreach (var pl in lstPlaylist)
                {
                    pl.PlayListName = newName;
                    pl.Description = desc;
                    Console.WriteLine("Playlist Name: {0}, {1}, {2}", pl.Id, pl.PlayListName, pl.Description);
                }
                ctx.SaveChanges();
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
                var lstMusic = (from pl in ctx.PlayLists
                                where pl.PlayListName == plName
                                from s in ctx.Songs
                                where s.Id == pl.SongId
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
