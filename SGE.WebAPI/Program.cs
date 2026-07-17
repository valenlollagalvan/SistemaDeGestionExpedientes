using SGE.WebAPI.Dependencias;
using SGE.WebAPI.Endpoints;
using SGE.WebAPI.Middlewares;
using Scalar.AspNetCore;
using SGE.Aplicacion; 
using SGE.Infraestructura.Extensiones;
using SGE.Infraestructura.Datos; 
using SGE.Aplicacion.Usuarios; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi()
    .AddAplicacion() 
    .AddInfraestructura(builder.Configuration) 
    .AddAutenticacionJWT(builder.Configuration) 
    .AddProblemDetails()
    .AddExceptionHandler<ExcepcionGlobalMiddleware>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendLocal", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddHealthChecks()
    .AddDbContextCheck<SgeContext>("database");

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("auth", httpContext =>
        System.Threading.RateLimiting.RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new System.Threading.RateLimiting.FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SgeContext>();
    var hashService = scope.ServiceProvider.GetRequiredService<IHashContrasenaService>(); 

    SgeSqlite.Inicializar(context, hashService);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseExceptionHandler();
app.UseCors("FrontendLocal");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => "¡La API del SGE está funcionando correctamente!");
app.MapHealthChecks("/health").AllowAnonymous();
app.MapAutorizacionEndpoints();
app.MapExpedienteEndpoints();
app.MapTramiteEndpoints();
app.MapUsuariosEndpoints();

app.Run();
