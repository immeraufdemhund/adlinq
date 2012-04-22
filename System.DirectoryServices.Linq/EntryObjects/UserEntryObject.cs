using System.Runtime.InteropServices;

namespace System.DirectoryServices.Linq.EntryObjects
{
	public class UserEntryObject : EntryObject
	{
		/// <summary>
		/// Sets the password for the current account.
		/// </summary>
		/// <param name="password">The value to set as the password.</param>
		/// <returns>true if the password was set, false otherwise.</returns>
		public bool SetPassword(string password)
		{ 
			if (!string.IsNullOrEmpty(password))
			{
				var setPasswordResult = Entry.Invoke("SetPassword", new[]
				{
					password
				});

				var result = setPasswordResult == null;

				if (!result)
				{
					Marshal.ReleaseComObject(setPasswordResult);
				}

				return result;
			}

			return false;
		}
	}
}
