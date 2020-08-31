// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.WhiteLabel.Validation
{
    public class ValidatableObject<T> : ObservableObject, IValidatableObject
    {
        private readonly IList<IValidationRule<T>> _validationRules;

        private T _value = default!;
        private bool _isValid = true;

        public ValidatableObject()
        {
            _validationRules = new List<IValidationRule<T>>();

            Errors = new ObservableRangeCollection<string>();
            Errors.CollectionChanged += ErrorsCollectionChanged;

            CleanErrorsOnChange = true;
        }

        public bool CleanErrorsOnChange { get; set; }

        public T Value
        {
            get => _value;
            set
            {
                if (Set(ref _value, value) && CleanErrorsOnChange)
                {
                    Errors.Clear();
                }
            }
        }

        public bool IsValid
        {
            get => _isValid;
            private set => Set(ref _isValid, value);
        }

        public ObservableRangeCollection<string> Errors { get; }

        public string FirstError => Errors.FirstOrDefault() ?? string.Empty;

        public void AddRule(IValidationRule<T> rule)
        {
            _validationRules.Add(rule);
        }

        public virtual bool Validate()
        {
            Errors.Clear();

            var errors = _validationRules
                .Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors.AddRange(errors);

            return IsValid;
        }

        private void ErrorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableRangeCollection<string> errors)
            {
                IsValid = errors.Count == 0;
                RaisePropertyChanged(nameof(FirstError));
            }
        }
    }
}
