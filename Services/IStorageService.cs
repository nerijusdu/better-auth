using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterAuth.Services;

public interface IStorageService
{
    Task SaveValue<T>(string key, T value);
    Task<T> GetValue<T>(string key);
    Task RemoveItem(string key);
}
