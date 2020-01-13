using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SGSciFiSignal : MonoBehaviour {

	public Color signalColorDim;
	public Color signalColorBright;

	public List<Light> lights = new List<Light>();

	private AudioSource audioSource;
	
	IEnumerator Start() {
		
		audioSource = GetComponent<AudioSource>();
		audioSource.loop = true;
		
		while ( !GetComponent<DarkArtsStudios.SoundGenerator.Composition>().audioClips.ContainsKey("Output") )
		{
			yield return null;
		}
		
		audioSource.clip = GetComponent<DarkArtsStudios.SoundGenerator.Composition>().audioClips["Output"];
		audioSource.Play();
	}
	
	void Update () {
		gameObject.GetComponent<Renderer>().material.color = Color.Lerp(
			signalColorDim, signalColorBright,
			Mathf.PingPong( Time.time*2f, 1.0f ) );
		foreach ( Light light in lights )
			light.color = gameObject.GetComponent<Renderer>().material.color;
	}
}
