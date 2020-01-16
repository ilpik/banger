using System.ComponentModel;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal sealed class ItemMovingEventArgs : CancelEventArgs
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

		public int DestinationItemIndex
		{
			get;
			internal set;
		}

		public int NewItemIndex
		{
			get
			{
				int num = DestinationItemIndex;
				if (num > ItemIndex)
				{
					num--;
				}
				return num;
			}
		}

		public ItemMovingEventArgs(IReorderableListAdaptor adaptor, int itemIndex, int destinationItemIndex)
		{
			Adaptor = adaptor;
			ItemIndex = itemIndex;
			DestinationItemIndex = destinationItemIndex;
		}
	}
}
