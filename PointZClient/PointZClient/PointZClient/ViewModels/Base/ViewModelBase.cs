using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using PointZClient.Services.Navigation;
using Xamarin.Forms;

namespace PointZClient.ViewModels.Base
{
    public class ViewModelBase : BindableObject
    {
        protected readonly INavigationService NavigationService;

        public ViewModelBase() => this.NavigationService = ViewModelLocator.Resolve<INavigationService>();

        protected void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            string name = GetMemberInfo(property).Name;
            Debug.WriteLine($"Property changed: {name}");
            OnPropertyChanged(name);
        }

        private static MemberInfo GetMemberInfo(Expression expression)
        {
            MemberExpression operand;
            LambdaExpression lambdaExpression = (LambdaExpression) expression;

            if (lambdaExpression.Body is UnaryExpression body)
                operand = (MemberExpression) body.Operand;
            else operand = (MemberExpression) lambdaExpression.Body;

            return operand.Member;
        }
    }
}