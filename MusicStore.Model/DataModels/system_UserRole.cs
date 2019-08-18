﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.DataModels
{
    public class system_UserRole
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<system_User> Users { get; set; }
    }
}
