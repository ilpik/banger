using System;

namespace DarkArtsStudios.SoundGenerator.Module.Filter
{
	[Serializable]
	public class FrequencySetter : BaseModule
	{
		public static string MenuEntry()
		{
			return "Filter/Frequency Setter";
		}

		public override void InitializeAttributes()
		{
			attributes.Add(new Attribute("Input", _hiddenValue: true));
			Attribute attribute = new Attribute("Frequency");
			attribute.value = 261.626f;
			attribute.type = Attribute.AttributeType.FREQUENCY;
			attribute.clampMinimum = true;
			attribute.clampMinimumValue = 27.5f;
			attributes.Add(attribute);
		}

		public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			double result = 0f;
			if (depth > BaseModule.TOODEEP)
			{
				return result;
			}
			double frequency = attribute("Frequency").value;
			if ((bool)attribute("Frequency").generator)
			{
				frequency = attribute("Frequency").generator.amplitude(time, depth + 1, sampleRate);
			}
			if ((bool)attribute("Input").generator)
			{
				result = attribute("Input").generator.amplitude(time, depth + 1, sampleRate);
			}
			return result;
		}
	}
}
