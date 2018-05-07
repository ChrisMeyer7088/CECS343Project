using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public GameObject Player;
    private Vector3 offset;
	private Vector3 originalPosition;
	void Start () {
		originalPosition = this.transform.position;
        offset = transform.position - Player.transform.position;
        offset += new Vector3(0, 2, 0); 
	}
	
	// Update is called once per frame
	void Update () {
		if (!Player) 
		{
			Player = GameObject.FindGameObjectWithTag ("Player");
			this.transform.position = originalPosition;
		}
			
        transform.position = Player.transform.position + offset;
	}
}
