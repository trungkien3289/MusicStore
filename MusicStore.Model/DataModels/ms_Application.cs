using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.DataModels
{
    public class ms_Application
    {
        [Key]
        public int Id { get; set; }
        public string AppId { get; set; }
        public string WDGId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }
        public bool Generic { get; set; }
    }
}
