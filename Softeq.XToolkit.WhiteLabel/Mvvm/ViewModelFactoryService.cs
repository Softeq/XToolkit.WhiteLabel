// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    /// <summary>
    ///     Implementation of a factory service that resolves ViewModel dependencies
    ///     using the built-in IoC container.
    /// </summary>
    public class ViewModelFactoryService : IViewModelFactoryService
    {
        private readonly IContainer _iocContainer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewModelFactoryService"/> class.
        /// </summary>
        /// <param name="container">IoC container to resolve dependencies.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="container"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        public ViewModelFactoryService(
            IContainer container)
        {
            _iocContainer = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <inheritdoc/>
        public TViewModel ResolveViewModel<TViewModel, TParam>(TParam param)
            where TViewModel : ObservableObject, IViewModelParameter<TParam>
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.Parameter = param;
            return viewModel;
        }

        /// <inheritdoc/>
        public TViewModel ResolveViewModel<TViewModel>() where TViewModel : ObservableObject
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            return viewModel;
        }
    }
}