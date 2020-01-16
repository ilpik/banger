using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator
{
	[CustomEditor(typeof(Composition), true)]
	internal class CompositionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			Texture2D banner = ProductInformation.primaryProduct.banner;
			EditorGUILayout.BeginVertical(GUILayout.MinWidth(banner.width / 2), GUILayout.MaxWidth(banner.width / 2));
			Rect aspectRect = GUILayoutUtility.GetAspectRect((float)banner.width / (float)banner.height);
			if (GUI.Button(aspectRect, global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GUIUtility.TempContent(banner, "Open Composition Editor")))
			{
				MainMenuItems.OpenCompositionEditorWindow();
			}
			EditorGUIUtility.AddCursorRect(aspectRect, MouseCursor.Link);
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			if (SettingsEditor.Debug)
			{
				Composition composition = base.target as Composition;
				int num = 0;
				foreach (BaseModule module in composition.modules)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.ObjectField(module ? module.name : "?????", module, typeof(BaseModule), true);
					bool num2 = GUILayout.Button("Remove");
					EditorGUILayout.EndHorizontal();
					if (num2)
					{
						CompositionEditor.RemoveModuleById(composition, num);
						break;
					}
					num++;
				}
			}
		}
	}
}
