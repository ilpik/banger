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

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			float num = 0f;
			if (depth > BaseModule.TOODEEP)
			{
				return num;
			}
			float num2 = attribute("Multiplier").value;
			if ((bool)attribute("Multiplier").generator)
			{
				num2 *= attribute("Multiplier").generator.amplitude(frequency, time, duration, depth + 1, sampleRate);
			}
			if ((bool)attribute("Input").generator)
			{
				num += attribute("Input").generator.amplitude(frequency, time, duration, depth + 1, sampleRate);
			}
			return num * num2;
		}
	}
}
