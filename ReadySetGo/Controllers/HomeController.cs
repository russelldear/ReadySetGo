using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReadySetGo.Library;
using ReadySetGo.Models;

namespace ReadySetGo.Controllers
{
    public class HomeController : Controller
    {
        readonly ISetlistBuilder _setlistBuilder;

        public HomeController(ISetlistBuilder setlistBuilder)
        {
            _setlistBuilder = setlistBuilder;
        }

        public IActionResult Index(string artistName)
        {
            if (string.IsNullOrWhiteSpace(artistName))
            {
                return View();
            }

            var songs = _setlistBuilder.CreateSetlist(artistName);

            return View(new HomeModel { ArtistName = WebUtility.UrlDecode(artistName), Songs = songs });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
