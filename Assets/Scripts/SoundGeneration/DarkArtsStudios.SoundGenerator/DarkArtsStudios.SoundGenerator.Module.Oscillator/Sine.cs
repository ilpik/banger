using System;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public class Sine : BaseOscillator
	{
		public static string MenuEntry()
		{
			return "Oscillator/Sine";
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			return Mathf.Sin((float)Math.PI / 180f * ((time * frequency + attribute("Phase").value) * 360f));
		}
	}
}
