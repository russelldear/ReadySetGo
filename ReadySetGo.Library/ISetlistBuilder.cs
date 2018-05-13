using System.Collections.Generic;
using ReadySetGo.Library.DataContracts;

namespace ReadySetGo.Library
{
    public interface ISetlistBuilder
    {
        PlaylistResult CreateSetlist(string artist, int concertCount);
    }
}