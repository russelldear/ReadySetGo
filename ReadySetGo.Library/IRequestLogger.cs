using ReadySetGo.Library.DataContracts;

namespace ReadySetGo.Library
{
    public interface IRequestLogger
    {
        void LogSetlist(PlaylistResult playlistResult);
        void LogSpotify(SpotifyUser user, PlaylistResult playlistResult, TokenResponse token);

    }
}