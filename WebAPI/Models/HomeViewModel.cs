﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            TopAlbums = new List<AlbumSummary>();
        }
        public IList<AlbumSummary> TopAlbums { get; set; }
    }

}