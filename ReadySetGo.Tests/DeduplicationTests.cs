using System;
using System.Collections.Generic;
using ReadySetGo.Library;
using ReadySetGo.Library.DataContracts;
using Xunit;

namespace ReadySetGo.Tests
{
    public class DeduplicationTests
    {
        private List<Song> _songs;

        private List<Song> _songList1 = new List<Song> { new Song { Name = "Song1" }, new Song { Name = "Song2" }, new Song { Name = "Song1" } };
        private List<Song> _songList2 = new List<Song> { new Song { Name = "Song1" }, new Song { Name = "Song1" }, new Song { Name = "Song2" } };
        private List<Song> _songList3 = new List<Song> { new Song { Name = "Song1" }, new Song { Name = "Song2" }, new Song { Name = "Song2" } };
        private List<Song> _songList4 = new List<Song> { new Song { Name = "Song2" }, new Song { Name = "Song2" }, new Song { Name = "Song2" } };

        [Fact]
        public void Deduplication_1()
        {
            Given_this_list_of_songs_with_duplicates(_songList1);

            When_I_deduplicate();

            There_are_no_duplicate_songs_left();
        }

        [Fact]
        public void Deduplication_2()
        {
            Given_this_list_of_songs_with_duplicates(_songList2);

            When_I_deduplicate();

            There_are_no_duplicate_songs_left();
        }

        [Fact]
        public void Deduplication_3()
        {
            Given_this_list_of_songs_with_duplicates(_songList3);

            When_I_deduplicate();

            There_are_no_duplicate_songs_left();
        }

        [Fact]
        public void Deduplication_4()
        {
            Given_this_list_of_songs_with_duplicates(_songList4);

            When_I_deduplicate();

            There_is_only_one_song_left();
        }

        private void Given_this_list_of_songs_with_duplicates(List<Song> songs)
        {
            _songs = songs;
        }

        private void When_I_deduplicate()
        {
            _songs.RemoveDuplicates();
        }

        private void There_are_no_duplicate_songs_left()
        {
            var expectedSongs = new List<Song>
            {
                new Song { Name = "Song1" },
                new Song { Name = "Song2" }
            };

            Assert.Equal(expectedSongs.Count, _songs.Count);

            for (var i = 0; i < _songs.Count; i++)
            {
                Assert.Equal(_songs[i].Name, expectedSongs[i].Name);
            }
        }

        private void There_is_only_one_song_left()
        {
            Assert.Equal(1, _songs.Count);
            Assert.Equal("Song2", _songs[0].Name);
        }
    }
}
