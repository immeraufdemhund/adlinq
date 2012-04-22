using System.Linq.Expressions;

namespace System.DirectoryServices.Linq.Expressions
{
	public class OrderByExpression : Expression
	{
		#region Constructors

		public OrderByExpression(MemberExpression expression) : this(expression, OrderByDirection.Ascending)
		{
		}

		public OrderByExpression(MemberExpression expression, OrderByDirection direction)
		{
			OrderByProperty = expression;
			Direction = direction;
		}

		#endregion

		#region Properties

		public override Type Type
		{
			get
			{
				return OrderByProperty.Type;
			}
		}

		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)DirectoryExpressionType.OrderBy;
			}
		}

		public OrderByDirection Direction { get; internal set; }

		public MemberExpression OrderByProperty { get; private set; }

		#endregion
	}
}
