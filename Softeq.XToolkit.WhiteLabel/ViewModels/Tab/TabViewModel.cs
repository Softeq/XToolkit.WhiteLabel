// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    /// <summary>
    ///     Base view model to describe the root tab.
    /// </summary>
    /// <typeparam name="TKey">The type of the tab key.</typeparam>
    public abstract class TabViewModel<TKey> : RootFrameNavigationViewModelBase
    {
        private readonly TabItem<TKey> _tab;

        private string? _badgeText;
        private bool _isBadgeVisible;

        protected TabViewModel(
            IFrameNavigationService frameNavigationService,
            TabItem<TKey> tab)
            : base(frameNavigationService)
        {
            _tab = tab ?? throw new ArgumentNullException(nameof(tab));
        }

        /// <summary>
        ///     Gets or sets the text for the tab badge.
        /// </summary>
        public string? BadgeText
        {
            get => _badgeText;
            set => Set(ref _badgeText, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the tab badge is visible.
        /// </summary>
        public bool IsBadgeVisible
        {
            get => _isBadgeVisible;
            set => Set(ref _isBadgeVisible, value);
        }

        /// <inheritdoc cref="TabItem{TKey}.Title" />
        public string Title => _tab.Title;

        /// <inheritdoc cref="TabItem{TKey}.Key" />
        public TKey Key => _tab.Key;

        /// <inheritdoc cref="IFrameNavigationService.CanGoBack"/>
        public bool CanGoBack => FrameNavigationService.CanGoBack;

        /// <inheritdoc cref="IFrameNavigationService.GoBack()"/>
        public void GoBack() => FrameNavigationService.GoBack();
    }

    /// <summary>
    ///     The view model to describe the root tab with navigation to the first page.
    /// </summary>
    /// <typeparam name="TFirstViewModel">The type of first view model in the navigation stack.</typeparam>
    /// <typeparam name="TKey">The type of the tab key.</typeparam>
    public class TabViewModel<TFirstViewModel, TKey>
        : TabViewModel<TKey> where TFirstViewModel : IViewModelBase
    {
        public TabViewModel(
            IFrameNavigationService frameNavigationService,
            TabItem<TKey> tab)
            : base(frameNavigationService, tab)
        {
        }

        /// <inheritdoc cref="IFrameNavigationService.NavigateToFirstPage()"/>
        public override void NavigateToFirstPage()
        {
            // Check fast-backward nav by tab selected
            if (FrameNavigationService.IsEmptyBackStack)
            {
                FrameNavigationService.NavigateToViewModel<TFirstViewModel>(true);
            }
            else
            {
                FrameNavigationService.NavigateToFirstPage();
            }
        }
    }
}
