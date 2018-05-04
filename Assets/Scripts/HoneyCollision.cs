using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyCollision : MonoBehaviour
{
    GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            player = other.gameObject;
            Debug.Log(player.tag);
            player.GetComponent<PlayerCharacterControl>().speed = 7f;
            Destroy(this);
        }
    }
}
