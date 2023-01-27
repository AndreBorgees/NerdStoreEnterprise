using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Core.Specification
{
    internal sealed class AndSpecification<T>: Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();

            var invokedExpression = Expression.Invoke(rightExpression, leftExpression);

            return (Expression<Func<T, bool>>)Expression.Lambda(Expression.AndAlso(leftExpression.Body, invokedExpression), leftExpression.Parameters);
        }
    }
}
