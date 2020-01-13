using UnityEngine;
using System.Collections;

public class SGSimplePlatformerCamera : MonoBehaviour {

	public GameObject player;

	void Update () {

		transform.position = new Vector3( player.transform.position.x, player.transform.position.y + 3f, -7f );
		transform.LookAt( player.transform.position );
	
	}
}
