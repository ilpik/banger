using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public class CurvedTriangle : BaseOscillator
	{
		public static string MenuEntry()
		{
			return "Oscillator/Curved Triangle";
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			return (Mathf.Pow((Mathf.Repeat(time * frequency + attribute("Phase").value, 1f) - 0.5f) * 2f, 2f) - 0.5f) * 2f;
		}
	}
}
