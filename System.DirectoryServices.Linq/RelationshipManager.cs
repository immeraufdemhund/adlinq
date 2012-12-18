using System.Collections.Generic;
using System.DirectoryServices.Linq.EntryObjects;

namespace System.DirectoryServices.Linq
{
    public class RelationshipManager : IRelationshipManager
    {
        private readonly Type _parentType;
        private readonly IDictionary<string, object> _relationships = new Dictionary<string, object>();

        public RelationshipManager(EntryObject entryParent)
        {
            EntryObject = entryParent;
            _parentType = entryParent.GetType();
        }

        public EntryObject EntryObject { get; private set; }

        public EntryReference<TEntry> GetEntryReference<TEntry>(string propertyName) where TEntry : EntryObject
        {
            var property = _parentType.GetProperty(propertyName);
            return new EntryReference<TEntry>(EntryObject, property);
        }

        public EntryCollection<TEntry> GetEntryCollection<TEntry>(string propertyName) where TEntry : EntryObject
        {
            if (!_relationships.ContainsKey(propertyName))
            {
                var property = _parentType.GetProperty(propertyName);
                var result = new EntryCollection<TEntry>(EntryObject, property);
                _relationships.Add(propertyName, result);

                return result;
            }

            return (EntryCollection<TEntry>)_relationships[propertyName];
        }
    }
}
