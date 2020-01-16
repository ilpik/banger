using UnityEngine;

public class SGexSine : DarkArtsStudios.SoundGenerator.Module.BaseModule
{
    public static string MenuEntry() { return "Examples/Oscillator/Sine (C#)"; }

    public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
    {
        return Mathf.Sin(Mathf.Deg2Rad * time * frequency * 360f);
    }

}