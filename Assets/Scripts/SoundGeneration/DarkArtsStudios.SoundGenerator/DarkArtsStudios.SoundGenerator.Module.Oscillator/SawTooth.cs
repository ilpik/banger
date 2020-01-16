using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public class SawTooth : BaseOscillator
	{
		public static string MenuEntry()
		{
			return "Oscillator/SawTooth";
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			return (Mathf.Repeat(time * frequency + attribute("Phase").value, 1f) - 0.5f) * 2f;
		}
	}
}
