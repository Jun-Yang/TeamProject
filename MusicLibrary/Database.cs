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

        internal List<Song> GetSongsById(int id)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstSongs = (from s in ctx.Songs
                                where s.Id == id
                                select s).ToList<Song>();
                foreach (var s in lstSongs)
                {
                    Console.WriteLine("S: {0}, {1}, {2},{3}", s.Id, s.Title, s.ArtistName, s.PathToFile);
                }
                return lstSongs;
            }
        }

        internal string GetPathBySongId(int id)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                return (from s in ctx.Songs where s.Id == id select s.PathToFile).ToString();
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

        internal void SavePlaylistsToLib(List<PlayList> lstPlaylist)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                foreach (var pl in lstPlaylist)
                {
                    ctx.PlayLists.Add(pl);
                    ctx.SaveChanges();
                    Console.WriteLine("Playlist: {0}, {1}, {2}, {3}", pl.Id, pl.SongId, pl.PlayListName, pl.Description);
                }
            }
        }

        internal void SaveOnePlaylistsToLib(List<PlayList> lstPlaylist)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                foreach (var pl in lstPlaylist)
                {
                    ctx.PlayLists.Add(pl);
                    Console.WriteLine("Playlist: {0}, {1}, {2}, {3}", pl.Id, pl.SongId, pl.PlayListName, pl.Description);
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        Console.WriteLine("Entity.Infrastructure update Exception"+ex.StackTrace);
                    }

                    
                }
            }
        }


        //internal void SavePlaylist(String plName,String plDescription)
   
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

        internal void DeletePlaylistFromLib(string plName)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstPlaylist = (from pl in ctx.PlayLists
                                   where pl.PlayListName == plName
                                   select pl
                                   ).ToList<PlayList>();
                foreach (var pl in lstPlaylist)
                {
                    ctx.PlayLists.Remove(pl);
                    ctx.SaveChanges();
                    Console.WriteLine("Playlist: {0}, {1}, {2}, {3}", pl.Id, pl.SongId, pl.PlayListName, pl.Description);
                }
            }
        }

        internal void TruncatePlaylistsFromLib(string plName)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                ctx.Database.ExecuteSqlCommand("TRUNCATE TABLE [PlayLists]");
            }
        }

        internal void TruncateSongsFromLib(string plName)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                ctx.Database.ExecuteSqlCommand("TRUNCATE TABLE [Songs]");
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

        //add by chen
        internal int GetPlaylistsMaxId()
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                int maxId = ctx.PlayLists.Max(p => p.Id);
                return maxId;
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

        internal void UpdatePlaylistbyName(String oldName,string newName,string desc)
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

        //adding by chen 0426
        internal void UpdateSongByPath(Song song)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                var lstSong = (from s in ctx.Songs
                               where s.PathToFile == song.PathToFile
                               select s
                                   ).ToList<Song>();
                foreach (var s in lstSong)
                {
                    s.Title = song.Title;
                    s.ArtistName = song.ArtistName;
                    s.Album = song.Album;
                    s.Description = song.Description;
                    s.Rating = song.Rating;
                    s.Year = song.Year;
                    s.Genre = song.Genre;

                    Console.WriteLine("Song : {0}, {1}, {2}", s.Id, s.ArtistName, s.Description);
                }
                ctx.SaveChanges();
            }
        }

        internal void InsertSongToPlaylist(PlayList pl)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                ctx.PlayLists.Add(pl);
                ctx.SaveChanges();
                Console.WriteLine("Playlist: {0}, {1}, {2}, {3}", pl.Id, pl.SongId, pl.PlayListName, pl.Description);
            }
        }

        internal List<Song> GetSongByPlaylistName(string plName)
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

        internal PlayList GetPlaylistByName(string plName)
        {
            using (RemoteLibraryEntities ctx = new RemoteLibraryEntities())
            {
                return  (from pl in ctx.PlayLists where pl.PlayListName == plName
                                select pl).FirstOrDefault <PlayList>();
            }
        }
    }
}
