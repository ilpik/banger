using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SGSciFiCamera : MonoBehaviour {

	public float rotationSpeed = -10f;

	void Update () {
		transform.LookAt(Vector3.zero,Vector3.up);
		if (Application.isPlaying)
			transform.RotateAround( Vector3.zero, Vector3.up, rotationSpeed*Time.deltaTime );
	}
}
