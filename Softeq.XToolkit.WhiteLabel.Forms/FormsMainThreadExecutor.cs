// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Threading;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms
{
    public class FormsMainThreadExecutor : IMainThreadExecutor
    {
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
