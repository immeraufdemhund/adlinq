using System.Linq;

namespace System.DirectoryServices.Linq.EntryObjects
{
	public interface IEntrySet : IQueryable, IOrderedQueryable
	{
	}

	public interface IEntrySet<T> : IEntrySet, IQueryable<T>, IOrderedQueryable<T>
	{
		EntryQuery<T> CreateQuery();
	}
}
