using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour {

	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shake_decay;
	public float shake_intensity;

	//shakes camera and returns it to it's regular position after
	void Update (){
	//	Debug.Log ("CameraShake update being called from foot script");
		//decreases intensity gradually until shake_intensity<=0 and then stops shaking
		if (shake_intensity > 0){
			transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
			transform.rotation = new Quaternion(
				originRotation.x + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.y + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.z + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.w + Random.Range (-shake_intensity,shake_intensity) * .2f);
			shake_intensity -= shake_decay;
		}
	}

	//sets variable for the shake in Update()
	public void Shake(){
		originPosition = transform.position;
		originRotation = transform.rotation;
		//
		shake_intensity = .2f;
		//lower number = longer shake duration
		shake_decay = 0.01f;
	}

		

}
