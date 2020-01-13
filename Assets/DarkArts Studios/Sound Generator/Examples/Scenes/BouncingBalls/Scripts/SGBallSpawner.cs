using UnityEngine;
using System.Collections;

public class SGBallSpawner : MonoBehaviour {

	public GameObject ballPrefab = null;

	void Update () {
		if ( Input.GetMouseButtonDown(0) ) {
			GameObject ball = (GameObject)GameObject.Instantiate(ballPrefab);
			float y = (Input.mousePosition.y*4/Screen.height)+1;
			float x = (Input.mousePosition.x-Screen.width/2)*8/Screen.width;
			ball.transform.position = new Vector3(x,y,0);
			ball.name = "Ball";
		}
		if ( Input.GetMouseButtonDown(1) ) {
			foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject)))
			{
				if (gameObject.name == "Ball")
					GameObject.Destroy(gameObject);
			}
		}

	}
}
