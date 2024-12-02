Si no esta instalado:

# Instalación de Versionado de APIs para ASP.NET Core
dotnet add package Microsoft.AspNetCore.Mvc.Versioning --version 5.1.0

# Instalación de autorizaciones en ASP.NET Core
dotnet add package Microsoft.AspNetCore.Authorization --version 8.0.0

# Instalación de las herramientas de Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.8

# Instalación de ASP.NET Core Identity con Entity Framework Core
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0

# Instalación de Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.8

# Instalación de las herramientas de diseño de Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.8

# Instalación del proveedor de SQL Server para Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.8

# Instalación de extensiones de configuración para ASP.NET Core
dotnet add package Microsoft.Extensions.Configuration --version 8.0.0

# Instalación de Swashbuckle (para documentación de APIs con Swagger)
dotnet add package Swashbuckle.AspNetCore --version 6.7.3


----------------------------------------------------------------------------------------
# Instalación de JWT Bearer Authentication para ASP.NET Core"NO ES NECESARIO PARA ESTE PROYECTO"
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0

# Instalación del paquete IdentityModel para trabajar con tokens JWT "NO NECESARIO PARA ESTE PROYECTO" 
dotnet add package Microsoft.IdentityModel.Tokens --version 8.0.0

# Instalación de MailKit (para trabajar con correos electrónicos) " NO ES NECESARIO PARA ESTE PROYECTO"
dotnet add package MailKit --version 4.7.1
------------------------------------------------------------------------------------------




Migracion: (para que cree la Base de Datos)


     1°  dotnet ef migrations add Initial

     2°  dotnet ef database update
     3°  dotnet run para ejecutar el proyecto 
 

     
