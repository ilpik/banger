using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal static class EditorUtility
	{
		private static Dictionary<Object, Editor> editors = new Dictionary<Object, Editor>();

		private static Color webLinkColour = new Color(0.25f, 0.4f, 0.8f);

		private static GUIStyle _webLinkStyle = null;

		internal static GUIStyle webLinkStyle
		{
			get
			{
				if (_webLinkStyle == null)
				{
					_webLinkStyle = new GUIStyle(EditorStyles.largeLabel)
					{
						alignment = TextAnchor.MiddleCenter,
						fontStyle = FontStyle.Bold
					};
					_webLinkStyle.normal.textColor = webLinkColour;
				}
				return _webLinkStyle;
			}
		}

		internal static bool WebLink(GUIContent content)
		{
			bool result = GUILayout.Button(content, webLinkStyle);
			Rect lastRect = GUILayoutUtility.GetLastRect();
			EditorGUIUtility.AddCursorRect(lastRect, MouseCursor.Link);
			lastRect.y += lastRect.height - 2f;
			lastRect.height = 2f;
			Color backgroundColor = GUI.backgroundColor;
			Color color = GUI.color;
			GUI.backgroundColor = webLinkColour;
			GUI.Box(lastRect, "");
			GUI.backgroundColor = backgroundColor;
			GUI.color = color;
			return result;
		}

		internal static bool WebLink(string content)
		{
			return WebLink(GUIUtility.TempContent(content));
		}

		internal static Editor CreateEditor(Object obj)
		{
			if (editors.ContainsKey(obj) && (bool)editors[obj])
			{
				return editors[obj];
			}
			editors[obj] = Editor.CreateEditor(obj);
			Debug.LogWarning("Editors List Size: " + editors.Count);
			return editors[obj];
		}
	}
}
