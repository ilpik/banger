using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	public interface IElementAdderMenu
	{
		bool IsEmpty
		{
			get;
		}

		void DropDown(Rect position);
	}
}
