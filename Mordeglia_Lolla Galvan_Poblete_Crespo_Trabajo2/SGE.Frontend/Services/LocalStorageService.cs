using Microsoft.JSInterop;

namespace SGE.Frontend.Services;

public class LocalStorageService(IJSRuntime jsRuntime)
{
    public ValueTask SetAsync(string key, string value)
    {
        return jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public ValueTask<string?> GetAsync(string key)
    {
        return jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
    }

    public ValueTask RemoveAsync(string key)
    {
        return jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
}
