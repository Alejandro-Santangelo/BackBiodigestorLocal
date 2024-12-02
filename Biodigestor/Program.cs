
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Biodigestor.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Biodigestor.Models;
using Biodigestor.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

// Agregar servicios al contenedor.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ClienteDniAuthorizationFilter>();
});

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder
            .WithOrigins("http://localhost:4200") // URL específica de tu aplicación Angular
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // Esto permite el envío de credenciales
    });
});

// Registrar la base de datos con EF Core
builder.Services.AddDbContext<BiodigestorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar autenticación basada en cookies
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "AuthCookie";
        options.LoginPath = "/Auth/login";
        options.Cookie.SameSite = SameSiteMode.None; // Importante para CORS
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Recomendado para producción
    });

// Registrar el filtro personalizado para la autorización por DNI
builder.Services.AddScoped<ClienteDniAuthorizationFilter>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Biodigestor API", Version = "v1" });
});

var app = builder.Build();

// Configurar la tubería de solicitudes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biodigestor API V1");
        c.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Agregar redirección de la raíz a index.html
app.MapGet("/", context => {
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});

// Habilitar CORS - Debe ir antes de Authentication y Authorization
app.UseCors("AllowAllOrigins");

// Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();