using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList.Internal;
using System;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal class SerializedPropertyAdaptor : IReorderableListAdaptor
	{
		private SerializedProperty _arrayProperty;

		public float FixedItemHeight;

		public SerializedProperty this[int index] => _arrayProperty.GetArrayElementAtIndex(index);

		public SerializedProperty arrayProperty => _arrayProperty;

		public int Count => _arrayProperty.arraySize;

		public SerializedPropertyAdaptor(SerializedProperty arrayProperty, float fixedItemHeight)
		{
			if (arrayProperty == null)
			{
				throw new ArgumentNullException("Array property was null.");
			}
			if (!arrayProperty.isArray)
			{
				throw new InvalidOperationException("Specified serialized propery is not an array.");
			}
			_arrayProperty = arrayProperty;
			FixedItemHeight = fixedItemHeight;
		}

		public SerializedPropertyAdaptor(SerializedProperty arrayProperty)
			: this(arrayProperty, 0f)
		{
		}

		public virtual bool CanDrag(int index)
		{
			return true;
		}

		public virtual bool CanRemove(int index)
		{
			return true;
		}

		public void Add()
		{
			int arraySize = _arrayProperty.arraySize;
			int num = ++_arrayProperty.arraySize;
			SerializedPropertyUtility.ResetValue(_arrayProperty.GetArrayElementAtIndex(arraySize));
		}

		public void Insert(int index)
		{
			_arrayProperty.InsertArrayElementAtIndex(index);
			SerializedPropertyUtility.ResetValue(_arrayProperty.GetArrayElementAtIndex(index));
		}

		public void Duplicate(int index)
		{
			_arrayProperty.InsertArrayElementAtIndex(index);
		}

		public void Remove(int index)
		{
			SerializedProperty arrayElementAtIndex = _arrayProperty.GetArrayElementAtIndex(index);
			if (arrayElementAtIndex.propertyType == SerializedPropertyType.ObjectReference)
			{
				arrayElementAtIndex.objectReferenceValue = null;
			}
			_arrayProperty.DeleteArrayElementAtIndex(index);
		}

		public void Move(int sourceIndex, int destIndex)
		{
			if (destIndex > sourceIndex)
			{
				destIndex--;
			}
			_arrayProperty.MoveArrayElement(sourceIndex, destIndex);
		}

		public void Clear()
		{
			_arrayProperty.ClearArray();
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
			EditorGUI.PropertyField(position, this[index], GUIContent.none, includeChildren: false);
		}

		public virtual float GetItemHeight(int index)
		{
			if (FixedItemHeight == 0f)
			{
				return EditorGUI.GetPropertyHeight(this[index], GUIContent.none, includeChildren: false);
			}
			return FixedItemHeight;
		}
	}
}
