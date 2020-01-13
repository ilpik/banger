using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SGMonkeyBusiness : MonoBehaviour {
	
	public DarkArtsStudios.SoundGenerator.Composition MonkeyMixer;
	
	private DarkArtsStudios.SoundGenerator.Module.AudioClip audioClipModule;
	private DarkArtsStudios.SoundGenerator.Module.Output outputModule;

	public List<AudioClip> sounds;

	void Start () {
		
		foreach( DarkArtsStudios.SoundGenerator.Module.BaseModule module in MonkeyMixer.modules)
		{
			if ( module.GetType() == typeof( DarkArtsStudios.SoundGenerator.Module.AudioClip ) )
				audioClipModule = module as DarkArtsStudios.SoundGenerator.Module.AudioClip;
			if ( module.GetType() == typeof( DarkArtsStudios.SoundGenerator.Module.Output ) )
				outputModule = module as DarkArtsStudios.SoundGenerator.Module.Output;
		}
		
		if ( audioClipModule == null )
			Debug.LogError("No AudioClip Module found!");
		
		GetComponent<AudioSource>().loop = false;
		
	}
	
	void OnGUI() {
		GUILayout.BeginVertical();
		{
			GUILayout.Space(16);
			
			foreach( AudioClip sound in sounds )
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(16);
					
					GUILayout.Label( sound.name );

					GUILayout.Space(16);

					if ( GUILayout.Button( "Original" ) )
						GetComponent<AudioSource>().PlayOneShot( sound );

					GUILayout.Space(16);
					
					if (GUILayout.Button( "Monkey" ) )
					{
						audioClipModule.audioClip = sound;
						audioClipModule.audioData = null;
						outputModule.Generate();
						GetComponent<AudioSource>().PlayOneShot( outputModule.audioClip );

					}
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndVertical();
/*
		Rect labelPlacement = new Rect( 10, 10, Screen.width - 20, 20 );
		
		// No Microphones plugged in
		if ( Microphone.devices.Length < 1 )
		{
			GUI.Label( labelPlacement, "Please plug a microphone into your computer");
			microphoneDevice = null;
			return;
		}
		
		// Select which microphone to use
		if (microphoneDevice == null)
		{
			GUI.Label( labelPlacement, "Please select a microphone to use:");
			int deviceEnumerator = 1;
			foreach (string deviceName in Microphone.devices) {
				Rect deviceButtonPlacement = new Rect(labelPlacement);
				deviceButtonPlacement.y = deviceEnumerator * labelPlacement.height*1.5f + labelPlacement.y;
				if (GUI.Button(deviceButtonPlacement,deviceName))
				{
					microphoneDevice = deviceName;
					break;
				}
			}
			return;	
		}
		
		if ( GUI.Button( labelPlacement, "Stop" ) )
		{
			microphoneDevice = null;
		}
*/		
	}
	
	void Update () {
/*
		if ( microphoneDevice == null )
			return;
		
		if ( !Microphone.IsRecording( microphoneDevice ) )
		{
			int recordingEnum = ( currentRecordingAudioClip + (int)AudioClipOffset.RECORDING ) & 3;
			int generatingEnum = ( currentRecordingAudioClip + (int)AudioClipOffset.GENERATING ) & 3;
			int playingEnum = ( currentRecordingAudioClip + (int)AudioClipOffset.PLAYING ) & 3;
			
			recordedAudioClips[ recordingEnum ] = Microphone.Start( microphoneDevice, false, soundLength, 22050);
			
			if ( recordedAudioClips[ playingEnum ] )
			{
				audio.PlayOneShot( recordedAudioClips[ playingEnum ] );
			}
			
			currentRecordingAudioClip += 1;
			
			audioClipModule.audioClip = recordedAudioClips[ generatingEnum ];
			audioClipModule.audioData = null; // reset cached data
			outputModule.attribute("Duration").value = soundLength;
			outputModule.sampleRate = 22050;
			
			outputModule.Generate();
			
			recordedAudioClips[ generatingEnum ] = outputModule.audioClip;
			
		}
*/		
	}
}
