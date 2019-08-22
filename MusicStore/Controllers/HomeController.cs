using MusicStore.Model.DataModels;
using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            IList<SummarySongModel> songs = GetFavoriteSong();
            return View(songs);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult GetListSong()
        {
            IList<SummarySongModel> songs = GetFavoriteSong();

            return Json(songs, JsonRequestBehavior.AllowGet);
        }

        private IList<SummarySongModel> GetFavoriteSong()
        {
            IList<SummarySongModel> songs = new List<SummarySongModel>();
            songs.Add(new SummarySongModel()
            {
                Title = "All This Is - Joe L.'s Studio",
                MediaUrl = "/Data/Audio/AC_ATKMTake_2.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "The Forsaken - Broadwing Studio (Final Mix)",
                MediaUrl = "/Data/Audio/AC_M.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "All The King's Men - Broadwing Studio (Final Mix)",
                MediaUrl = "/Data/Audio/AC_TSOWAfucked_up.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "The Forsaken - Broadwing Studio (First Mix)",
                MediaUrl = "/Data/Audio/BS_ATKM.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "All The King's Men - Broadwing Studio (First Mix)",
                MediaUrl = "/Data/Audio/BS_TF.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "All This Is - Alternate Cuts",
                MediaUrl = "/Data/Audio/BSFM_ATKM.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "All The King's Men (Take 1) - Alternate Cuts",
                MediaUrl = "/Data/Audio/BSFM_TF.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "All The King's Men (Take 2) - Alternate Cuts",
                MediaUrl = "/Data/Audio/JLS_ATI.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "Magus - Alternate Cuts",
                MediaUrl = "/Data/Audio/PNY04-05_M.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "The State Of Wearing Address (fucked up) - Alternate Cuts",
                MediaUrl = "/Data/Audio/PNY04-05_M.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = ">Magus - Popeye's (New Years '04 - '05)",
                MediaUrl = "/Data/Audio/PNY04-05_T.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "On The Waterfront - Popeye's (New Years '04 - '05)",
                MediaUrl = "/Data/Audio/PNY04-05_TSOWA.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "Trance - Popeye's (New Years '04 - '05)",
                MediaUrl = "/Data/Audio/SSB06_06_03_I.mp3",
                Thumbnail = ""
            });

            return songs;
        }
    }
}