// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.OS;
using AndroidX.Fragment.App;

namespace Softeq.XToolkit.WhiteLabel.Droid.Internal
{
    internal sealed class ViewModelStorageFragment : Fragment, IInstanceStorage
    {
        private const string ViewModelStoreIdKey = "WL_ViewModelStoreId";

        private bool _destroyedBySystem;

        private string ViewModelStoreId { get; set; } = Guid.NewGuid().ToString();

        /// <inheritdoc />
        public TViewModel Get<TViewModel>(string fragmentName)
            where TViewModel : class
        {
            var viewModel = ViewModelCache.Get<TViewModel>(ViewModelStoreId, fragmentName);

            if (viewModel != null)
            {
                return viewModel;
            }

            // Important case, when app process was restarted:
            // If in-memory ViewModelCache was cleared -> create new ViewModel
            return Dependencies.Container.Resolve<TViewModel>();
        }

        /// <inheritdoc />
        public void Add<TViewModel>(string fragmentName, TViewModel viewModel)
            where TViewModel : class
        {
            ViewModelCache.Add(ViewModelStoreId, fragmentName, viewModel);
        }

        /// <inheritdoc />
        public void Remove(string fragmentName)
        {
            ViewModelCache.Remove(ViewModelStoreId, fragmentName);
        }

        /// <inheritdoc />
        public void Remove(IEnumerable<string> fragmentNames)
        {
            ViewModelCache.Remove(ViewModelStoreId, fragmentNames);
        }

        /// <inheritdoc />
        public void Clear()
        {
            ViewModelCache.Clear(ViewModelStoreId);
        }

        /// <inheritdoc />
        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (savedInstanceState != null)
            {
                ViewModelStoreId = savedInstanceState.GetString(ViewModelStoreIdKey);
            }

            RetainInstance = true;
        }

        /// <inheritdoc />
        public override void OnResume()
        {
            base.OnResume();

            _destroyedBySystem = false;
        }

        /// <inheritdoc />
        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutString(ViewModelStoreIdKey, ViewModelStoreId);

            _destroyedBySystem = true;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            base.OnDestroy();

            if (!_destroyedBySystem)
            {
                Clear();
            }
        }
    }
}
