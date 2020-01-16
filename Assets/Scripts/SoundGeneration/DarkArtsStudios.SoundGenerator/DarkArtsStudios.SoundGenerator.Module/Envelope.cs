using System;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
	[Serializable]
	public class Envelope : BaseModule
	{
		public enum EnvelopeType
		{
			Duration,
			Time,
			WaveLength
		}

		[SerializeField]
		public AnimationCurve envelope = new AnimationCurve(new Keyframe(0f, -1f), new Keyframe(1f, 1f));

		[SerializeField]
		public EnvelopeType envelopeType;

		public static string MenuEntry()
		{
			return "Envelope";
		}

		public override void InitializeAttributes()
		{
			showPreviewTexture = false;
		}

		public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			switch (envelopeType)
			{
			case EnvelopeType.Duration:
				return envelope.Evaluate(time / duration);
			case EnvelopeType.Time:
				return envelope.Evaluate(time);
			case EnvelopeType.WaveLength:
				return envelope.Evaluate(time * frequency);
			default:
				return 0f;
			}
		}
	}
}
