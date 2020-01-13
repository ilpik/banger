using UnityEngine;

public class SGexCosine : DarkArtsStudios.SoundGenerator.Module.BaseModule {
	public static string MenuEntry() { return "Examples/Oscillator/Cosine (C#)"; }
	
	public override float OnAmplitude(float frequency, float time, float duration, int depth)
	{
		return Mathf.Cos(Mathf.Deg2Rad * time * frequency * 360f);
	}

}