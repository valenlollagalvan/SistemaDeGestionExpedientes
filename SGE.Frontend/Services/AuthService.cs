using System.Net.Http.Json;
using System.Text.Json;
using SGE.Frontend.Models;

namespace SGE.Frontend.Services;

public class AuthService(HttpClient httpClient, TokenAuthenticationStateProvider authState)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task LoginAsync(string correoElectronico, string contrasena)
    {
        var response = await httpClient.PostAsJsonAsync("/api/auth/login", new LoginRequest(correoElectronico, contrasena), JsonOptions);
        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException((int)response.StatusCode, "No se pudo iniciar sesion. Revisá el correo y la contrasena.");
        }

        var login = await response.Content.ReadFromJsonAsync<LoginResponse>(JsonOptions)
            ?? throw new ApiException(500, "La API no devolvio un token.");

        await authState.SetTokenAsync(login.Token);
    }

    public async Task<Guid> RegistrarAsync(string nombre, string correoElectronico, string contrasena)
    {
        var response = await httpClient.PostAsJsonAsync("/api/auth/registrar", new RegistrarUsuarioRequest(nombre, correoElectronico, contrasena), JsonOptions);
        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException((int)response.StatusCode, "No se pudo registrar el usuario.");
        }

        var registro = await response.Content.ReadFromJsonAsync<RegistrarUsuarioResponse>(JsonOptions)
            ?? throw new ApiException(500, "La API no devolvio el usuario creado.");

        return registro.IdUsuario;
    }

    public Task LogoutAsync()
    {
        return authState.ClearTokenAsync();
    }
}
