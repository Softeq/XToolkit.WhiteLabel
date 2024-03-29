// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq.Expressions;
using System.Reflection;

namespace Softeq.XToolkit.WhiteLabel.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:System.Linq.Expressions.Expression"/>.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        ///     Converts an expression into a <see cref="T:System.Reflection.MemberInfo" />.
        /// </summary>
        /// <param name="expression">The expression to convert.</param>
        /// <returns>The member info.</returns>
        public static MemberInfo GetMemberInfo(this Expression expression)
        {
            var lambda = (LambdaExpression) expression;

            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression unaryExpression)
            {
                memberExpression = (MemberExpression) unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression) lambda.Body;
            }

            return memberExpression.Member;
        }
    }
}
