using System;
using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator.Module.Oscillator;
using UnityEngine;

public class Sin : BaseOscillator
{
    public static string MenuEntry()
    {
        return "Oscillator/SinX";
    }

    public override float OnAmplitude(float frequency, float time, float duration, int depth)
    {
        float phase = this.attribute("Phase").value;
        return Mathf.Sin(MathUtil.DegToRad((time * frequency + phase) * 360.0f));
    }
}