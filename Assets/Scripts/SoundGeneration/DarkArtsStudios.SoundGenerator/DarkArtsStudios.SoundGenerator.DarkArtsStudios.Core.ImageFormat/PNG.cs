using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.ImageFormat
{
	internal static class PNG
	{
		public static byte[] Encode(Texture2D texture)
		{
			return texture.EncodeToPNG();
		}
	}
}
