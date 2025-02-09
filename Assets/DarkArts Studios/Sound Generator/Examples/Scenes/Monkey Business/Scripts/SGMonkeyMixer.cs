﻿using System;
using System.Collections;
using Random = UnityEngine.Random;

public class SGMonkeyMixer : DarkArtsStudios.SoundGenerator.Module.BaseModule {
	public static string MenuEntry() { return "Examples/Scenes/Monkey Business/Monkey Mixer"; }

	private int currentMonkey;
	private bool speaking;
	private double monkeyStartTime;
	private double humanVolume;
	private float monkeyFrequencyMultiplier;

	private float accumulation = 0.9f;
	
	public override void InitializeAttributes()
	{
		attributes.Add(new Attribute( "Human", true));
		for (int monkey=1; monkey <= 5; monkey++ )
			attributes.Add(new Attribute( string.Format( "Monkey-{0}", monkey ), true));

		attributes.Add( new Attribute("Threshold",false) );

		speaking = false;
		humanVolume = 0;
		monkeyStartTime = 0;
		currentMonkey = 1;
		monkeyFrequencyMultiplier = 1;
	}
	
	public override double OnAmplitude(double time, int depth, int sampleRate)
	{
		Attribute human = attribute ( "Human" );
		if ( human == null || human.generator == null ) return 0;

		double humanAmplitude = human.generator.amplitude(time, depth + 1, sampleRate);

		humanVolume = humanVolume*accumulation + Math.Abs( humanAmplitude );

		float threshold = attribute("Threshold").value;

		if ( speaking )
		{
			if ( humanVolume < threshold )
				speaking = false;
			
			Attribute monkey = attribute ( string.Format( "Monkey-{0}", currentMonkey ) );
			if ( monkey == null || monkey.generator == null ) return 0;
			
			return monkey.generator.amplitude(time - monkeyStartTime, depth + 1, sampleRate);// + humanAmplitude/40;
		}
		else
		{
			if ( humanVolume >= threshold )
			{
				monkeyStartTime = time;
				speaking = true;
				monkeyFrequencyMultiplier = Random.Range( 0.9f, 1.1f );
				currentMonkey = Random.Range(1,6);
				//Debug.Log("Start speak!");
			}
		}

		return 0;

	}
	
	
}
