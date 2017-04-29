using MusicLibrary;
using NUnit.Framework;
using System;

namespace NUnitMusicLibrary
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestSongYearPassed()
        {
            // TODO: Add your test code here
            Song song = new Song("Hello", "Adele", 1, 1, "PoP", "C:\\MusicLibrary\\hello.mp3", 1900, "Pop", 3);
            song.Year = DateTime.ParseExact("1900", "yyyy", null);
            song.Year = DateTime.ParseExact("2099", "yyyy", null);
            song.Year = DateTime.ParseExact("2055", "yyyy", null);
            song.Year = DateTime.ParseExact("2017", "yyyy", null);
            Assert.Pass("Your first passing test");
        }

        [Test]
        public void TestSongYearLowBoundaryFailed()
        {
            // TODO: Add your test code here
            Song song = new Song("Hello", "Adele", 1, 1, "PoP", "C:\\MusicLibrary\\hello.mp3", 1900, "Pop", 3);
            song.Year = DateTime.ParseExact("1899", "yyyy", null);
        }

        [Test]
        public void TestSongYearHighBoundaryFailed()
        {
            // TODO: Add your test code here
            Song song = new Song("Hello", "Adele", 1, 1, "PoP", "C:\\MusicLibrary\\hello.mp3", 1900, "Pop", 3);

            song.Year = DateTime.ParseExact("2100", "yyyy", null);
        }

        [Test]
        public void TestSongYearLowInvalidValueFailed()
        {
            // TODO: Add your test code here
            Song song = new Song("Hello", "Adele", 1, 1, "PoP", "C:\\MusicLibrary\\hello.mp3", 1900, "Pop", 3);

            song.Year = DateTime.ParseExact("1855", "yyyy", null);
        }

        [Test]
        public void TestSongYearHighInvalidValueFailed()
        {
            // TODO: Add your test code here
            Song song = new Song("Hello", "Adele", 1, 1, "PoP", "C:\\MusicLibrary\\hello.mp3", 1900, "Pop", 3);

            song.Year = DateTime.ParseExact("2150", "yyyy", null);
        }
    }
}
