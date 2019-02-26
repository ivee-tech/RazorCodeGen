using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Linq.Dynamic;


namespace Iv.Common
{
    public static class ExpressionHelper<M, E>
    {
        public static Expression<Func<E, bool>> ToECond(Expression<Func<M, bool>> mCond)
        {
            if (mCond == null) 
                return null;
            string paramName = mCond.Parameters[0].Name;
            string expression = ((LambdaExpression)mCond).ToString();
            expression = Regex.Replace(expression, @"\b" + paramName + @"\b\.?", "");
            expression = expression.Replace("=>", "").Trim();
            LambdaExpression expr = System.Linq.Dynamic.DynamicExpression.ParseLambda(typeof(E), typeof(bool), expression, null);
            return (Expression<Func<E, bool>>)expr;
        }

        public static Expression<Func<E, bool>> ToECond(Condition cond)
        {
            if (cond == null)
                return null;
            string expression = cond.ToLambdaString();
            LambdaExpression expr = System.Linq.Dynamic.DynamicExpression.ParseLambda(typeof(E), typeof(bool), expression, null);
            return (Expression<Func<E, bool>>)expr;
       }
    }
}
