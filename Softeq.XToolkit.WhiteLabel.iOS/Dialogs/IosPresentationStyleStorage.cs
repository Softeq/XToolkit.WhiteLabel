using System;
using System.Collections.Generic;

namespace Softeq.XToolkit.WhiteLabel.iOS.Dialogs
{
    public class IosPresentationStyleStorage
    {
        private readonly IDictionary<string, Type> _presentationStyles;

        public IosPresentationStyleStorage()
        {
            _presentationStyles = new Dictionary<string, Type>();
        }

        public void Initialize(ICollection<KeyValuePair<string, Type>> presentationStyles)
        {
            foreach(var pair in presentationStyles)
            {
                _presentationStyles.Add(pair);
            }
        }

        public PresentationArgsBase GetStyle(string id)
        {
            return (PresentationArgsBase) Activator.CreateInstance(_presentationStyles[id]);
        }
    }
}
