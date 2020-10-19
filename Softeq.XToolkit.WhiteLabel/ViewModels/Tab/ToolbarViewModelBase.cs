// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    public abstract class ToolbarViewModelBase<TKey> : ViewModelBase
    {
        private int _selectedIndex;

        public ToolbarViewModelBase()
        {
            TabModels = new List<TabItem<TKey>>();
            TabViewModels = new List<TabViewModel<TKey>>();
        }

        public IList<TabItem<TKey>> TabModels { get; protected set; }

        public IList<TabViewModel<TKey>> TabViewModels { get; protected set; }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value);
        }

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
