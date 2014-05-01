using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentIbatis.Core.Utils
{
    public class ReflectionHelper
    {
        public static PropertyInfo GetProperty<TModel>(Expression<Func<TModel, object>> expression)
        {
            var isExpressionOfDynamicComponent = expression.ToString().Contains("get_Item");

            if (isExpressionOfDynamicComponent)
                return GetDynamicComponentProperty(expression);

            var memberExpression = GetMemberExpression(expression);

            return (PropertyInfo)memberExpression.Member;
        }

        private static PropertyInfo GetDynamicComponentProperty<TModel, T>(Expression<Func<TModel, T>> expression)
        {
            Type desiredConversionType = null;
            MethodCallExpression methodCallExpression = null;
            var nextOperand = expression.Body;

            while (nextOperand != null)
            {
                if (nextOperand.NodeType == ExpressionType.Call)
                {
                    methodCallExpression = nextOperand as MethodCallExpression;
                    desiredConversionType = desiredConversionType ?? methodCallExpression.Method.ReturnType;
                    break;
                }

                if (nextOperand.NodeType != ExpressionType.Convert)
                    throw new ArgumentException("Expression not supported", "expression");

                var unaryExpression = (UnaryExpression)nextOperand;
                desiredConversionType = unaryExpression.Type;
                nextOperand = unaryExpression.Operand;
            }

            var constExpression = methodCallExpression.Arguments[0] as ConstantExpression;

            return new DummyPropertyInfo((string)constExpression.Value, desiredConversionType);
        }

        public static PropertyInfo GetProperty<TModel, T>(Expression<Func<TModel, T>> expression)
        {
            var isExpressionOfDynamicComponent = expression.ToString().Contains("get_Item");

            if (isExpressionOfDynamicComponent)
                return GetDynamicComponentProperty(expression);

            MemberExpression memberExpression = GetMemberExpression(expression);

            return (PropertyInfo)memberExpression.Member;
        }

        private static MemberExpression GetMemberExpression<TModel, T>(Expression<Func<TModel, T>> expression)
        {
            return GetMemberExpression(expression, true);
        }

        private static MemberExpression GetMemberExpression<TModel, T>(Expression<Func<TModel, T>> expression, bool enforceCheck)
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (enforceCheck && memberExpression == null)
            {
                throw new ArgumentException("Not a member access", "expression");
            }

            return memberExpression;
        }


    }
}