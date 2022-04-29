using Exceptions.DomainLayer;
using Exceptions.Exceptions;
using Exceptions.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exceptions.Controllers
{
    public class MoviesController : Controller
    {
        private readonly DomainFacade _domainFacade = new DomainFacade();

        [Route("api/selam")]
        [HttpGet]
        public IEnumerable<string> Selam()
        {
            yield return "asd";
            yield return "bce";
        }

        [Route("api/movies/get")]
        [HttpGet]
        public IEnumerable<MovieResource> Get()
        {
            var movies = _domainFacade.GetAllMovies();
            return MapToMovieResource(movies);
        }

        [Route("api/movies/genre/{genreAsString}")]
        [HttpGet]
        public ActionResult<IEnumerable<MovieResource>> GetMoviesByGenre(string genreAsString)
        {
            try
            {
                var genre = GenreParser.Parse(genreAsString);
                var movies = _domainFacade.GetMoviesByGenre(genre);
                return new OkObjectResult(MapToMovieResource(movies));
            }
            catch (Exception e)
            {
                return new ExceptionActionResult(e);
            }
        }

        private static IEnumerable<MovieResource> MapToMovieResource(IEnumerable<Movie> movies)
        {
            foreach (var movie in movies)
            {
                yield return new MovieResource { Name = movie.Name, Genre = GenreParser.ToString(movie.Genre), ImageUrl = movie.ImageUrl, Year = movie.Year };
            }
        }
    }
}
