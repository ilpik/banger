using System;

namespace DarkArtsStudios.SoundGenerator.Module.Filter
{

	[Serializable]
	public class Mixer : BaseModule
	{
		public static string MenuEntry()
		{
			return "Filter/Mixer";
		}

		public override void InitializeAttributes()
		{
			attributes.Add(new Attribute("Input", _hiddenValue: true));
			attributes.Add(new Attribute("Input", _hiddenValue: true));
			attributes.Add(new Attribute("Input", _hiddenValue: true));
			attributes.Add(new Attribute("Input", _hiddenValue: true));
		}

		public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			double num = 0f;

			foreach (Attribute attribute in attributes)
			{
				if ((bool)attribute.generator)
				{
					num += attribute.generator.amplitude(time, depth + 1, sampleRate);
				}
			}
			return num;
		}
	}
}
