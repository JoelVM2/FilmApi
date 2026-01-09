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
        static HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5124/") };

        public static async Task ListFilms()
        {
            Console.Clear();
            var films = await client.GetFromJsonAsync<List<Film>>("Film");
            if (films == null || films.Count == 0)
            {
                Console.WriteLine("No hay películas registrados.");
                return;
            }

            foreach (var f in films)
            {
                ShowFilm(f);
            }
        }

        public static async Task SearchFilm()
        {
            Console.Clear();
            Console.WriteLine("Buscar por: 1) Título 2) Director 3) Año 4) Género 5) Duración 6) Calificación");
            var input = Console.ReadLine();
            string url = "Film";

            switch (input)
            {
                case "1":
                    Console.Write("Título: ");
                    var title = Console.ReadLine();
                    url += $"/FilmByTitle?title={title}";
                    break;
                case "2":
                    Console.Write("Director: ");
                    var director = Console.ReadLine();
                    url += $"/FilmByDirector?director={director}";
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
                results.ForEach(ShowFilm);
        }

        public static async Task AddFilm()
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

            var response = await client.PostAsJsonAsync("Film/AddFilm", p);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        public static async Task ModifyFilm()
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

            var response = await client.PutAsJsonAsync("Film/ModifyFilm", p);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        public static async Task DeleteFilm()
        {
            Console.Clear();
            Console.WriteLine("\n--- Eliminar Película ---");
            Console.Write("Nombre de la película a eliminar: ");
            var title = Console.ReadLine();
            var response = await client.DeleteAsync($"Film/DeleteFilm?title={title}");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        public static async Task FilmQuiz()
        {
            var films = await client.GetFromJsonAsync<List<Film>>("Film");
            if (films == null || films.Count == 0)
            {
                Console.WriteLine("No hay películas disponibles para el juego.");
                Console.WriteLine("Presiona Enter para volver al menú...");
                Console.ReadLine();
                return;
            }

            var rnd = new Random();
            int totalPoints = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n Adivina la película (por pistas)");
                Console.WriteLine("Escribe el título en cualquier momento.");
                Console.WriteLine("Escribe '0' para salir.\n");

                var film = films[rnd.Next(films.Count)];
                int cluesUsed = 0;
                int points = 5;

                while (true)
                {
                    cluesUsed++;

                    switch (cluesUsed)
                    {
                        case 1:
                            Console.WriteLine($"Pista 1️ Año: {film.Year}");
                            break;
                        case 2:
                            Console.WriteLine($"Pista 2️ Género: {film.Genre}");
                            points--;
                            break;
                        case 3:
                            Console.WriteLine($"Pista 3️ Director: {film.Director}");
                            points--;
                            break;
                        case 4:
                            Console.WriteLine($"Pista 4️ Duración: {film.Duration} minutos");
                            points--;
                            break;
                        case 5:
                            Console.WriteLine($"Pista 5️ Calificación: {film.Rating}");
                            points--;
                            break;
                        default:
                            Console.WriteLine($"\n Sin más pistas. La película era: {film.Title}");
                            Console.WriteLine("\nPresiona Enter para continuar...");
                            Console.ReadLine();
                            break;
                    }

                    if (cluesUsed > 5)
                        break;

                    Console.Write("\n¿Tu respuesta?: ");
                    var answer = Console.ReadLine();

                    if (answer == "0")
                    {
                        Console.Clear();
                        Console.WriteLine($" Juego terminado. Puntos totales: {totalPoints}");
                        Console.WriteLine("Presiona Enter para volver al menú...");
                        Console.ReadLine();
                        return;
                    }

                    if (string.Equals(answer?.Trim(), film.Title, StringComparison.OrdinalIgnoreCase))
                    {
                        totalPoints += points;
                        Console.WriteLine($"\n ¡Correcto! Era {film.Title}");
                        Console.WriteLine($"⭐ Puntos obtenidos: {points}");
                        Console.WriteLine($" Puntos totales: {totalPoints}");
                        Console.WriteLine("\nPresiona Enter para la siguiente película...");
                        Console.ReadLine();
                        break;
                    }

                    Console.WriteLine(" Incorrecto... siguiente pista.\n");
                }
            }
        }


        private static void ShowFilm(Film f)
        {
            Console.WriteLine($"\nTitle: {f.Title}, Director: {f.Director}, Año: {f.Year}, Género: {f.Genre}, Duración: {f.Duration}, Calificación: {f.Rating}");
        }
    }
}
