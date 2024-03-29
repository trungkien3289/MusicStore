﻿using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.IService
{
    public interface ISongServices
    {
        SongEntity GetSongById(int songId);
        IEnumerable<SongEntity> GetTopSongs(int top);
        IEnumerable<SongEntity> GetSongsByCategory(string categoryUrl);
        IEnumerable<SongEntity> GetFeaturedSongs();
        IEnumerable<SongEntity> SearchByName(string query, int page = 1, int numberItemsPerPage = 10);
    }
}
