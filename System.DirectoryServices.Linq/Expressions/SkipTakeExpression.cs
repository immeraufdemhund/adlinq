using System.Linq.Expressions;

namespace System.DirectoryServices.Linq.Expressions
{
	public class SkipTakeExpression : Expression
	{
		#region Fields

		private readonly DirectoryExpressionType _nodeType;

		#endregion

		#region Constructors

		public SkipTakeExpression(ConstantExpression skipTake, DirectoryExpressionType nodeType)
		{
			if (nodeType != DirectoryExpressionType.Take && nodeType != DirectoryExpressionType.Skip)
			{
				throw new Exception();
			}

			SkipTake = skipTake;
			_nodeType = nodeType;
		}

		#endregion

		#region Properties

		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)_nodeType;
			}
		}

		public override Type Type
		{
			get
			{
				return SkipTake.Type;
			}
		}

		public int Amount
		{
			get
			{
				return (int)SkipTake.Value;
			}
		}

		public ConstantExpression SkipTake { get; private set; }

		#endregion
	}
}
