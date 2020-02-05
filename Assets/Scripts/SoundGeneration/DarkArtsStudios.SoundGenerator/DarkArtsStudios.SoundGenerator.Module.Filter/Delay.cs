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

		public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			double num = 0f;
			if (depth > BaseModule.TOODEEP)
			{
				return num;
			}
			double num2 = attribute("Delay").value;
			if ((bool)attribute("Delay").generator)
			{
				num2 *= attribute("Delay").generator.amplitude(time, depth + 1, sampleRate);
			}
			if ((bool)attribute("Input").generator)
			{
				num += attribute("Input").generator.amplitude(time - num2, depth + 1, sampleRate);
			}
			return num;
		}
	}
}
