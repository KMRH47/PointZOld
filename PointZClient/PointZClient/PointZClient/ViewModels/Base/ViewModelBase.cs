using System;
using System.Linq.Expressions;
using System.Reflection;
using Xamarin.Forms;

namespace PointZClient.ViewModels.Base
{
    public class ViewModelBase : BindableObject
    {
        protected void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            string name = GetMemberInfo(property).Name;
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