using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyCollision : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerCharacterControl>().speed = 7f;
            Destroy(gameObject);
        }
    }
}
