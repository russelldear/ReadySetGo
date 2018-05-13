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

        public List<Song> CreateSetlist(string artistName)
        {
            var artist = _setlistFmService.SearchArtist(artistName);

            var setlists = _setlistFmService.GetSetlists(artist.Mbid).Where(s => s.SetlistSets != null && s.SetlistSets.Sets.Any()).Take(5);

            var songs = setlists
                .SelectMany(sl => sl.SetlistSets.Sets)
                .SelectMany(s => s.Songs)
                .Where(song => song.Cover == null && song.Tape == null)
                .ToList();

            songs.RemoveDuplicates();

            return songs;
        }
    }

    public static class Extensions
    {
        public static void RemoveDuplicates (this List<Song> songs)
        {
            for (var i = 0; i < songs.Count; i++)
            {
                for (int j = 0; j < songs.Count; j++)
                {
                    if (songs[i].Name.ToUpper() == songs[j].Name.ToUpper() && i != j)
                    {
                        songs[i].DuplicateCount++;
                        songs.RemoveAt(j);
                        j--;
                    }
                }
            }
        }
    }
}
