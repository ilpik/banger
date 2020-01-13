using UnityEngine;
using System.Collections;

public class SGSciFiDoorTrigger : MonoBehaviour {

	public GameObject door;
	public float upSpeed = 7.5f;
	public float downSpeed = 15f;

	static float doorDown = 0.5f;
	static float doorUp = 1.75f;

	private float doorDelta = 0f;

	private AudioSource audioSource;
	private AudioClip doorOpen;
	private AudioClip doorClose;

	IEnumerator Start() {
		
		audioSource = GetComponent<AudioSource>();
		audioSource.loop = false;

		while ( !door.GetComponent<DarkArtsStudios.SoundGenerator.Composition>().audioClips.ContainsKey("Output") ||
		       !GetComponent<DarkArtsStudios.SoundGenerator.Composition>().audioClips.ContainsKey("Output") )
		{
			yield return null;
		}
		
		doorOpen = door.GetComponent<DarkArtsStudios.SoundGenerator.Composition>().audioClips["Output"];
		doorClose = GetComponent<DarkArtsStudios.SoundGenerator.Composition>().audioClips["Output"];
	}
	


	void OnTriggerEnter( Collider other )
	{
		doorDelta = upSpeed;
		audioSource.PlayOneShot(doorOpen);
	}
	
	void OnTriggerExit( Collider other )
	{
		doorDelta = -downSpeed;
		audioSource.PlayOneShot(doorClose);
	}

	void Update() {
		if ( doorDelta == 0 )
			return;

		float doorPosition = door.transform.position.y + doorDelta*Time.deltaTime;

		if ( doorPosition < doorDown )
		{
			doorPosition = doorDown;
			doorDelta = 0;
		}

		if ( doorPosition > doorUp )
		{
			doorPosition = doorUp;
			doorDelta = 0;
		}

		door.transform.position = new Vector3( door.transform.position.x, doorPosition, door.transform.position.z );
	}
	
}
