using System.Collections.Generic;
using System.DirectoryServices.Linq.Attributes;
using System.DirectoryServices.Linq.Expressions;
using System.Reflection;

namespace System.DirectoryServices.Linq
{
	public class QueryExecutor : IQueryExecutor
	{
		public QueryExecutor(DirectoryContext context)
		{
			Context = context;
		}

		public DirectoryContext Context { get; private set; }

		public IQueryTranslator Translator
		{
			get
			{
				return Context.Translator;
			}
		}

		public T Execute<T>(string query)
		{
			using (var searcher = CreateDirectorySearcher(query, typeof(T)))
			{
				return Translator.TranslateOne<T>(searcher);
			}
		}

		public T Execute<T>(SingleResultExpression expression)
		{
			using (var searcher = CreateDirectorySearcher(expression))
			{
				return Translator.TranslateOne<T>(expression, searcher);
			}
		}

		public object Execute(string query, Type elementType)
		{
			using (var searcher = CreateDirectorySearcher(query, elementType))
			{
				return Translator.TranslateOne(elementType, searcher);
			}
		}

		public object Execute(SingleResultExpression expression)
		{
			using (var searcher = CreateDirectorySearcher(expression))
			{
				return Translator.TranslateOne(expression, searcher);
			}
		}

		//public IEnumerable<T> ExecuteCommand<T>(string query)
		//{

		//}

		public IEnumerator<T> ExecuteQuery<T>(string query)
		{
			using (var searcher = CreateDirectorySearcher(query, typeof(T)))
			{
				return Translator.Translate<T>(searcher);
			}
		}

		public IEnumerator<T> ExecuteQuery<T>(DirectoryExpression expression)
		{
			using (var searcher = CreateDirectorySearcher(expression))
			{
				return Translator.Translate<T>(expression, searcher);
			}
		}

		private DirectorySearcher CreateDirectorySearcher(DirectoryExpression expression)
		{
			var origionalType = expression.GetOrigionalType();
			return CreateDirectorySearcher(null, origionalType);
		}

		private DirectorySearcher CreateDirectorySearcher(string filter, Type elementType)
		{
			var properties = GetPropertiesFromType(elementType);
			return new DirectorySearcher(Context.DomainEntry, filter, properties)
			{
				PageSize = 1000,
				SearchScope = SearchScope.Subtree,
			};
		}

		private string[] GetPropertiesFromType(Type type)
		{
			var list = new List<string>();

			foreach (var property in type.GetProperties())
			{
				var attributeName = GetAttributeName<DirectoryPropertyAttribute>(property);

				if (!string.IsNullOrEmpty(attributeName))
				{
					list.Add(attributeName);
				}
			}

			return list.ToArray();
		}

		public string GetAttributeName<TAttribute>(MemberInfo info) where TAttribute : DirectoryAttribute
		{
			var attribute = info.GetAttribute<TAttribute>();

			if (attribute != null && !string.IsNullOrEmpty(attribute.Name))
			{
				return attribute.Name;
			}

			return null;
		}
	}
}
