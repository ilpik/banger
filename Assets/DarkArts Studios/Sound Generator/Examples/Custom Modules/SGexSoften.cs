using UnityEngine;
using System.Collections;

public class SGexSoften : DarkArtsStudios.SoundGenerator.Module.BaseModule {
	public static string MenuEntry() { return "Examples/Filter/Soften (C#)"; }

	public override void InitializeAttributes()
	{
		attributes.Add(new Attribute("Input", true));
		
		Attribute distance = new Attribute("Distance",0.1f);
		distance.type = Attribute.AttributeType.SLIDER;
		distance.clampMinimum = true;
		distance.clampMinimumValue = 0.01f;
		distance.clampMaximum = true;
		distance.clampMaximumValue = 0.999f;
		attributes.Add(distance);
	}

	public override float OnAmplitude(float frequency, float time, float duration, int depth)
	{
		float result = 0;
		if (attribute ("Input").generator)
		{
			float distance = attribute ("Distance").value;
			result += attribute ("Input").generator.amplitude(frequency,time - distance,duration,depth+1);
			result += attribute ("Input").generator.amplitude(frequency,time           ,duration,depth+1)*2;
			result += attribute ("Input").generator.amplitude(frequency,time + distance,duration,depth+1);
			result /= 4;
		}
		return result;
	}


}
