using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public class Noise : BaseOscillator
	{
		public static string MenuEntry()
		{
			return "Oscillator/Noise";
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			return Random.value * 2f - 1f;
		}
	}
}
