using DarkArtsStudios.SoundGenerator.Module;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator
{
	internal class SettingsEditor : EditorWindow
	{
		public static bool Debug
		{
			get
			{
				return EditorPrefs.GetBool(key("Debug"), defaultValue: false);
			}
			set
			{
				EditorPrefs.SetBool(key("Debug"), value);
			}
		}

		public static int IconSize
		{
			get
			{
				return EditorPrefs.GetInt(key("IconSize"), 32);
			}
			set
			{
				EditorPrefs.SetInt(key("IconSize"), (value > 19) ? value : 20);
			}
		}

		public static int SamplePreviewHeight
		{
			get
			{
				return EditorPrefs.GetInt(key("SamplePreviewHeight"), 17);
			}
			set
			{
				EditorPrefs.SetInt(key("SamplePreviewHeight"), (value >= 8) ? value : 8);
			}
		}

		private static string key(string keyName)
		{
			return "DarkArtsStudios.SoundGenerator." + keyName;
		}

		public static void ResetAllHideFlags()
		{
			Object[] array = Resources.FindObjectsOfTypeAll(typeof(Composition));
			for (int i = 0; i < array.Length; i++)
			{
				Composition composition = (Composition)array[i];
				foreach (BaseModule module in composition.modules)
				{
					if ((bool)module)
					{
						if (composition.gameObject.activeInHierarchy)
						{
							module.gameObject.hideFlags = ((!Debug) ? HideFlags.HideInHierarchy : HideFlags.None);
						}
						else
						{
							module.gameObject.hideFlags = HideFlags.HideInHierarchy;
							module.hideFlags = ((!Debug) ? HideFlags.HideInHierarchy : HideFlags.None);
						}
					}
				}
				string assetPath = AssetDatabase.GetAssetPath(composition);
				if (assetPath != "")
				{
					AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
				}
			}
			EditorApplication.RepaintHierarchyWindow();
		}

		private void OnGUI()
		{
			GUILayout.Label("Sound Generator Settings");
			IconSize = EditorGUILayout.IntField("Icon Size", IconSize);
			SamplePreviewHeight = EditorGUILayout.IntField("Sample Preview Height", SamplePreviewHeight);
			GUILayout.Space(16f);
			EditorGUILayout.HelpBox("Setting Debug Mode means a lot of Sound Generator's internal behaviour will be exposed. Edit these at your own risk!", MessageType.Warning);
			GUILayout.Space(16f);
			EditorGUI.BeginChangeCheck();
			Debug = GUILayout.Toggle(Debug, "Debug Mode");
			if (EditorGUI.EndChangeCheck())
			{
				ResetAllHideFlags();
			}
		}
	}
}
