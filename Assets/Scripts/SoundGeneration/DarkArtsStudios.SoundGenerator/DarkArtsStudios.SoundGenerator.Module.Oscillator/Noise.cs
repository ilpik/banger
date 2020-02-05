using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public class Noise : BaseOscillator
	{
		public static string MenuEntry()
		{
			return "Oscillator/Noise";
		}

		public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			return Random.value * 2f - 1f;
		}
	}
}
