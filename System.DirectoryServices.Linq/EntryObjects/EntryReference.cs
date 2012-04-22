using System.Reflection;

namespace System.DirectoryServices.Linq.EntryObjects
{
	public class EntryReference<TEntry> where TEntry : EntryObject
	{
		private EntryObject _entryObject;
		private PropertyInfo _property;

		public EntryReference(EntryObject entryObject, PropertyInfo property)
		{
			_entryObject = entryObject;
			_property = property;
		}

		public TEntry Value { get; set; }
	}
}
