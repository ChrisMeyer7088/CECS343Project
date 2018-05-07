using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Team343.CharacterSystem
{
public class Tutorial : MonoBehaviour {
	public Text displayText;

	void Start()
	{
		displayText.gameObject.SetActive (false); //turns UI text comp off
	}

	void OnTriggerEnter2D (Collider2D player)
	{
		if (player.gameObject.tag == "Player") {
			displayText.gameObject.SetActive (true);
			Debug.Log (player.tag);
			StartCoroutine ("WaitForSec");
		}
	}

	IEnumerator WaitForSec()
	{
		yield return new WaitForSeconds (10);

			Destroy (displayText);
	}
		
}
}
			