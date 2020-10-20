// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    /// <summary>
    ///     Base view model to describe tabbed components.
    /// </summary>
    /// <typeparam name="TKey">The type of the tab key.</typeparam>
    public abstract class ToolbarViewModelBase<TKey> : ViewModelBase
    {
        private int _selectedIndex;

        protected ToolbarViewModelBase()
        {
            TabModels = new List<TabItem<TKey>>();
            TabViewModels = new List<TabViewModel<TKey>>();
        }

        /// <summary>
        ///     Gets or sets the list of tab models.
        /// </summary>
        public IList<TabItem<TKey>> TabModels { get; protected set; }

        /// <summary>
        ///     Gets or sets the list of tab view models.
        /// </summary>
        public IList<TabViewModel<TKey>> TabViewModels { get; protected set; }

        /// <summary>
        ///     Gets or sets the index of the selected tab.
        /// </summary>
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value);
        }

        /// <inheritdoc />
        public override void OnInitialize()
        {
            base.OnInitialize();

            if (TabModels == null)
            {
                throw new InvalidOperationException($"You must init {nameof(TabModels)} property");
            }

            TabViewModels = TabModels
                .Select(x => x.CreateViewModel())
                .ToList();
        }
    }
}
