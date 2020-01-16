using System;

namespace DarkArtsStudios.SoundGenerator.Module.Filter
{
	[Serializable]
	public class Harmonics : BaseModule
	{
		public static string MenuEntry()
		{
			return "Filter/Harmonics";
		}

		public override void InitializeAttributes()
		{
			attributes.Add(new Attribute("Input", _hiddenValue: true));
			Attribute attribute = new Attribute("Tone Offset", 1f);
			attribute.clampMinimum = true;
			attribute.clampMinimumValue = 1f;
			attribute.allowInput = false;
			attribute.type = Attribute.AttributeType.SLIDER;
			attributes.Add(attribute);
			Attribute attribute2 = new Attribute("Tone Strength", 1f);
			attribute2.type = Attribute.AttributeType.FLOAT_POSITIVE;
			attributes.Add(attribute2);
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			float num = 0f;
			if (depth > BaseModule.TOODEEP)
			{
				return num;
			}
			Attribute attribute = base.attribute("Input");
			Attribute attribute2 = base.attribute("Tone Offset");
			Attribute attribute3 = base.attribute("Tone Strength");
			float num2 = attribute3.value;
			if ((bool)attribute3.generator)
			{
				num2 *= attribute3.generator.amplitude(frequency, time, duration, depth + 1, sampleRate);
			}
			if ((bool)attribute.generator)
			{
				num += attribute.generator.amplitude(frequency, time, duration, depth + 1, sampleRate);
				num += attribute.generator.amplitude(frequency / (attribute2.value + 1f), time, duration, depth + 1, sampleRate) * num2;
				num += attribute.generator.amplitude(frequency * (attribute2.value + 1f), time, duration, depth + 1, sampleRate) * num2;
			}
			return num;
		}
	}
}
