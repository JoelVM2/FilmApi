# Film Api

Esta solución de .NET contiene dos proyectos principales:

1. **FilmApi**: API REST para gestionar información sobre películas.
2. **FilmView**: Aplicación de consola que se ejecuta automáticamente al iniciar la API (actualmente solo se inicia, puedes agregar interacción con la API).

---

## Requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Sistema operativo: Windows, Linux o macOS
* Editor recomendado: [Visual Studio](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)

---

## Estructura del proyecto

```
FilmsApi/
│
├─ FilmsApi/            # Proyecto Web API
│   ├─ Controllers/
│   │   └─ FilmsController.cs
│   ├─ Model/
│   │   └─ Film.cs
│   ├─ Films.json
│   └─ Program.cs
│
└─ FilmsView/           # Proyecto de consola
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
cd FilmsSolution
```

2. Asegurarse de tener instalada la versión correcta de .NET:

```bash
dotnet --version
```

3. Restaurar paquetes:

```bash
dotnet restore
```

---

## Ejecución

### Iniciar la API

Al ejecutar `FilmsApi`, se iniciará automáticamente `FilmsView`:

```bash
dotnet run --project FilmsApi
```

La API estará disponible en:

```
https://localhost:5001
http://localhost:5000
```

Se habilita **Swagger** en el entorno de desarrollo para probar los endpoints:

```
https://localhost:5001/swagger
```

### Endpoints disponibles

**Obtener todas las películas:**

```
GET /Films
```

**Filtrar por título, director, año, género, duración o calificación:**

```
GET /Films/FilmByTitle?title=Inception
GET /Films/FilmByDirector?director=Nolan
GET /Films/FilmByYear?year=2010
GET /Films/FilmByGenre?genre=Action
GET /Films/FilmByDuration?duration=120
GET /Films/FilmByRating?rating=8.5

```

**Agregar una película:**

```
POST /Films/AddFilm
Body: 
{
  "id": 1,
  "title": "Inception",
  "director": "Christopher Nolan",
  "year": 2010,
  "genre": "Sci-Fi",
  "duration": 148,
  "rating": 8.8
}
```

**Modificar una película:**

```
PUT /Films/ModifyFilm
Body: mismo formato que POST
```

**Eliminar una película:**

```
DELETE /Films/DeleteFilm?title=Inception
```

---

## Funcionalidades destacadas

* **Gestión completa de películas**: CRUD sobre películas.
* **Archivo JSON persistente**: Todos los cambios se guardan en `Films.json`.
* **Swagger UI**: Documentación interactiva de la API.
* **Integración con aplicación de consola**: `FilmsView` se inicia automáticamente al levantar la API.

---

## Notas

* Asegúrate de que `Film.json` tenga permisos de lectura/escritura para evitar errores.
* La API actualmente no implementa seguridad; para producción se recomienda añadir autenticación y autorización.
* `FilmsView` puede extenderse para consumir los endpoints de la API y mostrar información de manera interactiva.
