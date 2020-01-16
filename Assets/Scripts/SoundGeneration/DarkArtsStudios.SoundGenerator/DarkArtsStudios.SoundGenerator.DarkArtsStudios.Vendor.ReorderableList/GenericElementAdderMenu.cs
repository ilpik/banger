using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal sealed class GenericElementAdderMenu : IElementAdderMenu
	{
		private GenericMenu _innerMenu = new GenericMenu();

		public bool IsEmpty => _innerMenu.GetItemCount() == 0;

		public void AddItem(GUIContent content, GenericMenu.MenuFunction handler)
		{
			_innerMenu.AddItem(content, on: false, handler);
		}

		public void AddDisabledItem(GUIContent content)
		{
			_innerMenu.AddDisabledItem(content);
		}

		public void AddSeparator(string path = "")
		{
			_innerMenu.AddSeparator(path);
		}

		public void DropDown(Rect position)
		{
			_innerMenu.DropDown(position);
		}
	}
}
