using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Common
{
    public static class ExpressionExtensions
    {
        public static string ToSqlString(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                    var add = expression as BinaryExpression;
                    return add.Left.ToSqlString() + " + " + add.Right.ToSqlString();
                case ExpressionType.Constant:
                    var constant = expression as ConstantExpression;
                    if (constant.Type == typeof(string))
                        return "N'" + constant.Value.ToString().Replace("'", "''") + "'";
                    return constant.Value.ToString();
                case ExpressionType.Equal:
                    var equal = expression as BinaryExpression;
                    return equal.Left.ToSqlString() + " = " +
                           equal.Right.ToSqlString();
                case ExpressionType.Lambda:
                    var l = expression as LambdaExpression;
                    return l.Body.ToSqlString();
                case ExpressionType.MemberAccess:
                    var memberaccess = expression as MemberExpression;
                    // todo: if column aliases are used, look up ColumnAttribute.Name
                    return "[" + memberaccess.Member.Name + "]";
                case ExpressionType.AndAlso:
                    var andExpr = expression as BinaryExpression;
                    return andExpr.Left.ToSqlString() + " AND (" + andExpr.Right.ToSqlString() + ")";
                case ExpressionType.OrElse:
                    var orExpr = expression as BinaryExpression;
                    return orExpr.Left.ToSqlString() + " OR (" + orExpr.Right.ToSqlString() + ")";
            }

            throw new NotImplementedException(
              expression.GetType().ToString() + " " +
              expression.NodeType.ToString());
        }
    }
}
