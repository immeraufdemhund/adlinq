namespace System.DirectoryServices.Linq.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DirectoryPropertyAttribute : Attribute
	{
		public DirectoryPropertyAttribute(string name)
			: this(name, false)
		{
		}

		public DirectoryPropertyAttribute(string name, bool readOnly)
		{
			Name = name;
			IsReadOnly = readOnly;
			//Scope = SearchScope.Subtree;
		}

		//public bool IsReference { get; set; }

		public SearchScope Scope { get; set; }

		public string Name { get; private set; }

		public bool IsReadOnly { get; private set; }

		//public bool IsReferenceCollection { get; set; }
	}
}