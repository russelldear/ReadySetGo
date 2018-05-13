using System.Collections.Generic;
using ReadySetGo.Library.DataContracts;

namespace ReadySetGo.Library
{
    public interface ISetlistFmService
    {
        Artist SearchArtist(string artistName);

        List<Setlist> GetSetlists(string mbid, int concertCount, out int actualCount);
    }
}