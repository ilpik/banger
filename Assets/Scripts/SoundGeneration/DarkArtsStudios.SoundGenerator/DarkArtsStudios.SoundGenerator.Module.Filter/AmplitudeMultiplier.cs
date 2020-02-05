using System;

namespace DarkArtsStudios.SoundGenerator.Module.Filter
{
	[Serializable]
	public class AmplitudeMultiplier : BaseModule
	{
		public static string MenuEntry()
		{
			return "Filter/Amplitude Multiplier";
		}

		public override void InitializeAttributes()
		{
			attributes.Add(new Attribute("Input", _hiddenValue: true));
			attributes.Add(new Attribute("Multiplier", 1f));
		}

		public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			double num = 0;

			double num2 = attribute("Multiplier").value;
			if ((bool)attribute("Multiplier").generator)
			{
				num2 *= attribute("Multiplier").generator.amplitude(time, depth + 1, sampleRate);
			}
			if ((bool)attribute("Input").generator)
			{
				num += attribute("Input").generator.amplitude(time, depth + 1, sampleRate);
			}
			return num * num2;
		}
	}
}
