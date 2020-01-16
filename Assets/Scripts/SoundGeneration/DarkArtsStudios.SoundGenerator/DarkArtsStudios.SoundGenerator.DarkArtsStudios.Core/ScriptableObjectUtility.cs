using System.IO;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal static class ScriptableObjectUtility
	{
		public static void CreateAsset<T>(string name) where T : ScriptableObject
		{
			T val = ScriptableObject.CreateInstance<T>();
			string text = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (text == "")
			{
				text = "Assets";
			}
			else if (System.IO.Path.GetExtension(text) != "")
			{
				text = text.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
			}
			if (name == null)
			{
				name = "New " + typeof(T).ToString();
			}
			string path = AssetDatabase.GenerateUniqueAssetPath(text + "/" + name + ".asset");
			AssetDatabase.CreateAsset(val, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			UnityEditor.EditorUtility.FocusProjectWindow();
			Selection.activeObject = val;
		}

		public static void CreateAsset<T>() where T : ScriptableObject
		{
			CreateAsset<T>(null);
		}
	}
}
