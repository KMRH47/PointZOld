using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using PointZ.Services.Navigation;
using Xamarin.Forms;

namespace PointZ.ViewModels.Base
{
    public abstract class ViewModelBase : BindableObject
    {
        protected readonly INavigationService NavigationService;

        protected ViewModelBase() => this.NavigationService = ViewModelLocator.Resolve<INavigationService>();

        protected void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            string name = GetMemberInfo(property).Name;
            Debug.WriteLine($"Property changed: {name}");
            OnPropertyChanged(name);
        }
 
        public virtual Task InitializeAsync(object parameter) => Task.FromResult(false);

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