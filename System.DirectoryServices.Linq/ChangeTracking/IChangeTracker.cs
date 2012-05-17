using System.DirectoryServices.Linq.EntryObjects;

namespace System.DirectoryServices.Linq.ChangeTracking
{
	public interface IChangeTracker
	{
		void SubmitChanges();
        void TrackChanges<T>(T entryObject) where T : EntryObject;
		void AddObject<T>(T entryObject) where T : EntryObject;
		void DeleteObject<T>(T entryObject) where T : EntryObject;
	}
}
