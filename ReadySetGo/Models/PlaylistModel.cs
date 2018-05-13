using System;
using System.Collections.Generic;
using ReadySetGo.Library.DataContracts;

namespace ReadySetGo.Models
{
    public class PlaylistModel
    {
        public PlaylistModel()
        {
        }

        public List<Song> Songs { get; set; }
    }
}
