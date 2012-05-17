namespace System.DirectoryServices.Linq.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class DirectoryTypeAttribute : Attribute
	{
		private readonly string _schemaName;

		public DirectoryTypeAttribute(string name)
		{
			Name = name;
		}

		public DirectoryTypeAttribute(string name, string schema)
		{
			Name = name;
			_schemaName = schema;
		}

		public string Name { get; private set; }

		public string SchemaName
		{
			get
			{
				if (!string.IsNullOrEmpty(_schemaName))
				{
					return _schemaName;
				}

				return Name;
			}
		}
	}
}