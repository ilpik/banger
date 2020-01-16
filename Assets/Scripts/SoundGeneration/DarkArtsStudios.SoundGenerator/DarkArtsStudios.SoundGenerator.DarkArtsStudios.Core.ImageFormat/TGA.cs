using System;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.ImageFormat
{
	public static class TGA
	{
		private enum TypeID : byte
		{
			NoImageData = 0,
			UncompressedColourMapped = 1,
			UncompressedRGB = 2,
			UncompressedBlackAndWhite = 3,
			RunlengthEncodedColourMapped = 9,
			RunlengthEncodedRGB = 10,
			CompressedBlackAndWhite = 11,
			CompressedColourMappedHuffmanDeltaRunlength = 0x20,
			CompressedColourMappedHuffmanDeltaRunlength4PassQuatTree = 33
		}

		public static byte[] Encode(Texture2D texture)
		{
			byte[] array = new byte[18]
			{
				0,
				0,
				2,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				(byte)(texture.width % 256),
				(byte)(texture.width / 256),
				(byte)(texture.height % 256),
				(byte)(texture.height / 256),
				32,
				0
			};
			int num = array.Length;
			Array.Resize(ref array, array.Length + texture.width * texture.height * 4);
			for (int i = 0; i < texture.height; i++)
			{
				for (int j = 0; j < texture.width; j++)
				{
					Color pixel = texture.GetPixel(j, i);
					array[num++] = (byte)Mathf.RoundToInt(pixel.b * 255f);
					array[num++] = (byte)Mathf.RoundToInt(pixel.g * 255f);
					array[num++] = (byte)Mathf.RoundToInt(pixel.r * 255f);
					array[num++] = (byte)Mathf.RoundToInt(pixel.a * 255f);
				}
			}
			return array;
		}
	}
}
