using FilmView.Controllers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FilmsView
{
    class Program
    {
        static async Task WaitApiAsync(string url)
        {
            using var c = new HttpClient();
            while (true)
            {
                try
                {
                    var r = await c.GetAsync(url);
                    if (r.IsSuccessStatusCode) break;
                }
                catch { }
                Console.WriteLine("Esperando a que la API esté disponible...");
                await Task.Delay(1000);
            }
        }

        static async Task Main()
        {
            await WaitApiAsync("http://localhost:5124/Film");
            Console.WriteLine("API disponible. Iniciando menú...");

            while (true)
            {
                try
                {
                    ShowMenu();
                    var input = Console.ReadLine();
                    if (!int.TryParse(input, out int opc)) continue;

                    switch (opc)
                    {
                        case 1:
                            await FilmViewController.ListFilms();
                            break;
                        case 2:
                            await FilmViewController.SearchFilm();
                            break;
                        case 3:
                            await FilmViewController.AddFilm();
                            break;
                        case 4:
                            await FilmViewController.ModifyFilm();
                            break;
                        case 5:
                            await FilmViewController.DeleteFilm();
                            break;
                        case 6:
                            await FilmViewController.FilmQuiz();
                            break;
                        case 7:
                            Console.WriteLine("Saliendo...");
                            return;
                        default:
                            Console.WriteLine("Opción no válida.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n--- Menú ---");
            Console.WriteLine("1. Listar todas las películas");
            Console.WriteLine("2. Buscar película");
            Console.WriteLine("3. Agregar una película");
            Console.WriteLine("4. Modificar una película");
            Console.WriteLine("5. Eliminar una película");
            Console.WriteLine("6. Concurso de películas");
            Console.WriteLine("7. Salir");
            Console.Write("Selecciona una opción: ");
        }
    }
}
