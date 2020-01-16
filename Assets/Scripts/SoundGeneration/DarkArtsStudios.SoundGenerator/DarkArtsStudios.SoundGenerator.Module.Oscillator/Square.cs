using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public class Square : BaseOscillator
	{
		public static string MenuEntry()
		{
			return "Oscillator/Square";
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			if (!(Mathf.Repeat(time * frequency + attribute("Phase").value, 1f) < 0.5f))
			{
				return 1f;
			}
			return -1f;
		}
	}
}
