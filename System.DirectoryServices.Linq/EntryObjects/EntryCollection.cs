using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Linq.Attributes;
using System.DirectoryServices.Linq.Filters;
using System.Reflection;

namespace System.DirectoryServices.Linq.EntryObjects
{
    public class EntryCollection<TEntry> : IEnumerable<TEntry> where TEntry : class
    {
        #region Fields

        private bool _isLoaded;
        private readonly EntryObject _entryObject;
        private PropertyInfo _property;
        private List<TEntry> _items = new List<TEntry>();

        #endregion

        #region Constructors

        public EntryCollection(EntryObject entryObject, PropertyInfo property)
        {
            _entryObject = entryObject;
            _property = property;
        }

        #endregion

        #region Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TEntry> GetEnumerator()
        {
            if (!_isLoaded)
            {
                LoadCollection();
            }

            return _items.GetEnumerator();
        }

        private void LoadCollection()
        {
            //_entryObject.Entry.RefreshCache(new string[] { "tokenGroups" });

            //for (int i = 0; i < _entryObject.Entry.Properties["tokenGroups"].Count; i++)
            //{
            //    SecurityIdentifier sid = new SecurityIdentifier((byte[])_entryObject.Entry.Properties["tokenGroups"][i], 0);
            //    NTAccount nt = (NTAccount)sid.Translate(typeof(NTAccount));
            //    System.Diagnostics.Debug.WriteLine(nt.Value);
            //}

            //var user = (ActiveDs.IADsUser)_entryObject.Entry.NativeObject;
            //var groups = user.Groups();

            //foreach (var group in (IEnumerable)groups)
            //{
            //    DirectoryEntry obGpEntry = new DirectoryEntry(group);

            //    System.Diagnostics.Debug.WriteLine("");

            //    foreach (PropertyValueCollection item in obGpEntry.Properties)
            //    {
            //        System.Diagnostics.Debug.WriteLine(item.Value);
            //    }

            //    Marshal.ReleaseComObject(group);
            //}

            //Marshal.ReleaseComObject(groups);
            //Marshal.ReleaseComObject(user);

            var filter = CreateFilter(typeof(TEntry));
            var queryExecutor = _entryObject.Context.QueryExecutor;

            using (var enumerator = queryExecutor.ExecuteQuery<TEntry>(filter))
            {
                while (enumerator.MoveNext())
                {
                    _items.Add(enumerator.Current);
                }
            }

            _isLoaded = true;
        }

        private string CreateFilter(Type entryType)
        {
            var builder = new FilterBuilder(entryType);
            var attributeBuilder = builder.CreateBuilder();
            var attribute = entryType.AssertGetAttribute<DirectoryTypeAttribute>();
            attributeBuilder.AddObjectClass(attribute.Name);

            // Example AttributeName: "member:1.2.840.113556.1.4.1941:"
            var propertyAttribute = _property.AssertGetAttribute<EntryCollectionPropertyAttribute>();
            var attributeName = string.Concat(propertyAttribute.Name, propertyAttribute.MatchingRuleValue);
			attributeBuilder.AddAttribute(attributeName, FilterOperator.Equals, _entryObject.InternalDn);
            builder.AddBuilder(attributeBuilder);

            return builder.ToString();
        }

        #endregion
    }
}
