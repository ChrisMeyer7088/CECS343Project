using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Team343.CharacterSystem
{
    public class DisplayWin : MonoBehaviour
    {
        public Text winText;

        void Start()
        {
            winText.gameObject.SetActive(false); //turns UI text comp off
        }

        void OnTriggerEnter2D(Collider2D player)
        {
            Debug.Log(player.gameObject.tag);
            if (player.gameObject.tag == "Player")
            {
                Debug.Log ("Collision Detected");
                winText.gameObject.SetActive(true);
                StartCoroutine("WaitForSec");
            }
        }

        IEnumerator WaitForSec()
        {
            yield return new WaitForSeconds(4);

            Application.LoadLevel(1);
        }

    }
}
			