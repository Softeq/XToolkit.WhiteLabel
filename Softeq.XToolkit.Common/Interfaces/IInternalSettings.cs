// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Interfaces
{
    /// <summary>
    ///     Represents methods for application settings.
    /// </summary>
    public interface IInternalSettings
    {
        /// <summary>
        ///     Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateValue(string key, decimal value);

        /// <summary>
        ///     Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateValue(string key, bool value);

        /// <summary>
        ///     Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateValue(string key, long value);

        /// <summary>
        ///     Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateValue(string key, string value);

        /// <summary>
        ///     Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateValue(string key, int value);

        /// <summary>
        ///     Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateValue(string key, float value);

        /// <summary>
        ///     Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateValue(string key, DateTime value);

        /// <summary>
        ///     Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateValue(string key, Guid value);

        /// <summary>
        ///     Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateValue(string key, double value);

        /// <summary>
        ///     Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        decimal GetValueOrDefault(string key, decimal defaultValue = default);

        /// <summary>
        ///     Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        bool GetValueOrDefault(string key, bool defaultValue = default);

        /// <summary>
        ///     Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        long GetValueOrDefault(string key, long defaultValue = default);

        /// <summary>
        ///     Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        string GetValueOrDefault(string key, string defaultValue = default);

        /// <summary>
        ///     Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        int GetValueOrDefault(string key, int defaultValue = default);

        /// <summary>
        ///     Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        float GetValueOrDefault(string key, float defaultValue = default);

        /// <summary>
        ///     Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        DateTime GetValueOrDefault(string key, DateTime defaultValue = default);

        /// <summary>
        ///     Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        Guid GetValueOrDefault(string key, Guid defaultValue = default);

        /// <summary>
        ///     Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        double GetValueOrDefault(string key, double defaultValue = default);

        /// <summary>
        ///     Clear all keys from settings
        /// </summary>
        void Clear();

        /// <summary>
        ///     Checks to see if the key has been added.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if contains key, else false</returns>
        bool Contains(string key);

        /// <summary>
        ///     Removes a desired key from the settings
        /// </summary>
        /// <param name="key">Key for setting</param>
        void Remove(string key);
    }
}
