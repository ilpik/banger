using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
	public class Output : DAAudioFilter
    {
		internal static float smoothLoopThreshold = 0.0035f;

		[SerializeField]
		public int sampleRate = 44100;

        [NonSerialized]
		public UnityEngine.AudioClip audioClip;

		public static string MenuEntry()
		{
			return "Output";
		}

		public IEnumerable<float> IGenerate()
		{
			float useDuration = duration.value;
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
				audioData[i] = (float)amplitude((float)i / (float)sampleRate, 1, sampleRate);
			}
			int num2 = samples;
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
            sampleRate = AudioSettings.outputSampleRate;
            input = AddInput();
            duration = AddAttribute("Duration",
                b => b.WithValue(0.5f).WithType(Attribute.AttributeType.FLOAT_POSITIVE).WithInput(false));
		}

        public Attribute duration;

        public Attribute input;

		public override double OnAmplitude(double time, int depth, int _)
        {
            return input.getAmplitudeOrValue(time, depth + 1, sampleRate);
        }
	}
}
