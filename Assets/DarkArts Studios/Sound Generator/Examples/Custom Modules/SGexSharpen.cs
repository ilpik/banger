using UnityEngine;
using System.Collections;

public class SGexSharpen : DarkArtsStudios.SoundGenerator.Module.BaseModule
{
    public static string MenuEntry() { return "Examples/Filter/Sharpen (C#)"; }

    public override void InitializeAttributes()
    {
        attributes.Add(new Attribute("Input", true));

        Attribute distance = new Attribute("Distance", 0.1f);
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
        if (attribute("Input").generator)
        {
            float distance = attribute("Distance").value;
            double local = attribute("Input").generator.amplitude(time, depth + 1, sampleRate) * 2;
            double surround = attribute("Input").generator.amplitude(time - distance, depth + 1, sampleRate);
            surround += attribute("Input").generator.amplitude(time + distance, depth + 1, sampleRate);
            surround /= 4;
            result += local + (local - surround);
        }
        return result;
    }


}