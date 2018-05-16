using System;
using System.Collections.Generic;
using System.Net;
using ReadySetGo.Library.DataContracts;

namespace ReadySetGo.Models
{
    public class HomeModel
    {
        public bool? ArtistFound { get; set; }

        public bool? SongsFound { get; set; }

        public string ArtistName { get; set; }

        public int ConcertCount { get; set; }

        public int ActualCount { get; set; }

        public List<Song> Songs { get; set; }

        public string Url { get; set; }
    }

    public static class ModelExtensions
    {
        public static HomeModel ToHomeModel(this PlaylistResult source)
        {
            var target = new HomeModel
            {
                ArtistName = WebUtility.UrlDecode(source.ArtistName),
                ConcertCount = source.RequestedCount
            };

            if (source.ArtistFound.HasValue && source.ArtistFound.Value == false)
            {
                target.ArtistFound = false;
            }
            else if (source.SongsFound.HasValue && source.SongsFound.Value == false)
            {
                target.SongsFound = false;
            }
            else
            {
                target.ActualCount = source.ActualCount;
                target.Songs = source.Songs;
                target.ArtistFound = true;
                target.SongsFound = true;
            }

            target.Url = source.Url;

            return target;
        }
    }
}
