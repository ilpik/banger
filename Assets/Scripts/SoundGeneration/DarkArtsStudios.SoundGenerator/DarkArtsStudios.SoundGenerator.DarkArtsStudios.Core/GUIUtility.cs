using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal class GUIUtility
	{
		private static Texture2D s_DrawTexture = new Texture2D(1, 1);

		private static GUIContent s_TempContent = new GUIContent();

		private static GUIContent s_TempText = new GUIContent();

		private static GUIContent s_TempTextTooltip = new GUIContent();

		private static GUIContent s_TempImage = new GUIContent();

		private static GUIContent s_TempImageText = new GUIContent();

		private static GUIContent s_TempImageTooltip = new GUIContent();

		protected static void DrawQuad(Rect position, Color color)
		{
			s_DrawTexture.SetPixel(0, 0, color);
			s_DrawTexture.Apply();
			Texture2D background = GUI.skin.box.normal.background;
			GUI.skin.box.normal.background = s_DrawTexture;
			GUI.Box(position, GUIContent.none);
			GUI.skin.box.normal.background = background;
		}

		internal static GUIContent TempContent(string text, Texture2D image, string tooltip)
		{
			s_TempContent.text = text;
			s_TempContent.tooltip = tooltip;
			s_TempContent.image = image;
			return s_TempContent;
		}

		internal static GUIContent TempContent(string text)
		{
			s_TempText.text = text;
			return s_TempText;
		}

		internal static GUIContent TempContent(string text, string tooltip)
		{
			s_TempTextTooltip.text = text;
			s_TempTextTooltip.tooltip = tooltip;
			return s_TempTextTooltip;
		}

		internal static GUIContent TempContent(Texture2D image)
		{
			s_TempImage.image = image;
			return s_TempImage;
		}

		internal static GUIContent TempContent(Texture2D image, string tooltip)
		{
			s_TempImageTooltip.tooltip = tooltip;
			s_TempImageTooltip.image = image;
			return s_TempImageTooltip;
		}

		internal static GUIContent TempContent(string text, Texture2D image)
		{
			s_TempImageText.text = text;
			s_TempImageText.image = image;
			return s_TempImageText;
		}
	}
}
