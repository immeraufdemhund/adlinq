using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.DirectoryServices.Linq.EntryObjects
{
	public abstract class EntryQuery : IQueryable, IOrderedQueryable
	{
		private readonly EntryQueryState _queryState;

		internal protected EntryQuery(EntryQueryState queryState)
		{
			_queryState = queryState;
		}

		public EntryQueryState QueryState
		{
			get
			{
				return _queryState;
			}
		}

		protected DirectoryContext Context
		{
			get
			{
				return _queryState.Context;
			}
		}

		Type IQueryable.ElementType
		{
			get
			{
				return GetElementType();
			}
		}

		Expression IQueryable.Expression
		{
			get
			{
				return GetExpression();
			}
		}

		IQueryProvider IQueryable.Provider
		{
			get
			{
				return Context.QueryProvider;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumeratorCore();
		}

		protected virtual Expression GetExpression()
		{
			return QueryState.Expression;
		}

		protected abstract Type GetElementType();

		protected abstract IEnumerator GetEnumeratorCore();
	}

	public class EntryQuery<T> : EntryQuery, IQueryable<T>, IOrderedQueryable<T>
	{
		public EntryQuery(EntryQueryState queryState) : base(queryState)
		{
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumeratorCore();
		}

		protected override Type GetElementType()
		{
			return typeof(T);
		}

		protected override IEnumerator GetEnumeratorCore()
		{
			return GetEnumerator();
		}

		internal IEnumerator<T> GetResultsEnumerator()
		{
			var result = QueryState.GetExpression();
			return Context.QueryExecutor.ExecuteQuery<T>(result);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return GetResultsEnumerator();
		}
	}
}
