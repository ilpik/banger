using System;
using Assets.Scripts.SoundGeneration.Util;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public class Triangle : BaseOscillator
	{
		public static string MenuEntry()
		{
			return "Oscillator/Triangle";
		}

        public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			return (Math.Abs(MathUtil.Repeat(time * frequency.getAmplitudeOrValue(time, depth + 1, sampleRate) + phase.getAmplitudeOrValue(time, depth + 1, sampleRate), 1f) - 0.5f) - 0.25f) * 4f;
		}
	}
}
