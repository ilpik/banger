using System;

namespace DarkArtsStudios.SoundGenerator.Module
{
	[Serializable]
	public class Composition : BaseModule
	{
		public global::DarkArtsStudios.SoundGenerator.Composition composition;

		public static string MenuEntry()
		{
			return "Assets/Composition";
		}

		public override void InitializeAttributes()
		{
			attributes.Clear();
			if (!(composition == null))
			{
				foreach (BaseModule module in composition.modules)
				{
					foreach (Attribute attribute in module.attributes)
					{
						if (!(module is Output) && attribute.allowInput && attribute.generator == null)
						{
							attributes.Add(attribute);
						}
					}
				}
			}
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			float num = 0f;
			if (composition != null)
			{
				foreach (BaseModule module in composition.modules)
				{
					if (module is Output)
					{
						num += (module as Output).OnAmplitude(frequency, time, duration, depth + 1, sampleRate);
					}
				}
				return num;
			}
			return num;
		}
	}
}
