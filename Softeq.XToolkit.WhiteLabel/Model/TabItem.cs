// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Model
{
    /// <summary>
    ///     Model that represents a single tab.
    /// </summary>
    /// <typeparam name="TKey">Type of the tab key.</typeparam>
    public abstract class TabItem<TKey>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TabItem{TKey}"/> class.
        /// </summary>
        /// <param name="title">Tab title.</param>
        /// <param name="key">Tab key.</param>
        protected TabItem(string title, TKey key)
        {
            Title = title;
            Key = key;
        }

        /// <summary>
        ///     Gets the tab title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        ///     Gets the tab key.
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        ///     Creates <see cref="T:Softeq.XToolkit.WhiteLabel.ViewModels.Tab.TabViewModel`1"/>
        ///     and initializes it with the current <see cref="TabItem{TKey}"/>.
        /// </summary>
        /// <returns> Created TabViewModel.</returns>
        public abstract TabViewModel<TKey> CreateViewModel();
    }

    /// <typeparam name="TFirstViewModel">Type of the ViewModel of the first tab.</typeparam>
    /// <inheritdoc/>
    public class TabItem<TFirstViewModel, TKey> : TabItem<TKey> where TFirstViewModel : ViewModelBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TabItem{TFirstViewModel, TKey}"/> class.
        /// </summary>
        /// <param name="title">Tab title.</param>
        /// <param name="key">Tab key.</param>
        /// <param name="container">
        ///     Implementation of DI container that is required
        ///     for resolving the implementation of <see cref="T:Softeq.XToolkit.WhiteLabel.Navigation.IFrameNavigationService"/>.
        /// </param>
        public TabItem(string title, TKey key, IContainer container)
            : base(title, key)
        {
            Container = container;
        }

        /// <summary>
        ///     Gets the implementation of DI container.
        /// </summary>
        protected IContainer Container { get; }

        /// <inheritdoc/>
        public override TabViewModel<TKey> CreateViewModel()
        {
            var frameNavigationService = Container.Resolve<IFrameNavigationService>();
            var tabViewModel = new TabViewModel<TFirstViewModel, TKey>(frameNavigationService);
            tabViewModel.Initialize(this);
            return tabViewModel;
        }
    }
}