// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DryIoc;
using IContainer = Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract.IContainer;
using IDryContainer = DryIoc.IContainer;

[assembly: InternalsVisibleTo("Softeq.XToolkit.WhiteLabel.Tests")]

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Containers
{
    /// <summary>
    ///     An implementation of <see cref="IContainer"/> interface that resolves dependencies using
    ///     <see cref="T:DryIoc.IContainer"/> from <see href="https://github.com/dadhi/DryIoc">DryIoc</see> library.
    /// </summary>
    internal class DryIocContainerAdapter : IContainer
    {
        private IDryContainer _container = default!;

        /// <summary>
        ///     Initializes current service with the specified instance of <see cref="T:DryIoc.IContainer"/>.
        /// </summary>
        /// <param name="container">Instance of <see cref="T:DryIoc.IContainer"/>.</param>
        internal void Initialize(IDryContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <inheritdoc/>
        public TService Resolve<TService>(params object[] parameters) where TService : notnull
        {
            if (_container == null)
            {
                throw new InvalidOperationException("DryIocContainerAdapter is not initialized");
            }

            return _container.Resolve<TService>(parameters);
        }

        /// <inheritdoc/>
        public IEnumerable<TService> ResolveMany<TService>() where TService : notnull
        {
            if (_container == null)
            {
                throw new InvalidOperationException("DryIocContainerAdapter is not initialized");
            }

            return _container.ResolveMany<TService>(behavior: ResolveManyBehavior.AsFixedArray);
        }

        /// <inheritdoc/>
        public Lazy<TService> ResolveLazy<TService>() where TService : notnull
        {
            return new Lazy<TService>(() => _container.Resolve<TService>());
        }
    }
}
