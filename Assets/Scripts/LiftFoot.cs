using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiftFoot : MonoBehaviour {

	//public GameObject ant;
	PlayerCharacterControl bugScript;
	float Multiplier = 50f;
	bool grounded = false;
	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shake_decay;
	public float shake_intensity;
	GameObject fallingObj;

	void OnCollisionEnter2D (Collision2D maze)
	{
		//checks if foot collided with the ground collider
		if (maze.gameObject.CompareTag ("MazeCollider")) {
			grounded = true;
			//shakes camera
			Camera.main.GetComponent<cameraShake>().Shake();
			//dropping ant from ceiling
			isFalling();
		}
	}

	//drops ant from ceiling
	void isFalling()
	{
		bugScript = GameObject.Find("Player").GetComponent<PlayerCharacterControl>();
		bugScript.CurrentState = InsectCharacterControlBase.CharacterState.Falling;
		bugScript.IsTransitoningState = true;

	//	Debug.Log (bugScript.CurrentState);
		Debug.Log (bugScript.IsTransitoningState);
	}
		

	void Update()
	{
		if (grounded == true) {
			//lifts foot up once it touches the ground
			transform.Translate(Vector2.up * Time.deltaTime * Multiplier);
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
	