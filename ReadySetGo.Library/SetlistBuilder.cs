using System;
using System.Collections.Generic;
using System.Linq;
using ReadySetGo.Library.DataContracts;

namespace ReadySetGo.Library
{
    public class SetlistBuilder : ISetlistBuilder
    {
        private readonly ISetlistFmService _setlistFmService;

        public SetlistBuilder(ISetlistFmService setlistFmService)
        {
            _setlistFmService = setlistFmService;
        }

        public PlaylistResult CreateSetlist(string artistName, int requestedCount)
        {
            int actualCount;

            var artist = _setlistFmService.SearchArtist(artistName);

            if (artist == null)
            {
                return new PlaylistResult { ArtistFound = false };
            }

            var setlists = _setlistFmService.GetSetlists(artist.Mbid, requestedCount, out actualCount)
                                            .Where(s => s.SetlistSets != null && s.SetlistSets.Sets.Any());

            var songSetlists = GetSongsForSetlists(setlists);

            if (!songSetlists.Any(s => s.Any()))
            {
                return new PlaylistResult { ArtistName = artistName, SongsFound = false };
            }

            var songs = RiffleAndDeduplicate(songSetlists);

            return new PlaylistResult
            {
                ArtistName = artist.Name,
                RequestedCount = requestedCount,
                ActualCount = actualCount,
                Songs = songs,
                ArtistFound = true,
                SongsFound = true
            };
        }

        private static List<List<Song>> GetSongsForSetlists(IEnumerable<Setlist> setlists)
        {
            var songSetlists = new List<List<Song>>();

            foreach (var setlist in setlists)
            {
                var songSetlist = setlist.SetlistSets.Sets
                                         .SelectMany(s => s.Songs)
                                         .Where(song => song.Cover == null && song.Tape == null)
                                         .ToList();

                songSetlists.Add(songSetlist);
            }

            return songSetlists;
        }

        private static List<Song> RiffleAndDeduplicate(List<List<Song>> songSetlists)
        {
            var maxSongsInASetlist = songSetlists.Max(l => l.Count);

            var result = new List<Song>();

            for (var songCounter = 0; songCounter < maxSongsInASetlist; songCounter++)
            {
                for (var setlistCounter = 0; setlistCounter < songSetlists.Count; setlistCounter++)
                {
                    if (songSetlists.Count > setlistCounter && songSetlists[setlistCounter].Count > songCounter)
                    {
                        var currentSong = songSetlists[setlistCounter][songCounter];

                        if (result.Any(song => song.Name.ToUpper() == currentSong.Name.ToUpper()))
                        {
                            result.Single(song => song.Name.ToUpper() == currentSong.Name.ToUpper()).DuplicateCount++;
                        }
                        else
                        {
                            result.Add(currentSong);
                        }
                    }
                }
            }

            return result;
        }
    }
}
