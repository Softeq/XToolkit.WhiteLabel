﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface ICache
    {
        Task<DateTimeOffset?> GetExpiration(string key);

        Task<T> Get<T>(string key);

        Task Add(string key, DateTimeOffset date, object value);

        Task<bool> Exists(string key);

        Task<bool> IsExpired(string key);

        void Reset();
    }
}
