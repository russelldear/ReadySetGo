using System.Collections.Generic;
using ReadySetGo.Library.DataContracts;

namespace ReadySetGo.Library
{
    public interface ISpotifyService
    {
        string CreatePlaylist(TokenResponse token, PlaylistResult playlist);
    }
}