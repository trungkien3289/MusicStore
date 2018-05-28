using MusicStore.BussinessEntity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class SearchResultEntity
    {
        public ItemType Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
    }

    public class SearchResult
    {
        public IList<SearchResultEntity> Albums { get; set; }
        public IList<SearchResultEntity> Songs { get; set; }
        public IList<SearchResultEntity> Artists { get; set; }
    }
}