using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOnClick : MonoBehaviour {
	public GameObject loadingImage;

	public void loadScene(int level)
	{
		//loading picture
		loadingImage.SetActive (true);
		//used to load scenes in Unity
		//level parameter = index in build settings of level we want to load
		Application.LoadLevel(level);
	}
}
