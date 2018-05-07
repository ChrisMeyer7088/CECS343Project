using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterCollision : MonoBehaviour
{
    public Animator anim;
    public AnimationClip clip;
    public float deathTime;
	private GameObject targetObject;

    void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.layer != 0) {
			targetObject = collision.gameObject;
			if (targetObject.tag != "Player") {
				while (targetObject.transform.parent.gameObject != null) {
					targetObject = targetObject.transform.parent.gameObject;
					if (targetObject.transform.parent == null)
						break;
				}
			}
			Application.LoadLevel (SceneManager.GetActiveScene().buildIndex);;
		}
    }

    private Vector3 startPos;

    /*private void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.Log("Error: Did not find animation!");
        }
        else
        {
            //Debug.Log("Got animation");
        }
        startPos = transform.position;
        UpdateAnimClipTimes();
    }*/

//    public void UpdateAnimClipTimes()
//    {
//        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
//        foreach (AnimationClip clip in clips)
//        {
//            switch (clip.name)
//            {
//                case "death":
//                    deathTime = clip.length;
//                    break;
//            }
//        }
//    }
}
