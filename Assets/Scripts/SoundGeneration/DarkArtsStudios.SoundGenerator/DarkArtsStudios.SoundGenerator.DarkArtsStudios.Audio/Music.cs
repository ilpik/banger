using System.Collections.Generic;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio
{
	internal class Music
	{
		public static List<string> NoteName = new List<string>
		{
			"A",
			"A#",
			"B",
			"C",
			"C#",
			"D",
			"D#",
			"E",
			"F",
			"F#",
			"G",
			"G#"
		};

		public static int Octave(int octaveNote)
		{
			return octaveNote / 12 + 1;
		}

		public static int Note(int octaveNote)
		{
			return octaveNote % 12;
		}

		public static int OctaveNote(int octave, int note)
		{
			return (octave - 1) * 12 + note;
		}

		public static int OctaveNote(float frequency)
		{
			float num = Mathf.Log(frequency / 440f, 2f) + 4f;
			int num2 = Mathf.RoundToInt(num);
			int note = Mathf.RoundToInt(1200f * (num - (float)num2) / 100f) % 12;
			return OctaveNote(num2 + 1, note);
		}

		public static float Frequency(int octaveNote)
		{
			return 440f * Mathf.Pow(2f, ((float)octaveNote - 48f) / 12f);
		}

		public static float Frequency(int octave, int note)
		{
			return Frequency(OctaveNote(octave, note));
		}
	}
}
