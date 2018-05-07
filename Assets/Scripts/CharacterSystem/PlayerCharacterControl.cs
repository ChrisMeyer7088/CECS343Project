using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team343.CharacterSystem
{
	public class PlayerCharacterControl : InsectCharacterControlBase
	{
		private Vector3 startingPosition;
		private bool firstRun = true;
		Quaternion startingRotation;
		public GameObject player;

		protected override void DefaultUpdate()
		{
			if (firstRun) 
			{
				firstRun = false;
				startingPosition = transform.position;
				startingRotation = transform.rotation;
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)) 
			{
				Application.LoadLevel (SceneManager.GetActiveScene().buildIndex);
				GameObject mainPlayer = (GameObject)Instantiate(player, startingPosition, startingRotation);
				Destroy (this.gameObject);

			}
			if (Input.GetKeyDown(KeyCode.Alpha3)) 
			{
				Application.LoadLevel (1);
			}
			float xAxis = Input.GetAxis("Horizontal");
			float yAxis = 0;
			if (xAxis != 0)
			{
				animator.SetBool("While Moving", true);
			}
			else
			{
				animator.SetBool("While Moving", false);
			}
			if (xAxis < 0)
			{
				transform.localScale = new Vector2(-1, transform.localScale.y);
			}
			else if (xAxis > 0)
				transform.localScale = new Vector2(1, transform.localScale.y);

			var dir = transform.TransformDirection(new Vector3(xAxis, yAxis));
			//Debug.Log(dir);
			Debug.DrawLine(transform.position, transform.position + dir, Color.blue, 0.5f);
			Move(dir);

			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				TransitionStates(CharacterState.Falling);
			}
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.tag == "honey")
			{
				if(speed < 10)
					speed += 1;
				Destroy(other.gameObject);
			}
			if (other.gameObject.tag == "poison")
			{
				if(speed > 3)
					speed -= 1;
				Destroy(other.gameObject);
			}
		}
	}
}
