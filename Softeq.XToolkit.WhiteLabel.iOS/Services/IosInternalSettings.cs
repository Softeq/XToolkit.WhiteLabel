// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class IosInternalSettings : IInternalSettings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public bool AddOrUpdateValue(string key, decimal value)
        {
            return AppSettings.AddOrUpdateValue(key, value);
        }

        public bool AddOrUpdateValue(string key, bool value)
        {
            return AppSettings.AddOrUpdateValue(key, value);
        }

        public bool AddOrUpdateValue(string key, long value)
        {
            return AppSettings.AddOrUpdateValue(key, value);
        }

        public bool AddOrUpdateValue(string key, string value)
        {
            return AppSettings.AddOrUpdateValue(key, value);
        }

        public bool AddOrUpdateValue(string key, int value)
        {
            return AppSettings.AddOrUpdateValue(key, value);
        }

        public bool AddOrUpdateValue(string key, float value)
        {
            return AppSettings.AddOrUpdateValue(key, value);
        }

        public bool AddOrUpdateValue(string key, DateTime value)
        {
            return AppSettings.AddOrUpdateValue(key, value);
        }

        public bool AddOrUpdateValue(string key, Guid value)
        {
            return AppSettings.AddOrUpdateValue(key, value);
        }

        public bool AddOrUpdateValue(string key, double value)
        {
            return AppSettings.AddOrUpdateValue(key, value);
        }

        public decimal GetValueOrDefault(string key, decimal defaultValue = 0)
        {
            return AppSettings.GetValueOrDefault(key, defaultValue);
        }

        public bool GetValueOrDefault(string key, bool defaultValue = false)
        {
            return AppSettings.GetValueOrDefault(key, defaultValue);
        }

        public long GetValueOrDefault(string key, long defaultValue = 0)
        {
            return AppSettings.GetValueOrDefault(key, defaultValue);
        }

        public string GetValueOrDefault(string key, string defaultValue = null)
        {
            return AppSettings.GetValueOrDefault(key, defaultValue);
        }

        public int GetValueOrDefault(string key, int defaultValue = 0)
        {
            return AppSettings.GetValueOrDefault(key, defaultValue);
        }

        public float GetValueOrDefault(string key, float defaultValue = 0)
        {
            return AppSettings.GetValueOrDefault(key, defaultValue);
        }

        public DateTime GetValueOrDefault(string key, DateTime defaultValue = default)
        {
            return AppSettings.GetValueOrDefault(key, defaultValue);
        }

        public Guid GetValueOrDefault(string key, Guid defaultValue = default)
        {
            return AppSettings.GetValueOrDefault(key, defaultValue);
        }

        public double GetValueOrDefault(string key, double defaultValue = 0)
        {
            return AppSettings.GetValueOrDefault(key, defaultValue);
        }

        public void Clear()
        {
            AppSettings.Clear();
        }

        public bool Contains(string key)
        {
            return AppSettings.Contains(key);
        }

        public void Remove(string key)
        {
            AppSettings.Remove(key);
        }
    }
}
