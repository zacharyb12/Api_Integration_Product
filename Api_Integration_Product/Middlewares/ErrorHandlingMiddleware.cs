using System.Net;
using System.Text.Json;

namespace Api_Integration_Product.Middlewares
{
    public class ErrorHandlingMiddleware(RequestDelegate _next,ILogger<ErrorHandlingMiddleware> _logger,IHostEnvironment _env)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // exécute le reste du pipeline
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur non gérée : {Message}", ex.Message);

                if (context.Response.HasStarted)
                {
                    // La réponse est déjà partie, on ne peut plus rien réécrire → on relance
                    throw;
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // 1. On choisit le code HTTP selon le type d'exception
            HttpStatusCode statusCode;

            switch (ex)
            {
                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;       // 400
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;         // 404
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;     // 401
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError; // 500
                    break;
            }

            // 2. On prépare la réponse
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            // 3. On décide du message à afficher
            string message;
            if (_env.IsDevelopment())
            {
                message = ex.Message;
            }
            else if (statusCode == HttpStatusCode.InternalServerError)
            {
                message = "Une erreur interne est survenue. Veuillez réessayer plus tard.";
            }
            else
            {
                message = ex.Message;
            }

            // 4. On construit et on envoie la réponse JSON
            var reponse = new
            {
                status = (int)statusCode,
                error = statusCode.ToString(),
                message = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(reponse));
        }
    }
}
