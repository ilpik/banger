using System;
using System.Collections.Generic;
using Assets.Scripts.SoundGeneration.AttributeBuilder;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
	[ExecuteInEditMode]
	public abstract class BaseModule : MonoBehaviour
	{
		[Serializable]
		public class Attribute
		{
			public enum AttributeType
			{
				FLOAT,
				FLOAT_POSITIVE,
				FREQUENCY,
				SLIDER
			}

			[SerializeField]
			public string name;

			[SerializeField]
			public float value;

			[SerializeField]
			public BaseModule generator;

			[SerializeField]
			public bool hiddenValue;

			[SerializeField]
			public bool allowInput = true;

			[SerializeField]
			public AttributeType type;

			[SerializeField]
			public bool has_many;

			[SerializeField]
			public bool clampMinimum;

			[SerializeField]
			public bool clampMaximum;

			[SerializeField]
			public float clampMinimumValue;

			[SerializeField]
			public float clampMaximumValue = 1f;

			private void _CoreInit()
			{
				if (name == "Input")
				{
					has_many = true;
				}
			}

			public Attribute(string _name)
			{
				name = _name;
				_CoreInit();
			}

			public Attribute(string _name, bool _hiddenValue)
			{
				name = _name;
				hiddenValue = _hiddenValue;
				_CoreInit();
			}

			public Attribute(string _name, float _value)
			{
				name = _name;
				value = _value;
				_CoreInit();
			}
		}

		private static Color PreviewBackground = Color.black;

		private static Color PreviewGrid = new Color(1f, 0.5f, 0.25f, 0.25f);

		private static Color PreviewWave = Color.white;

		private static float PreviewIntensity = 1.5f;

		private static int PreviewTextureHeightPadding = 3;

		public bool showPreviewTexture = true;

		public static int TOODEEP = 50;

		private float latestFrequency = 3f;

		private float latestDuration = 1f;

		[NonSerialized]
		public bool dirty;

		public List<Attribute> attributes = new List<Attribute>();

		public Rect visualPlacementRect = new Rect(10f, 10f, 100f, 100f);

		public abstract float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate);

        public virtual void InitializePlayback()
        {

        }

		public virtual void InitializeAttributes()
		{
		}

		public float amplitude(float frequency, float time, float duration, int depth, int sampleRate)
		{
			latestFrequency = frequency;
			latestDuration = duration;
			return OnAmplitude(frequency, time, duration, depth, sampleRate);
		}

		public Attribute attribute(string attributeName)
		{
			foreach (Attribute attribute in attributes)
			{
				if (attribute.name == attributeName)
				{
					return attribute;
				}
			}
			return null;
		}

		public bool hasAttribute(string attributeName)
		{
			foreach (Attribute attribute in attributes)
			{
				if (attribute.name == attributeName)
				{
					return true;
				}
			}
			return false;
		}

		public virtual void InitializeName()
		{
			base.name = GetType().Name;
		}

		public void renderToPreviewTexture(ref Texture2D previewTexture)
		{
			float frequency = latestFrequency;
			float num = latestDuration;
			for (int i = 0; i < previewTexture.width; i++)
			{
				float a = (amplitude(frequency, (num * (float)i - 1f) / (float)previewTexture.width, num, 1, 44100) + 1f) * (float)(previewTexture.height - 2 * PreviewTextureHeightPadding) / 2f + (float)PreviewTextureHeightPadding;
				float b = (amplitude(frequency, num * (float)i / (float)previewTexture.width, num, 1, 44100) + 1f) * (float)(previewTexture.height - 2 * PreviewTextureHeightPadding) / 2f + (float)PreviewTextureHeightPadding;
				float num2 = Mathf.Min(a, b);
				float num3 = Mathf.Max(a, b);
				for (int j = 0; j < previewTexture.height; j++)
				{
					previewTexture.SetPixel(i, j, PreviewBackground);
					Color color = (j == previewTexture.height / 2) ? PreviewGrid : PreviewBackground;
					float num4 = 0f;
					if (!(num2 < (float)j) || !((float)j < num3))
					{
						if ((float)j < num2)
						{
							num4 = num2 - (float)j;
						}
						else if ((float)j > num3)
						{
							num4 = (float)j - num3;
						}
					}
					float num5 = Mathf.Clamp(PreviewIntensity - num4, 0f, PreviewIntensity) / PreviewIntensity;
					previewTexture.SetPixel(i, j, new Color(color.r + num5 * PreviewWave.r * PreviewWave.a, color.g + num5 * PreviewWave.g * PreviewWave.a, color.b + num5 * PreviewWave.b * PreviewWave.a, 1f));
				}
			}
			previewTexture.Apply();
		}

        protected Attribute AddInput()
        {
            var input = new Attribute("Input", _hiddenValue: true);
            attributes.Add(input);
            return input;
        }

        protected Attribute AddAttribute(string attributeName, Func<AttributeBuilder, AttributeBuilder> builder)
        {
            var b = AttributeBuilder.Create(attributeName);
            var attribute = builder(b).Build();
            attributes.Add(attribute);
            return attribute;
        }


        protected Attribute AddAttribute(string attributeName) => AddAttribute(attributeName, b => b);
    }
}
