using System.Collections.Generic;
using ReadySetGo.Library.DataContracts;

namespace ReadySetGo.Library
{
    public interface ISetlistBuilder
    {
        List<Song> CreateSetlist(string artist, int concertCount);
    }
}