using System;
using System.Collections.Generic;

namespace ReadySetGo.Library.DataContracts
{
    public class PlaylistResult
    {
        public string ArtistName { get; set; }

        public int ConcertCount { get; set; }

        public int ActualCount { get; set; }

        public List<Song> Songs { get; set; }
    }
}
