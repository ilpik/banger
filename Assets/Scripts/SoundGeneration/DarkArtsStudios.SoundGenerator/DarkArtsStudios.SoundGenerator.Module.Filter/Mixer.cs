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

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			float num = 0f;
			if (depth > BaseModule.TOODEEP)
			{
				return num;
			}
			foreach (Attribute attribute in attributes)
			{
				if ((bool)attribute.generator)
				{
					num += attribute.generator.amplitude(frequency, time, duration, depth + 1, sampleRate);
				}
			}
			return num;
		}
	}
}
