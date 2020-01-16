using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
	public class Output : BaseModule
	{
		internal static float smoothLoopThreshold = 0.0035f;

		[SerializeField]
		public int sampleRate = 44100;

		[SerializeField]
		public bool smoothLoop;

		[NonSerialized]
		public UnityEngine.AudioClip audioClip;

		public static string MenuEntry()
		{
			return "Output";
		}

		public IEnumerable<float> IGenerate()
		{
			float useDuration = attribute("Duration").value;
			if (smoothLoop)
			{
				int num = Mathf.CeilToInt(useDuration * attribute("Frequency").value);
				useDuration = (float)num / attribute("Frequency").value;
			}
			int samples = (int)(useDuration * (float)sampleRate) + 5;
			int audioSize = samples;
			float[] audioData = new float[audioSize];
			float percentDivisor = (float)audioSize / 100f;
			for (int i = 0; i < audioSize; i++)
			{
				if ((float)i % percentDivisor == 0f)
				{
					yield return (float)i / percentDivisor;
				}
				audioData[i] = amplitude(attribute("Frequency").value, (float)i / (float)sampleRate, useDuration, 1, sampleRate);
			}
			int num2 = samples;
			if (smoothLoop)
			{
				float num3 = audioData[0];
				for (int num4 = samples - 1; num4 >= samples / 2; num4--)
				{
					if (Mathf.Abs(audioData[num4] - num3) < smoothLoopThreshold)
					{
						num2 = num4 - 1;
						break;
					}
				}
			}
			audioClip = UnityEngine.AudioClip.Create(base.name, num2, 1, sampleRate, stream: false);
			Array.Resize(ref audioData, num2);
			audioClip.SetData(audioData, 0);
		}

		public void Generate()
		{
			foreach (float item in IGenerate())
			{
				_ = item;
			}
		}

		public override void InitializeAttributes()
		{
			attributes.Add(new Attribute("Input", _hiddenValue: true));
			Attribute attribute = new Attribute("Frequency", 261.6255f);
			attribute.type = Attribute.AttributeType.FREQUENCY;
			attributes.Add(attribute);
			Attribute attribute2 = new Attribute("Duration", 0.5f);
			attribute2.type = Attribute.AttributeType.FLOAT_POSITIVE;
			attribute2.allowInput = false;
			attributes.Add(attribute2);
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int _)
		{
			Attribute attribute = base.attribute("Input");
			if (attribute != null && attribute.generator != null)
			{
				return attribute.generator.amplitude(frequency, time, duration, depth + 1, sampleRate);
			}
			return 0f;
		}
	}
}
