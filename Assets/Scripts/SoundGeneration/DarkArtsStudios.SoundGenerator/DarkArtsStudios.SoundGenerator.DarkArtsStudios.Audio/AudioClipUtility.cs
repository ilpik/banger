using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio
{
	internal class AudioClipUtility
	{
		private static byte[] LittleEndianBinary(int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}

		private static byte[] LittleEndianBinary(short value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}

		public static void ExportWAV(AudioClip audioClip, string wavFilename)
		{
			BinaryWriter binaryWriter = new BinaryWriter(new FileStream(wavFilename, FileMode.Create));
			binaryWriter.Write(Encoding.ASCII.GetBytes("RIFF"));
			binaryWriter.Write(LittleEndianBinary(36 + audioClip.samples * 2));
			binaryWriter.Write(Encoding.ASCII.GetBytes("WAVE"));
			binaryWriter.Write(Encoding.ASCII.GetBytes("fmt "));
			binaryWriter.Write(LittleEndianBinary(16));
			binaryWriter.Write(LittleEndianBinary((short)1));
			binaryWriter.Write(LittleEndianBinary((short)1));
			binaryWriter.Write(LittleEndianBinary(audioClip.frequency));
			binaryWriter.Write(LittleEndianBinary(audioClip.frequency * 2));
			binaryWriter.Write(LittleEndianBinary((short)2));
			binaryWriter.Write(LittleEndianBinary((short)16));
			binaryWriter.Write(Encoding.ASCII.GetBytes("data"));
			binaryWriter.Write(LittleEndianBinary(audioClip.samples * 2));
			float[] array = new float[audioClip.samples];
			audioClip.GetData(array, 0);
			bool flag = false;
			float num = 0f;
			for (int i = 0; i < audioClip.samples; i++)
			{
				float num2 = array[i] * 32767f;
				if (num2 > 32767f)
				{
					flag = true;
					num = Mathf.Max(num, num2 - 32767f);
					num2 = 32767f;
				}
				if (num2 < -32768f)
				{
					flag = true;
					num = Mathf.Max(num, -32768f - num2);
					num2 = -32768f;
				}
				short value = Convert.ToInt16(num2);
				binaryWriter.Write(LittleEndianBinary(value));
			}
			binaryWriter.Close();
			if (flag)
			{
				int num3 = Mathf.Min(Mathf.Abs(-32768), 32767);
				float num4 = num * 100f / ((float)num3 + num);
				Debug.LogWarning($"Sample overflow while exporting WAV: Reduce volume by about {num4}% to avoid data loss!");
			}
		}
	}
}
