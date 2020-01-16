using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal static class TextureUtility
	{
		private static int FLATTEN_TEXTURE_ALPHA_GRID_SIZE = 8;

		private static Color FLATTEN_TEXTURE_ALPHA_COLOUR_A = new Color(0.6f, 0.6f, 0.6f);

		private static Color FLATTEN_TEXTURE_ALPHA_COLOUR_B = new Color(0.4f, 0.4f, 0.4f);

		public static Texture2D FlattenAlpha(Texture2D texture, Color colourA, Color colourB, int gridSize)
		{
			Texture2D texture2D = new Texture2D(texture.width, texture.height);
			Color[] array = new Color[texture.width * texture.height];
			for (int i = 0; i < array.Length; i++)
			{
				int num = i % texture.width;
				int num2 = i / texture.width;
				Color a = ((num / gridSize + num2 / gridSize) % 2 == 0) ? colourA : colourB;
				Color pixel = texture.GetPixel(num, num2);
				array[i] = Color.Lerp(a, pixel, pixel.a);
			}
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}

		public static Texture2D FlattenAlpha(Texture2D texture, Color colourA, Color colourB)
		{
			return FlattenAlpha(texture, colourA, colourB, FLATTEN_TEXTURE_ALPHA_GRID_SIZE);
		}

		public static Texture2D FlattenAlpha(Texture2D texture, int gridSize)
		{
			return FlattenAlpha(texture, FLATTEN_TEXTURE_ALPHA_COLOUR_A, FLATTEN_TEXTURE_ALPHA_COLOUR_B, gridSize);
		}

		public static Texture2D FlattenAlpha(Texture2D texture)
		{
			return FlattenAlpha(texture, FLATTEN_TEXTURE_ALPHA_COLOUR_A, FLATTEN_TEXTURE_ALPHA_COLOUR_B);
		}

		public static void Overlay(Texture2D texture, Texture2D overlay, int x, int y, float xScale, float yScale)
		{
			for (int i = 0; (float)i < (float)overlay.height * xScale; i++)
			{
				for (int j = 0; (float)j < (float)overlay.width * yScale; j++)
				{
					Color pixel = overlay.GetPixel(Mathf.RoundToInt((float)j / xScale), Mathf.RoundToInt((float)i / yScale));
					Color color = Color.Lerp(texture.GetPixel(x + j, y + i), pixel, pixel.a);
					texture.SetPixel(x + j, y + i, color);
				}
			}
		}
	}
}
