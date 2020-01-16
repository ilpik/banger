using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal class GenericListAdaptor<T> : IReorderableListAdaptor
	{
		private IList<T> _list;

		private ReorderableListControl.ItemDrawer<T> _itemDrawer;

		public float FixedItemHeight;

		public IList<T> List => _list;

		public T this[int index] => _list[index];

		public int Count => _list.Count;

		public GenericListAdaptor(IList<T> list, ReorderableListControl.ItemDrawer<T> itemDrawer, float itemHeight)
		{
			_list = list;
			_itemDrawer = (itemDrawer ?? new ReorderableListControl.ItemDrawer<T>(ReorderableListGUI.DefaultItemDrawer));
			FixedItemHeight = itemHeight;
		}

		public virtual bool CanDrag(int index)
		{
			return true;
		}

		public virtual bool CanRemove(int index)
		{
			return true;
		}

		public virtual void Add()
		{
			_list.Add(default(T));
		}

		public virtual void Insert(int index)
		{
			_list.Insert(index, default(T));
		}

		public virtual void Duplicate(int index)
		{
			T val = _list[index];
			ICloneable cloneable = val as ICloneable;
			if (cloneable != null)
			{
				val = (T)cloneable.Clone();
			}
			_list.Insert(index + 1, val);
		}

		public virtual void Remove(int index)
		{
			_list.RemoveAt(index);
		}

		public virtual void Move(int sourceIndex, int destIndex)
		{
			if (destIndex > sourceIndex)
			{
				destIndex--;
			}
			T item = _list[sourceIndex];
			_list.RemoveAt(sourceIndex);
			_list.Insert(destIndex, item);
		}

		public virtual void Clear()
		{
			_list.Clear();
		}

		public virtual void BeginGUI()
		{
		}

		public virtual void EndGUI()
		{
		}

		public virtual void DrawItemBackground(Rect position, int index)
		{
		}

		public virtual void DrawItem(Rect position, int index)
		{
			_list[index] = _itemDrawer(position, _list[index]);
		}

		public virtual float GetItemHeight(int index)
		{
			return FixedItemHeight;
		}
	}
}
