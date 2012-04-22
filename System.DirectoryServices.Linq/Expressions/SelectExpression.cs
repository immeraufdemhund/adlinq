using System.Linq.Expressions;

namespace System.DirectoryServices.Linq.Expressions
{
	public class SelectExpression : Expression
	{
		#region Constructors

		public SelectExpression(LambdaExpression projection)
		{
			Projection = projection;
		}

		#endregion

		#region Properties

		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)DirectoryExpressionType.Select;
			}
		}

		public override Type Type
		{
			get
			{
				return Projection.Type;
			}
		}

		public LambdaExpression Projection { get; private set; }

		#endregion
	}
}
