using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReadySetGo.Library;
using ReadySetGo.Models;

namespace ReadySetGo.Controllers
{
    public class PlaylistController : Controller
    {
        readonly ISetlistBuilder _setlistBuilder;

        public PlaylistController(ISetlistBuilder setlistBuilder)
        {
            _setlistBuilder = setlistBuilder;
        }

        public IActionResult Index(string artistName)
        {
            var songs = _setlistBuilder.CreateSetlist(artistName);

            return View(new PlaylistModel{ ArtistName = WebUtility.UrlDecode(artistName), Songs = songs });
        }
    }
}
