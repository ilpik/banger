using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal sealed class ItemInsertedEventArgs : EventArgs
	{
		public IReorderableListAdaptor Adaptor
		{
			get;
			private set;
		}

		public int ItemIndex
		{
			get;
			private set;
		}

		public bool WasDuplicated
		{
			get;
			private set;
		}

		public ItemInsertedEventArgs(IReorderableListAdaptor adaptor, int itemIndex, bool wasDuplicated)
		{
			Adaptor = adaptor;
			ItemIndex = itemIndex;
			WasDuplicated = wasDuplicated;
		}
	}
}
