using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.Linq.EntryObjects;
using System.Linq;
using System.Linq.Expressions;

namespace System.DirectoryServices.Linq.Expressions
{
	public class DirectoryExpression : Expression
	{
		#region Fields

		private Type _origionalType;
		private Expression _expression;
		private SelectExpression _select;
		private SkipTakeExpression _skip;
		private SkipTakeExpression _take;
		private OrderByExpression _orderBy;
		private List<WhereExpression> _whereClause;

		#endregion

		#region Constructors

		public DirectoryExpression(Expression expression)
		{
			_expression = expression;
			_whereClause = new List<WhereExpression>();
		}

		#endregion

		#region Properties

		public override bool CanReduce
		{
			get
			{
				return true;
			}
		}

		public override Type Type
		{
			get
			{
				return _expression.Type;
			}
		}

		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)DirectoryExpressionType.Directory;
			}
		}

		public Expression RootExpression
		{
			get
			{
				return _expression;
			}
		}

		public OrderByExpression OrderBy
		{
			get
			{
				return _orderBy;
			}
		}

		public SelectExpression Select
		{
			get
			{
				return _select;
			}
		}

		public SkipTakeExpression Skip
		{
			get
			{
				return _skip;
			}
		}

		public SkipTakeExpression Take
		{
			get
			{
				return _take;
			}
		}

		public ReadOnlyCollection<WhereExpression> WhereClause
		{
			get
			{
				return _whereClause.AsReadOnly();
			}
		}

		#endregion

		#region Methods

		public override Expression Reduce()
		{
			return _expression;
		}

		public Type GetOrigionalType()
		{
			if (_origionalType == null)
			{
				_origionalType = GetOrigionalType(_expression);
			}

			return _origionalType;
		}

		public Type GetOrigionalType(Expression expression)
		{
			var methodCall = expression as MethodCallExpression;

			if (methodCall != null)
			{
				var constant = methodCall.Arguments[0] as ConstantExpression;

				if (constant != null && constant.Value != null && typeof(IQueryable).IsAssignableFrom(constant.Type))
				{
					return ((IQueryable)constant.Value).ElementType;
				}

				return GetOrigionalType(methodCall.Arguments[0]);
			}

			if (Type.IsGenericType && typeof(EntrySet<>).IsAssignableFrom(Type.GetGenericTypeDefinition()))
			{
				return Type.GetGenericArguments()[0];
			}

			return Type;
		}

		public void AddWhere(WhereExpression where)
		{
			_whereClause.Add(where);
		}

		public void SetSelect(SelectExpression select)
		{
			_select = select;
		}

		public void SetSkip(SkipTakeExpression skip)
		{
			_skip = skip;
		}

		public void SetTake(SkipTakeExpression take)
		{
			_take = take;
		}

		public void SetOrderBy(OrderByExpression orderBy)
		{
			_orderBy = orderBy;
		}

		public override string ToString()
		{
			return string.Format(".Directory<{0}> {1}", Type.ToString(), RootExpression.ToString());
		}

		#endregion
	}
}
