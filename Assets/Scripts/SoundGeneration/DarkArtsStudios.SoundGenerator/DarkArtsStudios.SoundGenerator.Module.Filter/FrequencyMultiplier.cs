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

		public override void InitializeAttributes()
		{
			attributes.Add(new Attribute("Input", _hiddenValue: true));
			attributes.Add(new Attribute("Multiplier"));
			attribute("Multiplier").value = 1f;
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			float result = 0f;
			if (depth > BaseModule.TOODEEP)
			{
				return result;
			}
			float num = attribute("Multiplier").value;
			if ((bool)attribute("Multiplier").generator)
			{
				num *= attribute("Multiplier").generator.amplitude(frequency, time, duration, depth + 1, sampleRate);
			}
			if ((bool)attribute("Input").generator)
			{
				float frequency2 = frequency * num;
				result = attribute("Input").generator.amplitude(frequency2, time, duration, depth + 1, sampleRate);
			}
			return result;
		}
	}
}
