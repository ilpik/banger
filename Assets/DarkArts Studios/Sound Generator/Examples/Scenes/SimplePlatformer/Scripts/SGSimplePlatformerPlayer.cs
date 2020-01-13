using UnityEngine;
using System.Collections;

public class SGSimplePlatformerPlayer : MonoBehaviour {

	public float speed = 3f;
	public float jumpForce = 300f;

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

	void OnCollisionEnter(Collision collision)
	{
		audioSource.Play();
	}

	void Update () {
		transform.position = new Vector3( transform.position.x + Input.GetAxis("Horizontal") * speed * Time.deltaTime, transform.position.y, transform.position.z );
		if ( Input.GetButtonDown("Jump") )
			GetComponent<Rigidbody>().AddForce( Vector3.up * jumpForce );
	}

}
