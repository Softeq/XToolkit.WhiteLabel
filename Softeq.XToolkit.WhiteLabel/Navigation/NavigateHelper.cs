using System;
using System.Linq.Expressions;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class NavigateHelper<TViewModel> where TViewModel : IViewModelBase
    {
        private readonly TViewModel _viewModel;
        private readonly Action<bool> _navigateAction;

        public NavigateHelper(TViewModel viewModel, Action<bool> navigateAction)
        {
            _viewModel = viewModel;
            _navigateAction = navigateAction;
        }

        public NavigateHelper<TViewModel> WithParam<TValue>(Expression<Func<TViewModel, TValue>> property, TValue value)
        {
            SetMemberValue(property.GetMemberInfo(), _viewModel, value);
            
            return this;
        }

        public void Navigate(bool clearBackStack = false)
        {
            _navigateAction(clearBackStack);
        }

        private static void SetMemberValue(MemberInfo member, object target, object value)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo) member).SetValue(target, value);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo) member).SetValue(target, value, null);
                    break;
                default:
                    throw new ArgumentException("MemberInfo must be if type FieldInfo or PropertyInfo", nameof(member));
            }
        }
    }
}