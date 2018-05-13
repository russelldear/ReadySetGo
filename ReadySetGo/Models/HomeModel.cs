using System;
using System.Collections.Generic;
using ReadySetGo.Library.DataContracts;

namespace ReadySetGo.Models
{
    public class HomeModel
    {
        public string ArtistName { get; set; }

        public List<Song> Songs { get; set; }
    }
}
