// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Microsoft.Maui.Storage;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Services
{
    public class InternalSettings : IInternalSettings
    {
        public void AddOrUpdateValue(string key, decimal value)
        {
            Preferences.Set(key, decimal.ToDouble(value));
        }

        public void AddOrUpdateValue(string key, bool value)
        {
            Preferences.Set(key, value);
        }

        public void AddOrUpdateValue(string key, long value)
        {
            Preferences.Set(key, value);
        }

        public void AddOrUpdateValue(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public void AddOrUpdateValue(string key, int value)
        {
            Preferences.Set(key, value);
        }

        public void AddOrUpdateValue(string key, float value)
        {
            Preferences.Set(key, value);
        }

        public void AddOrUpdateValue(string key, DateTime value)
        {
            Preferences.Set(key, value);
        }

        public void AddOrUpdateValue(string key, Guid value)
        {
            Preferences.Set(key, value.ToString());
        }

        public void AddOrUpdateValue(string key, double value)
        {
            Preferences.Set(key, value);
        }

        public decimal GetValueOrDefault(string key, decimal defaultValue = default)
        {
            return (decimal)Preferences.Get(key, decimal.ToDouble(defaultValue));
        }

        public bool GetValueOrDefault(string key, bool defaultValue = default)
        {
            return Preferences.Get(key, defaultValue);
        }

        public long GetValueOrDefault(string key, long defaultValue = default)
        {
            return Preferences.Get(key, defaultValue);
        }

        public string GetValueOrDefault(string key, string? defaultValue = default)
        {
            return Preferences.Get(key, defaultValue);
        }

        public int GetValueOrDefault(string key, int defaultValue = default)
        {
            return Preferences.Get(key, defaultValue);
        }

        public float GetValueOrDefault(string key, float defaultValue = default)
        {
            return Preferences.Get(key, defaultValue);
        }

        public DateTime GetValueOrDefault(string key, DateTime defaultValue = default)
        {
            return Preferences.Get(key, defaultValue);
        }

        public Guid GetValueOrDefault(string key, Guid defaultValue = default)
        {
            var value = Preferences.Get(key, defaultValue.ToString());
            return Guid.Parse(value);
        }

        public double GetValueOrDefault(string key, double defaultValue = default)
        {
            return Preferences.Get(key, defaultValue);
        }

        public void Clear()
        {
            Preferences.Clear();
        }

        public bool Contains(string key)
        {
            return Preferences.ContainsKey(key);
        }

        public void Remove(string key)
        {
            Preferences.Remove(key);
        }
    }
}
