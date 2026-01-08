using FilmView.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FilmView.Controllers
{
    public static class FilmViewController
    {
        static HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5211/") };

        public static async Task ListCountries()
        {
            Console.Clear();
            var paises = await client.GetFromJsonAsync<List<Film>>("Films");
            if (paises == null || paises.Count == 0)
            {
                Console.WriteLine("No hay películas registrados.");
                return;
            }

            foreach (var p in paises)
            {
                ShowCountry(p);
            }
        }

        public static async Task SearchCountry()
        {
            Console.Clear();
            Console.WriteLine("Buscar por: 1) Título 2) Director 3) Año 4) Género 5) Duración 6) Calificación");
            var input = Console.ReadLine();
            string url = "Countries";

            switch (input)
            {
                case "1":
                    Console.Write("Título: ");
                    var title = Console.ReadLine();
                    url += $"/FilmByTitle?name={title}";
                    break;
                case "2":
                    Console.Write("Director: ");
                    var Director = Console.ReadLine();
                    url += $"/FilmByDirector?director={Director}";
                    break;
                case "3":
                    Console.Write("Año: ");
                    int.TryParse(Console.ReadLine(), out int year);
                    url += $"/FilmByYear?year={year}";
                    break;
                case "4":
                    Console.Write("Género: ");
                    string genre = Console.ReadLine();
                    url += $"/FilmByGenre?genre={genre}";
                    break;
                case "5":
                    Console.Write("Duración: ");
                    int.TryParse(Console.ReadLine(), out int duration);
                    url += $"/FilmByDuration?duration={duration}";
                    break;
                case "6":
                    Console.Write("Calificación: ");
                    double.TryParse(Console.ReadLine(), out double rating);
                    url += $"/FilmByRating?rating={rating}";
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    return;
            }

            var results = await client.GetFromJsonAsync<List<Film>>(url);
            if (results == null || results.Count == 0)
                Console.WriteLine("No se encontraron películas.");
            else
                results.ForEach(ShowCountry);
        }

        public static async Task AddCountry()
        {
            Console.Clear();
            Console.WriteLine("\n--- Agregar Película ---");
            var p = new Film();
            Console.Write("Título: "); p.Title = Console.ReadLine()!;
            Console.Write("Director: "); p.Director = Console.ReadLine()!;
            Console.Write("Año: "); int.TryParse(Console.ReadLine(), out int year); p.Year = year;
            Console.Write("Género: "); p.Genre = Console.ReadLine()!;
            Console.Write("Duración: "); int.TryParse(Console.ReadLine(), out int duration); p.Duration = duration;
            Console.Write("Calificación: "); double.TryParse(Console.ReadLine(), out double rating); p.Rating = rating;

            var response = await client.PostAsJsonAsync("Films/AddFilm", p);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        public static async Task ModifyCountry()
        {
            Console.Clear();
            Console.WriteLine("\n--- Modificar Película ---");
            var p = new Film();
            Console.Write("Título de la película a modificar: "); p.Title = Console.ReadLine()!;
            Console.Write("Nueva Director: "); p.Director = Console.ReadLine()!;
            Console.Write("Nuevo Año: "); int.TryParse(Console.ReadLine(), out int year); p.Year = year;
            Console.Write("Nuevo Género: "); p.Genre = Console.ReadLine()!;
            Console.Write("Nueva Duración: "); int.TryParse(Console.ReadLine(), out int duration); p.Duration = duration;
            Console.Write("Nueva Calificación: "); double.TryParse(Console.ReadLine(), out double rating); p.Rating = rating;

            var response = await client.PutAsJsonAsync("Films/ModifyFilm", p);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        public static async Task DeleteCountry()
        {
            Console.Clear();
            Console.WriteLine("\n--- Eliminar País ---");
            Console.Write("Nombre del país a eliminar: ");
            var name = Console.ReadLine();
            var response = await client.DeleteAsync($"Countries/DeleteCountry?name={name}");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        public static async Task CountryQuiz()
        {
            var countries = await client.GetFromJsonAsync<List<Film>>("Countries");
            if (countries == null || countries.Count == 0)
            {
                Console.WriteLine("No hay países disponibles para el quiz.");
                Console.WriteLine("Presiona Enter para volver al menú...");
                Console.ReadLine();
                return;
            }

            var rnd = new Random();
            int correct = 0;
            int total = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- Concurso de Países ---");
                Console.WriteLine("Adivina la capital del país mostrado.");
                Console.WriteLine("Escribe '0' para salir.\n");

                var country = countries[rnd.Next(countries.Count)];
                Console.Write($"¿Cuál es la capital de {country.Name}? ");
                var answer = Console.ReadLine();

                if (answer == "0")
                {
                    Console.Clear();
                    Console.WriteLine($"Has terminado el quiz. Respuestas correctas: {correct}/{total}");
                    Console.WriteLine("Presiona Enter para volver al menú...");
                    Console.ReadLine();
                    break;
                }

                total++;

                Console.Clear();
                if (string.Equals(answer.Trim(), country.Capital, StringComparison.OrdinalIgnoreCase))
                {
                    correct++;
                    Console.WriteLine($"¡Correcto! La capital de {country.Name} es {country.Capital}.\n");
                }
                else
                {
                    Console.WriteLine($"Incorrecto. La capital de {country.Name} es {country.Capital}.\n");
                }

                Console.WriteLine($"Progreso: {correct} correctas de {total} intentos");
                Console.WriteLine("\nPresiona Enter para continuar al siguiente país...");
                Console.ReadLine();
            }
        }


        private static void ShowCountry(Film p)
        {
            Console.WriteLine($"\nNombre: {p.title}, Capital: {p}, Población: {p}, Área: {p}, Idioma: {p}");
        }
    }
}
