# FilmView Console Project

Esta es la aplicación de consola de la solución **FilmsSolution** que permite interactuar con la API **FilmApi** para gestionar películas.

---

## Requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Sistema operativo: Windows, Linux o macOS
* API **FilmApi** corriendo en `http://localhost:5211/`

---

## Estructura del proyecto

```
FilmsView/           # Proyecto de consola
    ├─ Program.cs
    │
    ├─Controllers/
    │  └─ FilmViewController.cs
    └─Model/
       └─ Film.cs
```

---

## Configuración

1. Clonar el repositorio:

```bash
git clone <url-del-repositorio>
cd FilmSolution/FilmView
```

2. Restaurar paquetes:

```bash
dotnet restore
```

3. Asegúrate de que la API **FilmApi** esté corriendo en `http://localhost:5124/`

---

## Ejecución

```bash
dotnet run --project FilmView
```

La aplicación esperará a que la API esté disponible y luego mostrará un menú interactivo.

---

## Menú de la consola

1. **Listar todas las películas**: Muestra todas las películas registradas.
2. **Buscar película**: Permite buscar por título, director, año, género, duración o calificación.
3. **Agregar una película**: Solicita datos de la película y lo envía a la API.
4. **Modificar una película**: Actualiza los datos de una película existente.
5. **Eliminar una película**: Borra una película de la base de datos.
6. **Concurso de películas**: Juego para adivinar la película.
7. **Salir**: Termina la aplicación.

---

## Funcionalidades destacadas

* **Integración con API**: Utiliza `HttpClient` para consumir los endpoints de **FilmApi**.
* **Interfaz de consola interactiva**: Menú fácil de usar para todas las operaciones.
* **Concurso de Peliculas**: Modo divertido para adivinar películas.
* **Validación de disponibilidad de API**: Espera automáticamente a que la API esté activa antes de iniciar.

---

## Notas

* La API debe estar corriendo antes de iniciar la consola.
* Puedes extender `FilmView` para agregar más funcionalidades, como estadísticas o filtros avanzados.
* La aplicación utiliza `System.Net.Http.Json` para simplificar la serialización y deserialización de JSON.
