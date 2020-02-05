using System;
using Assets.Scripts.SoundGeneration.Util;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
	[Serializable]
	public class AudioClip : BaseModule
	{
		public UnityEngine.AudioClip audioClip;

		[NonSerialized]
		public float[] audioData;

		public static string MenuEntry()
		{
			return "Assets/AudioClip";
		}

        public Attribute frequency;

        public override void InitializeAttributes()
        {
            base.InitializeAttributes();
            frequency = AddFrequency();
        }

        public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			float num = 0f;
			if (audioClip != null)
			{
				if (audioData == null)
				{
					audioData = new float[audioClip.samples * audioClip.channels];
					audioClip.GetData(audioData, 0);
				}
				int num2 = MathUtil.RoundToInt(time * (float)audioClip.frequency * (frequency.getAmplitudeOrValue(time, depth + 1, sampleRate) / 261.6255f));
				if (num2 >= 0 && num2 < audioClip.samples)
				{
					for (int i = 0; i < audioClip.channels; i++)
					{
						num += audioData[num2 * audioClip.channels + i];
					}
					num /= (float)audioClip.channels;
				}
			}
			return num;
		}
	}
}
