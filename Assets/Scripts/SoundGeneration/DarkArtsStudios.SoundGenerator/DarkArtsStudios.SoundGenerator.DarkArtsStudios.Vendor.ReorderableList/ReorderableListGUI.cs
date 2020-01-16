using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal static class ReorderableListGUI
	{
		public const float DefaultItemHeight = 18f;

		private static GUIContent s_Temp;

		public static int IndexOfChangedItem
		{
			get;
			internal set;
		}

		public static int CurrentListControlID => ReorderableListControl.CurrentListControlID;

		public static Rect CurrentListPosition => ReorderableListControl.CurrentListPosition;

		public static int CurrentItemIndex => ReorderableListControl.CurrentItemIndex;

		public static Rect CurrentItemTotalPosition => ReorderableListControl.CurrentItemTotalPosition;

		private static ReorderableListControl DefaultListControl
		{
			get;
			set;
		}

		public static T DefaultItemDrawer<T>(Rect position, T item)
		{
			GUI.Label(position, "Item drawer not implemented.");
			return item;
		}

		public static string TextFieldItemDrawer(Rect position, string item)
		{
			if (item == null)
			{
				item = "";
				GUI.changed = true;
			}
			return EditorGUI.TextField(position, item);
		}

		static ReorderableListGUI()
		{
			s_Temp = new GUIContent();
			DefaultListControl = new ReorderableListControl();
			DefaultListControl.ContainerStyle = new GUIStyle(ReorderableListStyles.Container);
			DefaultListControl.FooterButtonStyle = new GUIStyle(ReorderableListStyles.FooterButton);
			DefaultListControl.ItemButtonStyle = new GUIStyle(ReorderableListStyles.ItemButton);
			IndexOfChangedItem = -1;
		}

		public static void Title(GUIContent title)
		{
			Title(GUILayoutUtility.GetRect(title, ReorderableListStyles.Title), title);
			GUILayout.Space(-1f);
		}

		public static void Title(string title)
		{
			s_Temp.text = title;
			Title(s_Temp);
		}

		public static void Title(Rect position, GUIContent title)
		{
			if (Event.current.type == EventType.Repaint)
			{
				ReorderableListStyles.Title.Draw(position, title, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
			}
		}

		public static void Title(Rect position, string text)
		{
			s_Temp.text = text;
			Title(position, s_Temp);
		}

		private static void DoListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmpty drawEmpty, float itemHeight, ReorderableListFlags flags)
		{
			ReorderableListControl.DrawControlFromState(new GenericListAdaptor<T>(list, drawItem, itemHeight), drawEmpty, flags);
		}

		private static void DoListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmptyAbsolute drawEmpty, float itemHeight, ReorderableListFlags flags)
		{
			GenericListAdaptor<T> adaptor = new GenericListAdaptor<T>(list, drawItem, itemHeight);
			ReorderableListControl.DrawControlFromState(position, adaptor, drawEmpty, flags);
		}

		public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmpty drawEmpty, float itemHeight, ReorderableListFlags flags)
		{
			DoListField(list, drawItem, drawEmpty, itemHeight, flags);
		}

		public static void ListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmptyAbsolute drawEmpty, float itemHeight, ReorderableListFlags flags)
		{
			DoListFieldAbsolute(position, list, drawItem, drawEmpty, itemHeight, flags);
		}

		public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmpty drawEmpty, float itemHeight)
		{
			DoListField(list, drawItem, drawEmpty, itemHeight, (ReorderableListFlags)0);
		}

		public static void ListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmptyAbsolute drawEmpty, float itemHeight)
		{
			DoListFieldAbsolute(position, list, drawItem, drawEmpty, itemHeight, (ReorderableListFlags)0);
		}

		public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmpty drawEmpty, ReorderableListFlags flags)
		{
			DoListField(list, drawItem, drawEmpty, 18f, flags);
		}

		public static void ListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmptyAbsolute drawEmpty, ReorderableListFlags flags)
		{
			DoListFieldAbsolute(position, list, drawItem, drawEmpty, 18f, flags);
		}

		public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmpty drawEmpty)
		{
			DoListField(list, drawItem, drawEmpty, 18f, (ReorderableListFlags)0);
		}

		public static void ListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmptyAbsolute drawEmpty)
		{
			DoListFieldAbsolute(position, list, drawItem, drawEmpty, 18f, (ReorderableListFlags)0);
		}

		public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, float itemHeight, ReorderableListFlags flags)
		{
			DoListField(list, drawItem, null, itemHeight, flags);
		}

		public static void ListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, float itemHeight, ReorderableListFlags flags)
		{
			DoListFieldAbsolute(position, list, drawItem, null, itemHeight, flags);
		}

		public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, float itemHeight)
		{
			DoListField(list, drawItem, null, itemHeight, (ReorderableListFlags)0);
		}

		public static void ListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, float itemHeight)
		{
			DoListFieldAbsolute(position, list, drawItem, null, itemHeight, (ReorderableListFlags)0);
		}

		public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListFlags flags)
		{
			DoListField(list, drawItem, null, 18f, flags);
		}

		public static void ListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListFlags flags)
		{
			DoListFieldAbsolute(position, list, drawItem, null, 18f, flags);
		}

		public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem)
		{
			DoListField(list, drawItem, null, 18f, (ReorderableListFlags)0);
		}

		public static void ListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem)
		{
			DoListFieldAbsolute(position, list, drawItem, null, 18f, (ReorderableListFlags)0);
		}

		public static float CalculateListFieldHeight(int itemCount, float itemHeight, ReorderableListFlags flags)
		{
			ReorderableListFlags flags2 = DefaultListControl.Flags;
			try
			{
				DefaultListControl.Flags = flags;
				return DefaultListControl.CalculateListHeight(itemCount, itemHeight);
			}
			finally
			{
				DefaultListControl.Flags = flags2;
			}
		}

		public static float CalculateListFieldHeight(int itemCount, ReorderableListFlags flags)
		{
			return CalculateListFieldHeight(itemCount, 18f, flags);
		}

		public static float CalculateListFieldHeight(int itemCount, float itemHeight)
		{
			return CalculateListFieldHeight(itemCount, itemHeight, (ReorderableListFlags)0);
		}

		public static float CalculateListFieldHeight(int itemCount)
		{
			return CalculateListFieldHeight(itemCount, 18f, (ReorderableListFlags)0);
		}

		private static void DoListField(SerializedProperty arrayProperty, float fixedItemHeight, ReorderableListControl.DrawEmpty drawEmpty, ReorderableListFlags flags)
		{
			ReorderableListControl.DrawControlFromState(new SerializedPropertyAdaptor(arrayProperty, fixedItemHeight), drawEmpty, flags);
		}

		private static void DoListFieldAbsolute(Rect position, SerializedProperty arrayProperty, float fixedItemHeight, ReorderableListControl.DrawEmptyAbsolute drawEmpty, ReorderableListFlags flags)
		{
			SerializedPropertyAdaptor adaptor = new SerializedPropertyAdaptor(arrayProperty, fixedItemHeight);
			ReorderableListControl.DrawControlFromState(position, adaptor, drawEmpty, flags);
		}

		public static void ListField(SerializedProperty arrayProperty, ReorderableListControl.DrawEmpty drawEmpty, ReorderableListFlags flags)
		{
			DoListField(arrayProperty, 0f, drawEmpty, flags);
		}

		public static void ListFieldAbsolute(Rect position, SerializedProperty arrayProperty, ReorderableListControl.DrawEmptyAbsolute drawEmpty, ReorderableListFlags flags)
		{
			DoListFieldAbsolute(position, arrayProperty, 0f, drawEmpty, flags);
		}

		public static void ListField(SerializedProperty arrayProperty, ReorderableListControl.DrawEmpty drawEmpty)
		{
			DoListField(arrayProperty, 0f, drawEmpty, (ReorderableListFlags)0);
		}

		public static void ListFieldAbsolute(Rect position, SerializedProperty arrayProperty, ReorderableListControl.DrawEmptyAbsolute drawEmpty)
		{
			DoListFieldAbsolute(position, arrayProperty, 0f, drawEmpty, (ReorderableListFlags)0);
		}

		public static void ListField(SerializedProperty arrayProperty, ReorderableListFlags flags)
		{
			DoListField(arrayProperty, 0f, null, flags);
		}

		public static void ListFieldAbsolute(Rect position, SerializedProperty arrayProperty, ReorderableListFlags flags)
		{
			DoListFieldAbsolute(position, arrayProperty, 0f, null, flags);
		}

		public static void ListField(SerializedProperty arrayProperty)
		{
			DoListField(arrayProperty, 0f, null, (ReorderableListFlags)0);
		}

		public static void ListFieldAbsolute(Rect position, SerializedProperty arrayProperty)
		{
			DoListFieldAbsolute(position, arrayProperty, 0f, null, (ReorderableListFlags)0);
		}

		public static float CalculateListFieldHeight(SerializedProperty arrayProperty, ReorderableListFlags flags)
		{
			ReorderableListFlags flags2 = DefaultListControl.Flags;
			try
			{
				DefaultListControl.Flags = flags;
				return DefaultListControl.CalculateListHeight(new SerializedPropertyAdaptor(arrayProperty));
			}
			finally
			{
				DefaultListControl.Flags = flags2;
			}
		}

		public static float CalculateListFieldHeight(SerializedProperty arrayProperty)
		{
			return CalculateListFieldHeight(arrayProperty, (ReorderableListFlags)0);
		}

		public static void ListField(SerializedProperty arrayProperty, float fixedItemHeight, ReorderableListControl.DrawEmpty drawEmpty, ReorderableListFlags flags)
		{
			DoListField(arrayProperty, fixedItemHeight, drawEmpty, flags);
		}

		public static void ListFieldAbsolute(Rect position, SerializedProperty arrayProperty, float fixedItemHeight, ReorderableListControl.DrawEmptyAbsolute drawEmpty, ReorderableListFlags flags)
		{
			DoListFieldAbsolute(position, arrayProperty, fixedItemHeight, drawEmpty, flags);
		}

		public static void ListField(SerializedProperty arrayProperty, float fixedItemHeight, ReorderableListControl.DrawEmpty drawEmpty)
		{
			DoListField(arrayProperty, fixedItemHeight, drawEmpty, (ReorderableListFlags)0);
		}

		public static void ListFieldAbsolute(Rect position, SerializedProperty arrayProperty, float fixedItemHeight, ReorderableListControl.DrawEmptyAbsolute drawEmpty)
		{
			DoListFieldAbsolute(position, arrayProperty, fixedItemHeight, drawEmpty, (ReorderableListFlags)0);
		}

		public static void ListField(SerializedProperty arrayProperty, float fixedItemHeight, ReorderableListFlags flags)
		{
			DoListField(arrayProperty, fixedItemHeight, null, flags);
		}

		public static void ListFieldAbsolute(Rect position, SerializedProperty arrayProperty, float fixedItemHeight, ReorderableListFlags flags)
		{
			DoListFieldAbsolute(position, arrayProperty, fixedItemHeight, null, flags);
		}

		public static void ListField(SerializedProperty arrayProperty, float fixedItemHeight)
		{
			DoListField(arrayProperty, fixedItemHeight, null, (ReorderableListFlags)0);
		}

		public static void ListFieldAbsolute(Rect position, SerializedProperty arrayProperty, float fixedItemHeight)
		{
			DoListFieldAbsolute(position, arrayProperty, fixedItemHeight, null, (ReorderableListFlags)0);
		}

		private static void DoListField(IReorderableListAdaptor adaptor, ReorderableListControl.DrawEmpty drawEmpty, ReorderableListFlags flags = (ReorderableListFlags)0)
		{
			ReorderableListControl.DrawControlFromState(adaptor, drawEmpty, flags);
		}

		private static void DoListFieldAbsolute(Rect position, IReorderableListAdaptor adaptor, ReorderableListControl.DrawEmptyAbsolute drawEmpty, ReorderableListFlags flags = (ReorderableListFlags)0)
		{
			ReorderableListControl.DrawControlFromState(position, adaptor, drawEmpty, flags);
		}

		public static void ListField(IReorderableListAdaptor adaptor, ReorderableListControl.DrawEmpty drawEmpty, ReorderableListFlags flags)
		{
			DoListField(adaptor, drawEmpty, flags);
		}

		public static void ListFieldAbsolute(Rect position, IReorderableListAdaptor adaptor, ReorderableListControl.DrawEmptyAbsolute drawEmpty, ReorderableListFlags flags)
		{
			DoListFieldAbsolute(position, adaptor, drawEmpty, flags);
		}

		public static void ListField(IReorderableListAdaptor adaptor, ReorderableListControl.DrawEmpty drawEmpty)
		{
			DoListField(adaptor, drawEmpty);
		}

		public static void ListFieldAbsolute(Rect position, IReorderableListAdaptor adaptor, ReorderableListControl.DrawEmptyAbsolute drawEmpty)
		{
			DoListFieldAbsolute(position, adaptor, drawEmpty);
		}

		public static void ListField(IReorderableListAdaptor adaptor, ReorderableListFlags flags)
		{
			DoListField(adaptor, null, flags);
		}

		public static void ListFieldAbsolute(Rect position, IReorderableListAdaptor adaptor, ReorderableListFlags flags)
		{
			DoListFieldAbsolute(position, adaptor, null, flags);
		}

		public static void ListField(IReorderableListAdaptor adaptor)
		{
			DoListField(adaptor, null);
		}

		public static void ListFieldAbsolute(Rect position, IReorderableListAdaptor adaptor)
		{
			DoListFieldAbsolute(position, adaptor, null);
		}

		public static float CalculateListFieldHeight(IReorderableListAdaptor adaptor, ReorderableListFlags flags)
		{
			ReorderableListFlags flags2 = DefaultListControl.Flags;
			try
			{
				DefaultListControl.Flags = flags;
				return DefaultListControl.CalculateListHeight(adaptor);
			}
			finally
			{
				DefaultListControl.Flags = flags2;
			}
		}

		public static float CalculateListFieldHeight(IReorderableListAdaptor adaptor)
		{
			return CalculateListFieldHeight(adaptor, (ReorderableListFlags)0);
		}
	}
}
