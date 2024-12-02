using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Biodigestor.Filters
{
public class ClienteDniAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var userRole = user.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        var userDni = user.Claims.FirstOrDefault(c => c.Type == "Dni")?.Value;

        // Verificar si el usuario es del rol "Cliente"
        if (userRole == "Cliente")
        {
            // Obtener el DNI del cliente autenticado
            if (userDni == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Verificar si la solicitud contiene el parámetro DNI (por ejemplo, en la ruta o en el query string)
            var routeDni = context.RouteData.Values["dni"]?.ToString(); // si el DNI está en la ruta
            var queryDni = context.HttpContext.Request.Query["dni"].ToString(); // si el DNI está en el query string

            if ((routeDni != null && routeDni != userDni) || (queryDni != null && queryDni != userDni))
            {
                context.Result = new ForbidResult("No tienes permiso para acceder a la información de otro usuario.");
                return;
            }
        }

        // Si es del rol "Administracion", no hacemos ninguna restricción
    }
}
}
