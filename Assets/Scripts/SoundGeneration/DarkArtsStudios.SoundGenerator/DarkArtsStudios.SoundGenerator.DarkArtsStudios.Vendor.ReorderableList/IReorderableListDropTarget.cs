namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	public interface IReorderableListDropTarget
	{
		bool CanDropInsert(int insertionIndex);

		void ProcessDropInsertion(int insertionIndex);
	}
}
