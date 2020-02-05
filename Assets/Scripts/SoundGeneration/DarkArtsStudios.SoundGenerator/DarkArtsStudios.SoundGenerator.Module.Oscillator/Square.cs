using System;
using Assets.Scripts.SoundGeneration.Util;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public class Square : BaseOscillator
	{
		public static string MenuEntry()
		{
			return "Oscillator/Square";
		}

        public override double OnAmplitude(double time, int depth, int sampleRate)
        {
            if (frequency.generator == null)
                return 0f;

			if (!(MathUtil.Repeat(time * frequency.generator.amplitude(time, depth + 1, sampleRate) + phase.getAmplitudeOrValue(time, depth + 1, sampleRate), 1f) < 0.5f))
			{
				return 1f;
			}
			return -1f;
		}
	}
}
