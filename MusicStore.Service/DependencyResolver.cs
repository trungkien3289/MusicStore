using MusicStore.Service.IService;
using MusicStore.Service.Services;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace MusicStore.Service
{
    [Export(typeof(Resolver.IComponent))]
    public class DependencyResolver : Resolver.IComponent
    {
        public void SetUp(Resolver.IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IAlbumServices, AlbumServices>();
            registerComponent.RegisterType<IArtistServices, ArtistServices>();
            registerComponent.RegisterType<ISongServices, SongServices>();
            registerComponent.RegisterType<IGenreServices, GenreServices>();
            registerComponent.RegisterType<IUserServices, UserServices>();
            registerComponent.RegisterType<ITokenServices, TokenServices>();
            registerComponent.RegisterType<IApplicationServices, ApplicationServices>();
            registerComponent.RegisterType<ICollectionServices, CollectionServices>();
            registerComponent.RegisterType<IProjectServices, ProjectServices>();
            registerComponent.RegisterType<ITaskServices, TaskServices>();
            registerComponent.RegisterType<ITaskRequestServices, TaskRequestServices>();
        }
    }
}