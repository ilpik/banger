using System;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal sealed class AddMenuClickedEventArgs : EventArgs
	{
		public IReorderableListAdaptor Adaptor
		{
			get;
			private set;
		}

		public Rect ButtonPosition
		{
			get;
			internal set;
		}

		public AddMenuClickedEventArgs(IReorderableListAdaptor adaptor, Rect buttonPosition)
		{
			Adaptor = adaptor;
			ButtonPosition = buttonPosition;
		}
	}
}
