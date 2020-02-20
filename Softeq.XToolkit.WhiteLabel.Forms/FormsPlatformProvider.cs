// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Threading;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms
{
    public class FormsPlatformProvider : IPlatformProvider
    {
        public bool InDesignMode => false;

        public void BeginOnUIThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        public Task OnUIThreadAsync(Action action)
        {
            return Device.InvokeOnMainThreadAsync(action);
        }

        public void OnUIThread(Action action)
        {
            throw new NotImplementedException("Method unsupported");
        }
    }
}
