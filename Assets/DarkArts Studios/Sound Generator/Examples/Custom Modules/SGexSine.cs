using System;
using UnityEngine;

public class SGexSine : DarkArtsStudios.SoundGenerator.Module.BaseModule
{
    public static string MenuEntry() { return "Examples/Oscillator/Sine (C#)"; }

    public Attribute frequency;

    public override void InitializeAttributes()
    {
        base.InitializeAttributes();
        frequency = AddFrequency();
    }

    public override double OnAmplitude(double time, int depth, int sampleRate)
    {
        return Math.Sin(Mathf.Deg2Rad * time * frequency.getAmplitudeOrValue(time, depth, sampleRate) * 360f);
    }

}