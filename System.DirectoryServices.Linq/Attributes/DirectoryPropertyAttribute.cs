namespace System.DirectoryServices.Linq.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DirectoryPropertyAttribute : Attribute
	{
		public DirectoryPropertyAttribute(string name)
		{
			Name = name;
			Scope = SearchScope.Subtree;
		}

		public SearchScope Scope { get; set; }

		public string Name { get; private set; }

		public bool IsReference { get; set; }

		public bool IsReferenceCollection { get; set; }
	}
}