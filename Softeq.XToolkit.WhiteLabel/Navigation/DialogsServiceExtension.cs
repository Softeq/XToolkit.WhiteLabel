﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public static class DialogsServiceExtension
    {
        public static DialogFluentNavigator<T> For<T>(this IDialogsService dialogsService) 
            where T : class, IDialogViewModel
        {
            return new DialogFluentNavigator<T>(dialogsService);
        }
    }
}
