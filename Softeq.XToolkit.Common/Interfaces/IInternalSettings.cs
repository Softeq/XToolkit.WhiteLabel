// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface IInternalSettings
    {
        bool AddOrUpdateValue(string key, decimal value);

        bool AddOrUpdateValue(string key, bool value);

        bool AddOrUpdateValue(string key, long value);

        bool AddOrUpdateValue(string key, string value);

        bool AddOrUpdateValue(string key, int value);

        bool AddOrUpdateValue(string key, float value);

        bool AddOrUpdateValue(string key, DateTime value);

        bool AddOrUpdateValue(string key, Guid value);

        bool AddOrUpdateValue(string key, double value);

        decimal GetValueOrDefault(string key, decimal defaultValue = default(decimal));

        bool GetValueOrDefault(string key, bool defaultValue = default(bool));

        long GetValueOrDefault(string key, long defaultValue = default(long));

        string GetValueOrDefault(string key, string defaultValue = default(string));

        int GetValueOrDefault(string key, int defaultValue = default(int));

        float GetValueOrDefault(string key, float defaultValue = default(float));

        DateTime GetValueOrDefault(string key, DateTime defaultValue = default(DateTime));

        Guid GetValueOrDefault(string key, Guid defaultValue = default(Guid));

        double GetValueOrDefault(string key, double defaultValue = default(double));

        void Clear();

        bool Contains(string key);

        void Remove(string key);
    }
}