using UnityEngine;
using System.Collections;

public class SGSimplePlatformerCoin : MonoBehaviour {

	public float rotationSpeed=1f;

	private bool alive = true;

	private AudioSource audioSource;

	IEnumerator Start() {

		audioSource = GetComponent<AudioSource>();
		audioSource.loop = false;

		while ( !GetComponent<DarkArtsStudios.SoundGenerator.Composition>().audioClips.ContainsKey("Output") )
		{
			yield return null;
		}
		audioSource.clip = GetComponent<DarkArtsStudios.SoundGenerator.Composition>().audioClips["Output"];
	}

	void Update () {
		transform.Rotate(0,0,rotationSpeed*Time.deltaTime);
		if (!alive)
		{
			if (!audioSource.isPlaying)
				GameObject.Destroy( gameObject );
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if ( alive )
		{
			audioSource.Play();
			GetComponent<Renderer>().enabled = false;
			alive = false;
		}
	}
}
