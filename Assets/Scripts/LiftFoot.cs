using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiftFoot : MonoBehaviour {

	float yourMultiplier = 50f;
	bool grounded = false;
	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shake_decay;
	public float shake_intensity;

	void OnCollisionEnter2D (Collision2D maze)
	{
		//checks if foot collided with the ground collider
		if (maze.gameObject.CompareTag ("MazeCollider")) {
			grounded = true;
			//Shake function from cameraShake, also calls cameraShake update()
			Camera.main.GetComponent<cameraShake>().Shake();
		}
	}

	void Update()
	{
		if (grounded == true) {
			//lifts foot up once it touches the ground
			transform.Translate(Vector2.up * Time.deltaTime * yourMultiplier);
		//	StartCoroutine ("WaitForSec");

		}
		//gets position of foot
		Vector2 pos = transform.position;
		//destroys foot once it's off the screen
		if(pos.y>45)
			Destroy(gameObject);
		
	}

	/*IEnumerator WaitForSec()
	{
		yield return new WaitForSeconds (2);
		//lifts foot up
		transform.Translate(Vector2.up * Time.deltaTime * yourMultiplier);
	}*/
		
}
			