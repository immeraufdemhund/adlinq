using System.DirectoryServices.Linq.Expressions;
using System.Linq.Expressions;

namespace System.DirectoryServices.Linq.EntryObjects
{
	public class EntryQueryState
	{
		private DirectoryExpressionVisitor _visitor;

		public EntryQueryState(DirectoryContext context, Expression expression)
		{
			Context = context;
			EntryType = expression.Type;
			Expression = expression;
		}

		public EntryQueryState(DirectoryContext context, Type entryType, Expression expression)
		{
			Context = context;
			EntryType = entryType;
			Expression = expression;
		}

		internal DirectoryExpressionVisitor Visitor
		{
			get
			{
				if (_visitor == null)
				{
					_visitor = new DirectoryExpressionVisitor();
				}

				return _visitor;
			}
		}

		public Type EntryType { get; private set; }

		public DirectoryContext Context { get; private set; }

		public Expression Expression { get; private set; }

		internal DirectoryExpression GetExpression()
		{
			var expression = new DirectoryExpression(Expression);
			return Visitor.VisitDirectory(expression);
		}

		internal SingleResultExpression GetSingleResultExpression()
		{
			var expression = new SingleResultExpression(Expression);
			return (SingleResultExpression)Visitor.VisitDirectory(expression);
		}
	}
}
