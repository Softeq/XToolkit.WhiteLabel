// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.WhiteLabel.Validation
{
    public class ValidatableObject<T> : ObservableObject, IValidatableObject
    {
        private readonly Action<T>? _valueWasChanged;
        private readonly T _defaultValue;

        private T _value;
        private bool _isValid = true;

        public ValidatableObject(
            T defaultValue,
            Action<T>? valueWasChanged = null)
        {
            _defaultValue = defaultValue;
            _value = defaultValue;
            _valueWasChanged = valueWasChanged;

            Validations = new List<IValidationRule<T>>();
            Errors = new ObservableRangeCollection<string>();
        }

        public List<IValidationRule<T>> Validations { get; }

        public T Value
        {
            get => _value;
            set
            {
                if (Set(ref _value, value))
                {
                    _valueWasChanged?.Invoke(value);
                }

                var cleanWhenEmpty = CleanErrorsWhenEmpty && IsEmpty;

                if (CleanErrorsOnChange || cleanWhenEmpty)
                {
                    ResetErrors();
                }
            }
        }

        public bool IsValid
        {
            get => _isValid;
            private set
            {
                Set(ref _isValid, value);
                RaisePropertyChanged(nameof(FirstError));
            }
        }

        public ObservableRangeCollection<string> Errors { get; }

        public string FirstError => Errors.FirstOrDefault() ?? string.Empty;

        public bool CleanErrorsOnChange { get; set; }

        public bool CleanErrorsWhenEmpty { get; set; } = true;

        public virtual bool IsEmpty
        {
            get
            {
                if (string.IsNullOrEmpty(Value as string))
                {
                    return true;
                }

                return false;
            }
        }

        public void AddError(string message)
        {
            Errors.Add(message);
            IsValid = false;
        }

        public bool Validate()
        {
            Errors.Clear();

            var errors = Validations.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage)
                .ToList();

            Errors.AddRange(errors);
            IsValid = !errors.Any();

            return IsValid;
        }

        public void ResetValue()
        {
            Value = _defaultValue;
            ResetErrors();
        }

        public void ResetErrors()
        {
            Errors.Clear();
            IsValid = true;
        }
    }
}
