using MusicStore.Service.IServices;
using MusicStore.Service.Services;
using Resolver;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IAlbumServices, AlbumServices>();
            registerComponent.RegisterType<IUserServices, UserServices>();
        }
    }
}
