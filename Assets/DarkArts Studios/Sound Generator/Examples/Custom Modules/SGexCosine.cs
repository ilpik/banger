using System;
using UnityEngine;

public class SGexCosine : DarkArtsStudios.SoundGenerator.Module.BaseModule {
	public static string MenuEntry() { return "Examples/Oscillator/Cosine (C#)"; }

    public Attribute frequency;

    public override void InitializeAttributes()
    {
        base.InitializeAttributes();
        frequency = AddFrequency();
    }

    public override double OnAmplitude(double time, int depth, int sampleRate)
	{
		return Math.Cos(Mathf.Deg2Rad * time * frequency.getAmplitudeOrValue(time, depth, sampleRate) * 360f);
	}

}