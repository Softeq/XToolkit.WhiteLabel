﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Commands;

namespace Playground.ViewModels.Collections.Products
{
    public class ProductHeaderViewModel
    {
        public int Id { get; set; }

        public string Category { get; set; } = string.Empty;

        public ICommand<ProductHeaderViewModel>? AddCommand { get; set; }

        public ICommand<ProductHeaderViewModel>? InfoCommand { get; set; }

        public ICommand<ProductHeaderViewModel>? GenerateCommand { get; set; }

        #region for Collect to Group

        public override bool Equals(object obj)
        {
            var headerObj = (ProductHeaderViewModel) obj;

            return Category == headerObj.Category;
        }

        public override int GetHashCode()
        {
            return Category == null ? 0 : Category.GetHashCode();
        }

        #endregion
    }
}
