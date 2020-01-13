using UnityEngine;
using System.Collections;

public class SGSciFiLaser : MonoBehaviour {

	public Vector3 direction = Vector3.up;
	public float speed = 20f;

	void Update () {
		transform.Translate( direction * Time.deltaTime * speed );
		if (transform.position.y < 0)
			Destroy (gameObject);
	}
}
