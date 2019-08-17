using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service
{
    public static class MapperInitialization
    {
        public static void init()
        {
        }

        public static void ConfigureProjectMapping()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<fl_Project, ProjectEntity>()).CreateMapper();
        }
    }
}
