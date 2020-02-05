using Assets.Scripts.SoundGeneration.Util;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public class SawTooth : BaseOscillator
	{
		public static string MenuEntry()
		{
			return "Oscillator/SawTooth";
		}

        public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			return (MathUtil.Repeat(time * frequency.getAmplitudeOrValue(time, depth + 1, sampleRate) + phase.getAmplitudeOrValue(time, depth + 1, sampleRate), 1f) - 0.5f) * 2f;
		}
	}
}
