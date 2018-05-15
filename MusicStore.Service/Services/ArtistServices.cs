using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using MusicStore.Model.UnitOfWork;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.Services
{
    public class ArtistServices : IArtistServices
    {
        #region variables
        private readonly UnitOfWork _unitOfWork;
        #endregion

        #region constructors
        public ArtistServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region public functions
        public IEnumerable<ArtistEntity> GetAllArtists()
        {
            var artists = _unitOfWork.ArtistRepository.GetAll().ToList();
            if (artists.Any())
            {
                Mapper.CreateMap<ms_Artist, ArtistEntity>();
                var artistList = Mapper.Map<List<ms_Artist>,List<ArtistEntity>>(artists);
                return artistList;
            }
            return null;
        }

        public ArtistEntity GetArtistById(int artistId)
        {
            var artist = _unitOfWork.ArtistRepository.GetByID(artistId);
            if(artist != null)
            {
                Mapper.CreateMap<ms_Artist, ArtistEntity>();
                var artistEntity = Mapper.Map<ms_Artist, ArtistEntity>(artist);
                return artistEntity;
            }
            return null;
        }

        public IEnumerable<ArtistEntity> GetArtistsByCategory(string categoryUrl)
        {
            ms_Genre foundGenre = _unitOfWork.GenreRepository.GetWithInclude(g => g.Url == categoryUrl, "Artists").FirstOrDefault();
            if (foundGenre == null) return null;
            if (foundGenre.Artists.Any())
            {
                Mapper.CreateMap<ms_Artist, ArtistEntity>();
                var artistsModel = Mapper.Map<List<ms_Artist>, List<ArtistEntity>>(foundGenre.Artists.ToList());
                return artistsModel;
            }
            return null;
        }

        public IEnumerable<ArtistEntity> GetTopArtists(int top)
        {
            var artists = _unitOfWork.ArtistRepository.GetAll().Take(top).ToList();
            if (artists.Any())
            {
                Mapper.CreateMap<ms_Artist, ArtistEntity>();
                var artistList = Mapper.Map<List<ms_Artist>, List<ArtistEntity>>(artists);
                return artistList;
            }
            return null;
        }

        #endregion
    }
}
