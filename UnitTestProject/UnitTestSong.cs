using MusicLibrary;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MusicLibrary.Tests
{
    [TestClass()]
    public class UnitTestSong
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SongYearTest(int year)
        {
            Song song = new Song(1,"Hello", "Adele",1,1,"PoP", "C:\\MusicLibrary\\hello.mp3",1900,"Pop",3);
            //song.Year = DateTime.ParseExact("1899", "yyyy", null);
            //song.Year = DateTime.ParseExact("2100", "yyyy", null);
            song.Year = DateTime.ParseExact("1900", "yyyy", null);
            song.Year = DateTime.ParseExact("2099", "yyyy", null);
            song.Year = DateTime.ParseExact("2055", "yyyy", null);
            song.Year = DateTime.ParseExact("2017", "yyyy", null);
            //Assert.Fail();
        }
    }
}

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestPlaylist
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
