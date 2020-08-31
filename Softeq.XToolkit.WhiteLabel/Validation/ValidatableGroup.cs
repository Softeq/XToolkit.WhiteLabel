// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;

namespace Softeq.XToolkit.WhiteLabel.Validation
{
    public class ValidatableGroup : IValidatableObject
    {
        private readonly IValidatableObject[] _validatableObjects;

        public ValidatableGroup(params IValidatableObject[] validatableObjects)
        {
            _validatableObjects = validatableObjects;
        }

        public bool Validate()
        {
            return _validatableObjects
                .Select(x => x.Validate())
                .ToList()
                .All(x => x);
        }
    }
}
