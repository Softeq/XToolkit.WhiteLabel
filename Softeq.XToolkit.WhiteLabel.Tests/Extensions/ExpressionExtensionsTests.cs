// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Extensions;

public class ExpressionExtensionsTests
{
    [Fact]
    public void GetMemberInfo_Null_ThrowsNullReferenceException()
    {
        Assert.Throws<NullReferenceException>(() =>
        {
            ExpressionExtensions.GetMemberInfo(null!);
        });
    }

    [Fact]
    public void GetMemberInfo_ConstantExpression_ThrowsInvalidCastException()
    {
        const string TestStr = "test";

        Assert.Throws<InvalidCastException>(() =>
        {
            ExpressionExtensions.GetMemberInfo(() => TestStr);
        });
    }

    [Fact]
    public void GetMemberInfo_ParameterExpression_ThrowsInvalidCastException()
    {
        Assert.Throws<InvalidCastException>(() =>
        {
            ExpressionExtensions.GetMemberInfo((bool a) => !a);
        });
    }

    [Fact]
    public void GetMemberInfo_FieldExpression_ReturnsMemberInfoWithCorrectName()
    {
        string testField = "test";
        Expression<Func<string>> expression = () => testField;
        MemberInfo expected = ((MemberExpression)expression.Body).Member;

        var result = expression.GetMemberInfo();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetMemberInfo_UnaryExpression_ReturnsMemberInfoWithCorrectName()
    {
        Expression<Func<Tuple<bool>, bool>> expression = arg => !arg.Item1;
        MemberInfo expected = ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member;

        var result = expression.GetMemberInfo();

        Assert.Equal(expected, result);
    }
}
