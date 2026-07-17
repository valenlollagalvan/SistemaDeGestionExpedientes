using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace SGE.Frontend.Services;

public class TokenAuthenticationStateProvider(LocalStorageService storage) : AuthenticationStateProvider
{
    public const string TokenKey = "sge.jwt";
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await storage.GetAsync(TokenKey);
        return new AuthenticationState(BuildPrincipal(token));
    }

    public async Task<string?> GetTokenAsync()
    {
        return await storage.GetAsync(TokenKey);
    }

    public async Task SetTokenAsync(string token)
    {
        await storage.SetAsync(TokenKey, token);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(BuildPrincipal(token))));
    }

    public async Task ClearTokenAsync()
    {
        await storage.RemoveAsync(TokenKey);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Anonymous)));
    }

    private static ClaimsPrincipal BuildPrincipal(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Anonymous;
        }

        try
        {
            var payload = token.Split('.')[1];
            var json = Encoding.UTF8.GetString(ParseBase64Url(payload));
            using var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("exp", out var expElement))
            {
                var expires = DateTimeOffset.FromUnixTimeSeconds(expElement.GetInt64());
                if (expires <= DateTimeOffset.UtcNow)
                {
                    return Anonymous;
                }
            }

            var claims = new List<Claim>();
            foreach (var property in doc.RootElement.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    claims.AddRange(property.Value.EnumerateArray().Select(item => new Claim(property.Name, item.ToString())));
                }
                else
                {
                    claims.Add(new Claim(property.Name, property.Value.ToString()));
                }
            }

            return new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        }
        catch
        {
            return Anonymous;
        }
    }

    private static byte[] ParseBase64Url(string value)
    {
        var base64 = value.Replace('-', '+').Replace('_', '/');
        base64 = base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        return Convert.FromBase64String(base64);
    }
}
