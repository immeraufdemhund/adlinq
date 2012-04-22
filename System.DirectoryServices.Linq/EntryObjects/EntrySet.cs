using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.DirectoryServices.Linq.EntryObjects
{
	public abstract class EntrySet : IEntrySet
	{
		internal EntrySet(DirectoryContext context)
		{
			Context = context;
		}

		protected DirectoryContext Context { get; private set; }

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
				return GetProvider();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumeratorCore();
		}

		protected virtual IQueryProvider GetProvider()
		{
			return Context.QueryProvider;
		}

		protected abstract Type GetElementType();

		protected abstract Expression GetExpression();

		protected abstract IEnumerator GetEnumeratorCore();
	}

	public class EntrySet<T> : EntrySet, IEntrySet<T>
	{
		public EntrySet(DirectoryContext context) : base(context)
		{
		}

		protected override Type GetElementType()
		{
			return typeof(T);
		}

		protected override Expression GetExpression()
		{
			return Expression.Constant(this);
		}

		protected override IEnumerator GetEnumeratorCore()
		{
			return GetEnumerator();
		}

		protected virtual EntryQuery<T> CreateQuery()
		{
			return (EntryQuery<T>)GetProvider().CreateQuery<T>(GetExpression());
		}

		EntryQuery<T> IEntrySet<T>.CreateQuery()
		{
			return CreateQuery();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return CreateQuery().GetEnumerator();
		}
	}
}
