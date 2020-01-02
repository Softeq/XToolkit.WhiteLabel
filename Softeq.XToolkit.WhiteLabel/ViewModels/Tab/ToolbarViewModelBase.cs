// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    public abstract class ToolbarViewModelBase : ViewModelBase
    {
        public IList<TabItem> TabModels { get; protected set; }

        public IList<TabViewModel> TabViewModels { get; protected set; } = default!;

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