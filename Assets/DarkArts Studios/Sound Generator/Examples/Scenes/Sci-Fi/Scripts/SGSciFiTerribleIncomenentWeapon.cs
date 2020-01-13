using UnityEngine;
using System.Collections;

public class SGSciFiTerribleIncomenentWeapon : MonoBehaviour {

	public GameObject player;
	public float timeBetweenShots = 1.0f;
	public float weaponMinRange = 0.5f;
	public float weaponMaxRange = 4.5f;
	public GameObject laserPrefab;
	public float laserLength=2.5f;

	private AudioSource audioSource;
	private float lastShot = 0;

	private GameObject laser = null;

	IEnumerator Start() {
		
		audioSource = GetComponent<AudioSource>();
		audioSource.loop = false;
		
		while ( !GetComponent<DarkArtsStudios.SoundGenerator.Composition>().audioClips.ContainsKey("Weapon") )
		{
			yield return null;
		}
		audioSource.clip = GetComponent<DarkArtsStudios.SoundGenerator.Composition>().audioClips["Weapon"];
	}

	void Shoot() {
		laser = (GameObject)Instantiate( laserPrefab );
		Vector3 laserTarget = transform.position + ( player.transform.position - transform.position ) * 0.75f;
		laserTarget.y = 0;
		Vector3 laserDirection = (laserTarget - transform.position).normalized;
		laser.GetComponent<SGSciFiLaser>().direction = laserDirection;
		laser.transform.position = transform.position;
		LineRenderer laserLine = laser.GetComponent<LineRenderer>();
		laserLine.SetPosition(0,Vector3.zero);
		laserLine.SetPosition(1, laserDirection * laserLength);
		audioSource.Play();
		lastShot = Time.time;
	}

	void Update () {

		if (Time.time - lastShot > timeBetweenShots &&
		    (transform.position - player.transform.position).magnitude < weaponMaxRange &&
			(transform.position - player.transform.position).magnitude > weaponMinRange )
				Shoot ();
	
	}
}
