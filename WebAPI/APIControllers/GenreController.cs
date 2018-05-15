using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.APIControllers
{
    public class GenreController : ApiController
    {
        private readonly IGenreServices _genreServices;

        public GenreController(IGenreServices genreServices)
        {
            _genreServices = genreServices;
        }

        // GET api/genre
        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage Get()
        {
            var genres = _genreServices.GetAllGenres();
            if (genres != null)
            {
                var genreEntities = genres as List<GenreEntity> ?? genres.ToList();
                if (genreEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, genreEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Genres not found");
        }
    }
}
