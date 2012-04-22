
namespace System.DirectoryServices.Linq
{
	public class UserEntryObject : EntryObject
	{
		public bool SetPassword(string password)
		{
			return Entry.Invoke("SetPassword", new[] { password }) == null;
		}
	}
}
