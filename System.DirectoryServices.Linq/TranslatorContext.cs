using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Linq.Expressions;
using System.Linq;

namespace System.DirectoryServices.Linq
{
	public class TranslatorContext
	{
		#region Constructors

		public TranslatorContext(DirectorySearcher directorySearcher)
		{
			//ElementType = elementType;
			DirectorySearcher = directorySearcher;
		}

		public TranslatorContext(DirectoryExpression expression, DirectorySearcher directorySearcher)
		{
			Expression = expression;
			DirectorySearcher = directorySearcher;
			//ElementType = expression.GetOrigionalType();
		}

		#endregion

		#region Properties

		//public Type ElementType { get; private set; }

		public DirectoryExpression Expression { get; private set; }

		public DirectorySearcher DirectorySearcher { get; private set; }

		#endregion

		#region Methods

		public SearchResult FindOne()
		{
			SearchResults results;

			try
			{
				results = GetAllSearchResults();
			}
			catch (Exception ex)
			{
				// TODO: Find reason of failure and rethrow valid exception.
				//throw ex;
				return null;
			}

			return GetSingleSearchResult(results);
		}

		public IEnumerable<SearchResult> FindAll()
		{
			try
			{
				return GetAllSearchResults();
			}
			catch (Exception)
			{
				// TODO Throw origional exception, but verify why it threw.
				return Enumerable.Empty<SearchResult>();
			}
		}

		private SearchResult GetSingleSearchResult(SearchResults results)
		{
			if (Expression != null && Expression.NodeType.Is(DirectoryExpressionType.SingleResult))
			{
				var single = (SingleResultExpression)Expression;

				if (results.Count > 1 && single.ThrowIfNotSingle)
				{
					throw new MoreThanOneResultException();
				}

				if (results.Count == 0 && single.ThrowIfNotFound)
				{
					throw new ResultNotFoundException();
				}

				var resultType = single.SingleResultType;

				if (resultType == SingleResultType.Last || resultType == SingleResultType.LastOrDefault)
				{
					return results.LastOrDefault();
				}

				return results.FirstOrDefault();
			}

			return results.FirstOrDefault();
		}

		private SearchResults GetAllSearchResults()
		{
			return new SearchResults(Expression, DirectorySearcher.FindAll());
		}

		#endregion
	}
}
