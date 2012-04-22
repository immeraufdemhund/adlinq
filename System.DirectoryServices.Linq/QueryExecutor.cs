﻿using System.Collections.Generic;
using System.DirectoryServices.Linq.Expressions;
using System.Reflection;
using System.DirectoryServices.Linq.Attributes;

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

		private static string[] GetPropertiesFromType(Type type)
		{
			var list = new List<string>();

			foreach (var property in type.GetProperties())
			{
				var attributeName = GetAttributeName(property);

				if (!string.IsNullOrEmpty(attributeName))
				{
					list.Add(attributeName);
				}
			}

			return list.ToArray();
		}

		private static string GetAttributeName(MemberInfo info)
		{
			var attribute = info.GetAttribute<DirectoryPropertyAttribute>();

			if (attribute != null && !string.IsNullOrEmpty(attribute.Name) && !attribute.IsReference && !attribute.IsReferenceCollection)
			{
				return attribute.Name;
			}

			return null;
		}
	}
}
