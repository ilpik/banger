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

        public override double OnAmplitude(double time, int depth, int sampleRate)
        {
            if (this.frequency == null)
                return 0.0f;

            //var gen = frequency.generator;
            //if (gen == null)
            //    return 0f;

            var mod = modulation1.generator == null ? 1f : modulation1.getAmplitudeOrValue(time, depth, sampleRate);

            var arg = time * frequency.getAmplitudeOrValue(time, depth, sampleRate) * mod + phase.value;
            return Math.Sin(Math.PI * 2 * arg);
		}
	}
}
