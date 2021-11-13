using Microsoft.Maui.Essentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetterAuth.Services;

public class StorageService : IStorageService
{
    public async Task<T> GetValue<T>(string key)
    {
        var strValue = await SecureStorage.GetAsync(key);
        if (strValue == null)
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(strValue);
    }

    public Task RemoveItem(string key)
    {
        SecureStorage.Remove(key);
        return Task.CompletedTask;
    }

    public async Task SaveValue<T>(string key, T value)
    {
        var strValue = JsonSerializer.Serialize(value);
        await SecureStorage.SetAsync(key, strValue);
    }
}
