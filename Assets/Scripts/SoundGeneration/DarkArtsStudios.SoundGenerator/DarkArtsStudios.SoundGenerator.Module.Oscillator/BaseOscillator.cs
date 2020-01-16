using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public abstract class BaseOscillator : BaseModule
	{
		public static List<Type> oscillators;

		public override void InitializeAttributes()
		{
			Attribute attribute = new Attribute("Phase");
			attribute.clampMaximum = true;
			attribute.clampMaximumValue = 1f;
			attribute.clampMinimum = true;
			attribute.clampMinimumValue = 0f;
			attribute.type = Attribute.AttributeType.SLIDER;
			attributes.Add(attribute);
		}

		static BaseOscillator()
		{
			oscillators = new List<Type>();
			foreach (Type item in from type in AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly assembly) => assembly.GetTypes())
				where type.IsSubclassOf(typeof(BaseOscillator))
				select type)
			{
				oscillators.Add(item);
			}
		}
	}
}
