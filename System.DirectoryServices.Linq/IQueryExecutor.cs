using System.Collections.Generic;
using System.DirectoryServices.Linq.Expressions;

namespace System.DirectoryServices.Linq
{
	public interface IQueryExecutor
	{
		T Execute<T>(string query);
		T Execute<T>(SingleResultExpression expression);

		object Execute(string query, Type elementType);
		object Execute(SingleResultExpression expression);

		//IEnumerable<T> ExecuteCommand<T>(string query);

		IEnumerator<T> ExecuteQuery<T>(string query);
		IEnumerator<T> ExecuteQuery<T>(DirectoryExpression expression);
	}
}
