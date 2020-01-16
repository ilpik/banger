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

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			float num = 0f;
			if (depth > BaseModule.TOODEEP)
			{
				return num;
			}
			if ((bool)attribute("Input").generator)
			{
				num += attribute("Input").generator.amplitude(frequency, duration - time, duration, depth + 1, sampleRate);
			}
			return num;
		}
	}
}
