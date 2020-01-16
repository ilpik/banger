using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	[Flags]
	internal enum ReorderableListFlags
	{
		DisableReordering = 0x1,
		HideAddButton = 0x2,
		HideRemoveButtons = 0x4,
		DisableContextMenu = 0x8,
		DisableDuplicateCommand = 0x10,
		DisableAutoFocus = 0x20,
		ShowIndices = 0x40,
		[Obsolete("This flag is redundant because the clipping optimization was removed.")]
		DisableClipping = 0x80,
		DisableAutoScroll = 0x100,
		ShowSizeField = 0x200
	}
}
