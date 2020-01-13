using UnityEngine;
using System.Collections;

public class SGBall : MonoBehaviour {

	public GameObject ballCompositionPrefab = null;

	private AudioClip bounceSound = null;

	void Start () {

		float ballSize = Random.value;
		float ballPitch = (1f-ballSize)+1f;
		transform.localScale = Vector3.one * (0.1f+ballSize);

		DarkArtsStudios.SoundGenerator.Composition bounceComposition = ballCompositionPrefab.GetComponent<DarkArtsStudios.SoundGenerator.Composition>();
		DarkArtsStudios.SoundGenerator.Module.Output output = null;
		foreach( DarkArtsStudios.SoundGenerator.Module.BaseModule module in bounceComposition.modules)
		{
			if ( module.GetType() == typeof(DarkArtsStudios.SoundGenerator.Module.Output) )
			{
				output = module as DarkArtsStudios.SoundGenerator.Module.Output;
				break;
			}
		}
		if (output)
		{
			output.attribute("Frequency").value = Mathf.Pow( ballPitch,2) * 100;
			output.Generate();
			bounceSound = output.audioClip;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		GetComponent<AudioSource>().PlayOneShot(bounceSound);
	}

}
