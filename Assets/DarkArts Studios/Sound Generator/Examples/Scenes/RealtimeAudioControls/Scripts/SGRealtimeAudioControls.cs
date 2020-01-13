using UnityEngine;
using System.Collections;
using System.Linq;

public class SGRealtimeAudioControls : MonoBehaviour {

	public float leftFrequency = 210f;
	public float rightFrequency = 220f;
	
	private float leftFrequencyPrevious = 0;
	private float rightFrequencyPrevious = 0;
	
	public float frequencyUpdateFPS = 30.0f;
	
	public DarkArtsStudios.SoundGenerator.Composition tonePrefab;
	private DarkArtsStudios.SoundGenerator.Module.Output toneOutput;

	public AudioSource leftAudioSource;
	public AudioSource rightAudioSource;
	
	private AudioClip leftAudioClip;
	private AudioClip rightAudioClip;

	#region Private Left/Right Regeration
	
	private void generateLeft()
	{
		toneOutput.attribute("Frequency").value = leftFrequency;
		toneOutput.Generate();
		leftAudioClip = toneOutput.audioClip;
		leftAudioSource.loop = false;
	}
	
	private void generateRight()
	{
		toneOutput.attribute("Frequency").value = rightFrequency;
		toneOutput.Generate();
		rightAudioClip = toneOutput.audioClip;
		rightAudioSource.loop = false;
	}
	
	#endregion
	
	#region Frequency Update Worker
	
	private void updateFrequencies()
	{
		if (leftFrequency != leftFrequencyPrevious)
		{
			generateLeft();
			leftFrequencyPrevious = leftFrequency;
		}
		if (rightFrequency != rightFrequencyPrevious)
		{
			generateRight();
			rightFrequencyPrevious = rightFrequency;
		}
	}
	
	#endregion
	
	#region Unity Hooks
	
	void Start () {

		// Retrieve toneOutput Module
		toneOutput = tonePrefab.modules.Find( x => x.GetType() == typeof(DarkArtsStudios.SoundGenerator.Module.Output)) as DarkArtsStudios.SoundGenerator.Module.Output;
		
		// Launch the frequency updater
		InvokeRepeating( "updateFrequencies", 0f, 1f/frequencyUpdateFPS);
		//leftAudioSource.Play();
		
	}
	
	void OnGUI()
	{
		// Left Frequency
		Rect leftSliderRect = new Rect( 10, 10, 20, Screen.height-20);
		Rect leftTextRect = new Rect( 30, 10, 100, 20);
		
		leftFrequency = GUI.VerticalSlider(leftSliderRect, leftFrequency, 300, 100);
		
		GUI.Label( leftTextRect, leftFrequency.ToString() + "hz" );
		
		// Right Frequency
		Rect rightSliderRect = new Rect( Screen.width - 20, 10, 20, Screen.height-20);
		Rect rightTextRect = new Rect( Screen.width - 60, 10, 100, 20);
		
		rightFrequency = GUI.VerticalSlider(rightSliderRect, rightFrequency, 300, 100);
		
			GUI.Label( rightTextRect, rightFrequency.ToString() + "hz" );
	}
	
	void Update () {
		
		if (!leftAudioSource.isPlaying)
			{
				leftAudioSource.loop = true;
				leftAudioSource.clip = leftAudioClip;
				leftAudioSource.Play();
			}
		if (!rightAudioSource.isPlaying)
			{
				rightAudioSource.loop = true;
				rightAudioSource.clip = rightAudioClip;
				rightAudioSource.Play();
			}
	}
	
	#endregion
	
}
