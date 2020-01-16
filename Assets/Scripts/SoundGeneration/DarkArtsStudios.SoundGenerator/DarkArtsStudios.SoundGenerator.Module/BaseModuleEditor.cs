using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
	[CustomEditor(typeof(BaseModule), true)]
	[InitializeOnLoad]
	internal class BaseModuleEditor : Editor
	{
		public class AttributeAdaptor : GenericListAdaptor<BaseModule.Attribute>
		{
			public AttributeAdaptor(List<BaseModule.Attribute> list, ReorderableListControl.ItemDrawer<BaseModule.Attribute> itemDrawer)
				: base((IList<BaseModule.Attribute>)list, itemDrawer, AttributeHeight)
			{
			}

			public override bool CanRemove(int index)
			{
				if (base.List[index].has_many)
				{
					int num = 0;
					foreach (BaseModule.Attribute item in base.List)
					{
						if (item.has_many && item.name == base.List[index].name)
						{
							num++;
						}
					}
					if (num > 1)
					{
						return true;
					}
				}
				return false;
			}

			public override float GetItemHeight(int index)
			{
				if (base.List[index].type == BaseModule.Attribute.AttributeType.FREQUENCY)
				{
					return AttributeHeight * 2f;
				}
				return AttributeHeight;
			}
		}

		public static float AttributeHeight = 18f;

		[NonSerialized]
		private Texture2D previewTexture;

		public override void OnInspectorGUI()
		{
			if (SettingsEditor.Debug)
			{
				base.OnInspectorGUI();
			}
		}

		private Rect ModuleAttributesGUI(Rect innerRect)
		{
			Rect result = new Rect(innerRect);
			BaseModule baseModule = base.target as BaseModule;
			if (baseModule.attributes.Count > 0)
			{
				ReorderableListGUI.ListField(new AttributeAdaptor(baseModule.attributes, AttributeDrawer), ReorderableListFlags.HideAddButton | ReorderableListFlags.HideRemoveButtons);
				Rect lastRect = GUILayoutUtility.GetLastRect();
				result.height = lastRect.height;
				result.width = lastRect.width;
			}
			else
			{
				result.height = 0f;
			}
			return result;
		}

		private BaseModule.Attribute AttributeDrawer(Rect position, BaseModule.Attribute attribute)
		{
			EditorGUIUtility.labelWidth = attribute.name.Length * 8;
			if (attribute.hiddenValue)
			{
				EditorGUI.LabelField(position, attribute.name);
			}
			else
			{
				if (attribute.type == BaseModule.Attribute.AttributeType.FREQUENCY)
				{
					List<string> list = new List<string>();
					int num = -1;
					int num2 = 0;
					for (int i = 1; i < 7; i++)
					{
						for (int j = 0; j < 12; j++)
						{
							list.Add(i.ToString() + " - " + global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio.Music.NoteName[j]);
							if (attribute.value >= global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio.Music.Frequency(i, j + 1))
							{
								num = num2;
							}
							num2++;
						}
					}
					if (num < 0)
					{
						num = 39;
					}
					int octaveNote = global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio.Music.OctaveNote(attribute.value);
					int selectedValue = global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio.Music.Octave(octaveNote);
					int selectedIndex = global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio.Music.Note(octaveNote);
					EditorGUI.BeginChangeCheck();
					selectedValue = EditorGUI.IntPopup(new Rect(position.x + position.width / 2f, position.y, position.width / 2f, AttributeHeight), selectedValue, new string[8]
					{
						"1",
						"2",
						"3",
						"4",
						"5",
						"6",
						"7",
						"8"
					}, new int[8]
					{
						1,
						2,
						3,
						4,
						5,
						6,
						7,
						8
					});
					selectedIndex = EditorGUI.Popup(new Rect(position.x, position.y, position.width / 2f, AttributeHeight), selectedIndex, global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio.Music.NoteName.ToArray());
					if (EditorGUI.EndChangeCheck())
					{
						global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio.Music.Octave(octaveNote);
						global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio.Music.Note(octaveNote);
						float num3 = attribute.value = global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio.Music.Frequency(selectedValue, selectedIndex);
					}
					EditorGUI.BeginChangeCheck();
					attribute.value = EditorGUI.FloatField(new Rect(position.x, position.y + AttributeHeight, position.width, AttributeHeight), "Frequency", attribute.value);
					if (EditorGUI.EndChangeCheck() && attribute.value < 27.5f)
					{
						attribute.value = 27.5f;
					}
				}
				else
				{
					Rect position2 = EditorGUI.PrefixLabel(position, 0, global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GUIUtility.TempContent(attribute.name));
					attribute.value = EditorGUI.FloatField(position2, attribute.value);
				}
				if (attribute.clampMinimum && attribute.value < attribute.clampMinimumValue)
				{
					attribute.value = attribute.clampMinimumValue;
				}
				if (attribute.clampMaximum && attribute.value > attribute.clampMaximumValue)
				{
					attribute.value = attribute.clampMaximumValue;
				}
			}
			if (attribute.type == BaseModule.Attribute.AttributeType.FLOAT_POSITIVE && attribute.value < 0f)
			{
				attribute.value = 0f;
			}
			return attribute;
		}

		private void AttributeDrawerEmpty()
		{
		}

		public void resetPreviewTexture()
		{
			if (previewTexture != null)
			{
				UnityEngine.Object.DestroyImmediate(previewTexture);
			}
			previewTexture = null;
		}

		public Rect ModuleGUI(Rect innerRect)
		{
			Rect result = new Rect(innerRect);
			result.height = 0f;
			EditorGUI.BeginChangeCheck();
			Rect innerRect2 = new Rect(innerRect);
			innerRect2.height = 0f;
			innerRect2 = ModuleAttributesGUI(innerRect2);
			result.width = 200f;
			result.height += innerRect2.height;
			result.width = Mathf.Max(result.width, innerRect2.width);
			Rect innerRect3 = new Rect(result.x, result.yMax, result.width, 0f);
			innerRect3 = OnModuleGUI(innerRect3);
			result.height += innerRect3.height;
			result.width = Mathf.Max(result.width, innerRect3.width);
			bool flag = EditorGUI.EndChangeCheck();
			BaseModule baseModule = base.target as BaseModule;
			baseModule.dirty |= flag;
			if (baseModule.showPreviewTexture)
			{
				Rect position = new Rect(result.x, result.y + result.height, result.width, SettingsEditor.SamplePreviewHeight);
				if (previewTexture == null)
				{
					previewTexture = new Texture2D((int)position.width, (int)position.height, TextureFormat.ARGB32, mipChain: false);
					baseModule.renderToPreviewTexture(ref previewTexture);
					baseModule.dirty = false;
				}
				GUI.DrawTexture(position, previewTexture);
				result.height += SettingsEditor.SamplePreviewHeight;
			}
			return result;
		}

		public virtual Rect OnModuleGUI(Rect innerRect)
		{
			return innerRect;
		}
	}
}
