using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MusicStore.APIController
{
    public class SongController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<SummarySongModel> Get()
        {
            return GetFavoriteSong();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
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