using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SGE.Frontend.Services;

public class ApiClient(HttpClient httpClient, TokenAuthenticationStateProvider authState)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<T?> GetAsync<T>(string url)
    {
        using var request = await CreateRequestAsync(HttpMethod.Get, url);
        using var response = await httpClient.SendAsync(request);
        await EnsureSuccessAsync(response);
        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    public async Task<T?> PostAsync<T>(string url, object body)
    {
        using var request = await CreateRequestAsync(HttpMethod.Post, url, body);
        using var response = await httpClient.SendAsync(request);
        await EnsureSuccessAsync(response);
        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    public async Task PutAsync(string url, object body)
    {
        using var request = await CreateRequestAsync(HttpMethod.Put, url, body);
        using var response = await httpClient.SendAsync(request);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteAsync(string url)
    {
        using var request = await CreateRequestAsync(HttpMethod.Delete, url);
        using var response = await httpClient.SendAsync(request);
        await EnsureSuccessAsync(response);
    }

    private async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string url, object? body = null)
    {
        var request = new HttpRequestMessage(method, url);
        var token = await authState.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        if (body is not null)
        {
            request.Content = JsonContent.Create(body, options: JsonOptions);
        }

        return request;
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var content = await response.Content.ReadAsStringAsync();
        var message = TryReadMessage(content);

        message ??= response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => "La sesion vencio o no se envio un token valido.",
            HttpStatusCode.Forbidden => "No tenes permisos para realizar esta accion.",
            HttpStatusCode.NotFound => "No se encontro el recurso solicitado.",
            _ => "La API rechazo la solicitud."
        };

        throw new ApiException((int)response.StatusCode, message);
    }

    private static string? TryReadMessage(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        try
        {
            using var doc = JsonDocument.Parse(content);
            if (doc.RootElement.TryGetProperty("message", out var message))
            {
                return message.GetString();
            }

            if (doc.RootElement.TryGetProperty("detail", out var detail))
            {
                return detail.GetString();
            }

            if (doc.RootElement.TryGetProperty("title", out var title))
            {
                return title.GetString();
            }
        }
        catch
        {
            return content;
        }

        return null;
    }
}

public class ApiException(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}
