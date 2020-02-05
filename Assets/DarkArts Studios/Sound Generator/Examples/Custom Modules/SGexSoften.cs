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

	public override double OnAmplitude(double time, int depth, int sampleRate)
	{
		double result = 0;
		if (attribute ("Input").generator)
		{
            double distance = attribute ("Distance").getAmplitudeOrValue(time, depth, sampleRate);
			result += attribute ("Input").generator.amplitude(time - distance,depth+1, sampleRate);
			result += attribute ("Input").generator.amplitude(time,depth+1, sampleRate)*2;
			result += attribute ("Input").generator.amplitude(time + distance,depth+1, sampleRate);
			result /= 4;
		}
		return result;
	}


}
