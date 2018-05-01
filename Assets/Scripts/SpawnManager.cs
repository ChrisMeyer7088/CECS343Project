using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	//public PlayerHealth playerHealth;       // Reference to the player's heatlh.
	public GameObject spawnObject;                // The enemy prefab to be spawned.
	public float spawnTime = 3f;            // How long between each spawn. 3 seconds in this case
	public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.


	/*void Start ()
	{
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		//InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}*/

	void Update()
	{
		float rand = Random.Range (0, 1000);
		if (rand == 1) {
			spawnTime = rand;
			Spawn ();
		}
			
	}


	void Spawn ()
	{
		// If the player has no health left...
	/*	if(playerHealth.currentHealth <= 0f)
		{
			// ... exit the function.
			return;
		}*/
			
		//Find a random index between zero and one less than the number of spawn points.
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		//ally.transform.rotation = Quaternion.Euler( 0, Random.Range(0, 360), 0 );

		//transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
		/*flip = this.gameObject.transform.localScale.x;
		this.gameObject.transform.localScale.x = -flip; have to find type of variable*/

		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		Instantiate (spawnObject, spawnPoints[spawnPointIndex].position, spawnObject.transform.rotation);

	}
}
