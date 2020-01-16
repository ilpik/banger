using System.ComponentModel;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal sealed class ItemRemovingEventArgs : CancelEventArgs
	{
		public IReorderableListAdaptor Adaptor
		{
			get;
			private set;
		}

		public int ItemIndex
		{
			get;
			internal set;
		}

		public ItemRemovingEventArgs(IReorderableListAdaptor adaptor, int itemIndex)
		{
			Adaptor = adaptor;
			ItemIndex = itemIndex;
		}
	}
}
