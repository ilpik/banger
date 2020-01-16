using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal sealed class ItemMovedEventArgs : EventArgs
	{
		public IReorderableListAdaptor Adaptor
		{
			get;
			private set;
		}

		public int OldItemIndex
		{
			get;
			internal set;
		}

		public int NewItemIndex
		{
			get;
			internal set;
		}

		public ItemMovedEventArgs(IReorderableListAdaptor adaptor, int oldItemIndex, int newItemIndex)
		{
			Adaptor = adaptor;
			OldItemIndex = oldItemIndex;
			NewItemIndex = newItemIndex;
		}
	}
}
