using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public GameObject Player;
    private Vector3 offset;
	void Start () {
        offset = transform.position - Player.transform.position;
        offset += new Vector3(0, 2, 0); 
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Player.transform.position + offset;
	}
}
