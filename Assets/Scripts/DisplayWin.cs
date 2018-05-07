using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Team343.CharacterSystem
{
public class DisplayWin : MonoBehaviour {
	public Text winText;

	void Start()
	{
		winText.gameObject.SetActive (false); //turns UI text comp off
	}

	void OnTriggerEnter2D (Collider2D player)
	{
		if (player.gameObject.tag == "Player") {
			winText.gameObject.SetActive (true);
			Debug.Log (player.tag);
			StartCoroutine ("WaitForSec");
		}
	}

	IEnumerator WaitForSec()
	{
		yield return new WaitForSeconds (3);

		Application.LoadLevel (1);
	}
		
}
}
			