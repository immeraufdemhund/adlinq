using System.Linq.Expressions;

namespace System.DirectoryServices.Linq.Expressions
{
	public class WhereExpression : Expression
	{
		#region Constructors

		public WhereExpression(LambdaExpression where)
		{
			Where = where;
		}

		#endregion

		#region Properties

		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)DirectoryExpressionType.Where;
			}
		}

		public override Type Type
		{
			get
			{
				return Where.Type;
			}
		}

		public LambdaExpression Where { get; private set; }

		#endregion

		#region Methods

		public Type GetParameterType()
		{
			if (Where.Parameters.Count == 1)
			{
				return Where.Parameters[0].Type;
			}

			return null;
		}

		#endregion
	}
}
