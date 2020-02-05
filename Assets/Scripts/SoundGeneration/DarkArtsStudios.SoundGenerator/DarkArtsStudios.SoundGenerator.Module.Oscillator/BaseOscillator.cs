using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module.Oscillator
{
	public abstract class BaseOscillator : DAAudioFilter
	{
		public static List<Type> oscillators;

        public Attribute frequency;

        public Attribute phase;

		public override void InitializeAttributes()
		{
            base.InitializeAttributes();
            phase = AddAttribute("Phase", b => b.Slider(0, 2 * Mathf.PI));
            frequency = AddFrequency();
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
