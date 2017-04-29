using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MusicLibrary
{
    [TestFixture]
    class SongYearTest
    {
        [Test]
        public void SongYearTestPassed(int year)
        {
            Song song = new Song("Hello", "Adele", 1, 1, "PoP", "C:\\MusicLibrary\\hello.mp3", 1900, "Pop", 3);
            song.Year = DateTime.ParseExact("1900", "yyyy", null);
            song.Year = DateTime.ParseExact("2099", "yyyy", null);
            song.Year = DateTime.ParseExact("2055", "yyyy", null);
            song.Year = DateTime.ParseExact("2017", "yyyy", null);
        }
    }
}
