using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary
{
    class Database
    {
        void GetSongsFromLib()
        {
            using (RemoteLibrary ctx = new RemoteLibrary())
            {
                var lstMusic = (from r in ctx.ListMusicLibrary select r).ToList<Song>();
                foreach (var s in lstMusic)
                {
                    ctx.ListMusicLibrary.Add(s);
                    Console.WriteLine("S: {0}, {1}, {2}", s.Id, s.Title, s.ArtistName);
                }
            }
        }

        void SaveSongsToLib()
        {
            using (RemoteLibrary ctx = new RemoteLibrary())
            {
                var lstMusic = (from r in ctx.ListMusicLibrary select r).ToList<Song>();
                foreach (var s in lstMusic)
                {
                    ctx.ListMusicLibrary.Add(s);
                    ctx.SaveChanges();
                    Console.WriteLine("S: {0}, {1}, {2}", s.Id, s.Title, s.ArtistName);
                }
            }
        }
    }
}
