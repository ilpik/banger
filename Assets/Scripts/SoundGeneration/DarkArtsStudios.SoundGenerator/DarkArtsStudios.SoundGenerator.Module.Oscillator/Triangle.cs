using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public class Triangle : BaseOscillator
	{
		public static string MenuEntry()
		{
			return "Oscillator/Triangle";
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			return (Mathf.Abs(Mathf.Repeat(time * frequency + attribute("Phase").value, 1f) - 0.5f) - 0.25f) * 4f;
		}
	}
}
