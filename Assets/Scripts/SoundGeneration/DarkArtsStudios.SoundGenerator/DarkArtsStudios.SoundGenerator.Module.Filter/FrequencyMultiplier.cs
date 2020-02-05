using System;

namespace DarkArtsStudios.SoundGenerator.Module.Filter
{
	[Serializable]
	public class FrequencyMultiplier : BaseModule
	{
		public static string MenuEntry()
		{
			return "Filter/Frequency Multiplier";
		}

        public Attribute frequency;

        public override void InitializeAttributes()
		{
			attributes.Add(new Attribute("Input", _hiddenValue: true));
			attributes.Add(new Attribute("Multiplier"));
			attribute("Multiplier").value = 1f;

            frequency = AddFrequency();
        }

		public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			double result = 0f;
			if (depth > BaseModule.TOODEEP)
			{
				return result;
			}

			double num = attribute("Multiplier").value;
			if ((bool)attribute("Multiplier").generator)
			{
				num *= attribute("Multiplier").generator.amplitude(time, depth + 1, sampleRate);
			}
			if ((bool)attribute("Input").generator)
			{
				double frequency2 = frequency.getAmplitudeOrValue(time, depth, sampleRate) * num;
				result = attribute("Input").generator.amplitude(time, depth + 1, sampleRate);
			}
			return result;
		}
	}
}
