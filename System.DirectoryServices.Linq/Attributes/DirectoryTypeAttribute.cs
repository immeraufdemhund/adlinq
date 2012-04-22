namespace System.DirectoryServices.Linq.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class DirectoryTypeAttribute : Attribute
	{
		public DirectoryTypeAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}