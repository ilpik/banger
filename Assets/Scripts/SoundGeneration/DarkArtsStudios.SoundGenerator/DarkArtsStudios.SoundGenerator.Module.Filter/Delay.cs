using System;

namespace DarkArtsStudios.SoundGenerator.Module.Filter
{
	[Serializable]
	public class Delay : BaseModule
	{
		public static string MenuEntry()
		{
			return "Filter/Delay";
		}

		public override void InitializeAttributes()
		{
			attributes.Add(new Attribute("Input", _hiddenValue: true));
			attributes.Add(new Attribute("Delay"));
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			float num = 0f;
			if (depth > BaseModule.TOODEEP)
			{
				return num;
			}
			float num2 = attribute("Delay").value;
			if ((bool)attribute("Delay").generator)
			{
				num2 *= attribute("Delay").generator.amplitude(frequency, time, duration, depth + 1, sampleRate);
			}
			if ((bool)attribute("Input").generator)
			{
				num += attribute("Input").generator.amplitude(frequency, time - num2, duration, depth + 1, sampleRate);
			}
			return num;
		}
	}
}
