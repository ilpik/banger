using UnityEngine;
using System.Collections;

public class SGSciFiPlayer : MonoBehaviour {

	public float speed = 25f;

	void Update () {

		transform.RotateAround( Vector3.zero, Vector3.up, Time.deltaTime*speed );
	
	}
}
