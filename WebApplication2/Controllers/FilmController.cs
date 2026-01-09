using FilmApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace FilmApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmController : ControllerBase
    {
        private readonly string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Films.json");

        [HttpGet]
        public IEnumerable<Film> Get()
        {
            return GetAllFilms();
        }

        [HttpGet("FilmByTitle")]
        public IEnumerable<Film> GetFilmByTitle(string title)
        {
            var films = GetAllFilms();
            return films.Where(c => c.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        [HttpGet("FilmByDirector")]
        public IEnumerable<Film> GetFilmByDirector(string director)
        {
            var films = GetAllFilms();
            return films.Where(c => c.Director.Contains(director, StringComparison.OrdinalIgnoreCase));
        }

        [HttpGet("FilmByYear")]
        public IEnumerable<Film> GetFilmByYear(int year)
        {
            var films = GetAllFilms();
            return films.Where(c => c.Year == year);
        }

        [HttpGet("FilmByGenre")]
        public IEnumerable<Film> GetFilmByGenre(string genre)
        {
            var films = GetAllFilms();
            return films.Where(c => c.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));
        }

        [HttpGet("FilmByDuration")]
        public IEnumerable<Film> GetFilmByDuration(int duration)
        {
            var films = GetAllFilms();
            return films.Where(c => c.Duration== duration);
        }

        [HttpGet("FilmByRating")]
        public IEnumerable<Film> GetFilmByRating(double Rating)
        {
            var films = GetAllFilms();
            return films.Where(c => c.Rating == Rating);
        }

        [HttpPost("AddFilm")]
        public IActionResult AddFilm([FromBody] Film film)
        {
            if (film == null)
                return BadRequest("Datos de la película son necesarios.");

            var films = GetAllFilms();

            if (films.Any(c => c.Title.Equals(film.Title, StringComparison.OrdinalIgnoreCase)))
                return BadRequest("La película ya existe actualmente.");

            films.Add(film);
            SaveFilms(films);

            return Ok("Película añadida adecuadamente.");
        }

        [HttpPut("ModifyFilm")]
        public IActionResult ModifyFilm([FromBody] Film updatedFilm)
        {
            if (updatedFilm == null)
                return BadRequest("Datos de la película son necesarios.");

            var films = GetAllFilms();
            var existingFilm = films.FirstOrDefault(c =>
                c.Title.Equals(updatedFilm.Title, StringComparison.OrdinalIgnoreCase));

            if (existingFilm == null)
                return NotFound("Película no encontrada.");

            existingFilm.Director = updatedFilm.Director;
            existingFilm.Year = updatedFilm.Year;
            existingFilm.Genre = updatedFilm.Genre;
            existingFilm.Duration = updatedFilm.Duration;
            existingFilm.Rating = updatedFilm.Rating;

            SaveFilms(films);

            return Ok("Película actualizada correctamente.");
        }

        [HttpDelete("DeleteFilm")]
        public IActionResult DeleteFilm(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest("El título de la película es necesario.");

            var films = GetAllFilms();
            var film = films.FirstOrDefault(c =>
                c.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (film == null)
                return NotFound("Película no encontrada.");

            films.Remove(film);
            SaveFilms(films);

            return Ok("La película fue borrada correctamente.");
        }

        private List<Film> GetAllFilms()
        {
            try
            {
                if (!System.IO.File.Exists(jsonPath))
                    return new List<Film>();

                string jsonString = System.IO.File.ReadAllText(jsonPath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<List<Film>>(jsonString, options) ?? new List<Film>();
            }
            catch
            {
                return new List<Film>();
            }
        }

        private void SaveFilms(List<Film> films)
        {
            var json = JsonSerializer.Serialize(films, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(jsonPath, json);
        }
    }
}
