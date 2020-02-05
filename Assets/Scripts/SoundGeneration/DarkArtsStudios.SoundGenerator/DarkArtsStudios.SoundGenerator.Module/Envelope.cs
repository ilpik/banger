using System;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
	[Serializable]
	public class Envelope : BaseModule
	{
		public enum EnvelopeType
		{
			Time,
			WaveLength
		}

		[SerializeField]
		public AnimationCurve envelope = new AnimationCurve(new Keyframe(0f, -1f), new Keyframe(1f, 1f));

        public Attribute frequency;

        [SerializeField]
		public EnvelopeType envelopeType;

		public static string MenuEntry()
		{
			return "Envelope";
		}

		public override void InitializeAttributes()
		{
			showPreviewTexture = false;
            frequency = AddFrequency();
        }

		public override double OnAmplitude(double time, int depth, int sampleRate)
		{
			switch (envelopeType)
			{
			case EnvelopeType.Time:
				return envelope.Evaluate((float)time);
			case EnvelopeType.WaveLength:
				return envelope.Evaluate((float)(time * frequency.getAmplitudeOrValue(time, depth + 1, sampleRate)));
			default:
				return 0f;
			}
		}
	}
}
