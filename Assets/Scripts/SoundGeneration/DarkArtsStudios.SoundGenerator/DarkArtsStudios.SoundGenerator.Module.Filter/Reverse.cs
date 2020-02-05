using System;

namespace DarkArtsStudios.SoundGenerator.Module.Filter
{
	[Serializable]
	public class Reverse : BaseModule
	{
		public static string MenuEntry()
		{
			return "Filter/Reverse";
		}

		public override void InitializeAttributes()
		{
			attributes.Add(new Attribute("Input", _hiddenValue: true));
		}

		public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			double num = 0f;
			if ((bool)attribute("Input").generator)
			{
				num += attribute("Input").generator.amplitude(time, depth + 1, sampleRate);
			}
			return num;
		}
	}
}
