using System;
using Assets.Scripts.SoundGeneration.Util;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public class CurvedTriangle : BaseOscillator
	{
		public static string MenuEntry()
		{
			return "Oscillator/Curved Triangle";
		}

        public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			return (Math.Pow((MathUtil.Repeat(time * frequency.getAmplitudeOrValue(time, depth + 1, sampleRate) + attribute("Phase").value, 1f) - 0.5f) * 2f, 2f) - 0.5f) * 2f;
		}
	}
}
