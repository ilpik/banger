using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList.Internal
{
	internal static class GUIHelper
	{
		public static Func<Rect> VisibleRect;

		public static Action<string> FocusTextInControl;

		private static GUIStyle s_TempStyle;

		private static GUIContent s_TempIconContent;

		private static readonly int s_IconButtonHint;

		private static readonly Color s_SeparatorColor;

		private static readonly GUIStyle s_SeparatorStyle;

		static GUIHelper()
		{
			s_TempStyle = new GUIStyle();
			s_TempIconContent = new GUIContent();
			s_IconButtonHint = "_ReorderableIconButton_".GetHashCode();
			Type type = Type.GetType("UnityEngine.GUIClip,UnityEngine");
			if (type != null)
			{
				PropertyInfo property = type.GetProperty("visibleRect", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					MethodInfo method = property.GetGetMethod(nonPublic: true) ?? property.GetGetMethod(nonPublic: false);
					VisibleRect = (Func<Rect>)Delegate.CreateDelegate(typeof(Func<Rect>), method);
				}
			}
			MethodInfo method2 = typeof(EditorGUI).GetMethod("FocusTextInControl", BindingFlags.Static | BindingFlags.Public);
			if (method2 == null)
			{
				method2 = typeof(GUI).GetMethod("FocusControl", BindingFlags.Static | BindingFlags.Public);
			}
			FocusTextInControl = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), method2);
			s_SeparatorColor = (EditorGUIUtility.isProSkin ? new Color(0.11f, 0.11f, 0.11f) : new Color(0.5f, 0.5f, 0.5f));
			s_SeparatorStyle = new GUIStyle();
			s_SeparatorStyle.normal.background = EditorGUIUtility.whiteTexture;
			s_SeparatorStyle.stretchWidth = true;
		}

		public static void DrawTexture(Rect position, Texture2D texture)
		{
			if (Event.current.type == EventType.Repaint)
			{
				s_TempStyle.normal.background = texture;
				s_TempStyle.Draw(position, GUIContent.none, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
			}
		}

		public static bool IconButton(Rect position, bool visible, Texture2D iconNormal, Texture2D iconActive, GUIStyle style)
		{
			int controlID = GUIUtility.GetControlID(s_IconButtonHint, FocusType.Passive);
			bool result = false;
			position.height += 1f;
			switch (Event.current.GetTypeForControl(controlID))
			{
			case EventType.MouseDown:
				if (GUI.enabled && Event.current.button != 1 && position.Contains(Event.current.mousePosition))
				{
					GUIUtility.hotControl = controlID;
					GUIUtility.keyboardControl = 0;
					Event.current.Use();
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlID)
				{
					Event.current.Use();
				}
				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl == controlID)
				{
					GUIUtility.hotControl = 0;
					result = position.Contains(Event.current.mousePosition);
					Event.current.Use();
				}
				break;
			case EventType.Repaint:
				if (visible)
				{
					bool flag = GUIUtility.hotControl == controlID && position.Contains(Event.current.mousePosition);
					s_TempIconContent.image = (flag ? iconActive : iconNormal);
					position.height -= 1f;
					style.Draw(position, s_TempIconContent, flag, flag, on: false, hasKeyboardFocus: false);
				}
				break;
			}
			return result;
		}

		public static bool IconButton(Rect position, Texture2D iconNormal, Texture2D iconActive, GUIStyle style)
		{
			return IconButton(position, visible: true, iconNormal, iconActive, style);
		}

		public static void Separator(Rect position, Color color)
		{
			if (Event.current.type == EventType.Repaint)
			{
				Color color2 = GUI.color;
				GUI.color = color;
				s_SeparatorStyle.Draw(position, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
				GUI.color = color2;
			}
		}

		public static void Separator(Rect position)
		{
			Separator(position, s_SeparatorColor);
		}
	}
}
