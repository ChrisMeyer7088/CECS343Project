using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team343.CharacterSystem
{
public class SpawnManager : MonoBehaviour {

	public GameObject spawnObject;                // The enemy prefab to be spawned.
	public float spawnTime = 3f;            // How long between each spawn. 3 seconds in this case
	public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
	public int probability = 10000;


	void Start ()
	{
		//	Debug.Log (spawnObject.tag);
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
			if (spawnObject.CompareTag ("randSpawn")) {
				InvokeRepeating ("Spawn", spawnTime, spawnTime);
			}
	}

	void OnTriggerEnter2D (Collider2D player)
	{
		Debug.Log ("Spawning should start now");
		if (spawnObject.CompareTag ("oneTimeSpawn"))
			Spawn ();
	}

	void Update()
	{
			if (spawnObject.CompareTag ("probSpawn")) {
				float rand = Random.Range (0, probability);
				if (rand == 1)
					Spawn ();
			} 
	}

	public void Spawn ()
	{		
		//Find a random index between zero and one less than the number of spawn points.
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);

		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		Instantiate (spawnObject, spawnPoints[spawnPointIndex].position, spawnObject.transform.rotation);

	}
}
}
