using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision : MonoBehaviour {

    public Animator anim;
    public AnimationClip clip;
    public float deathTime;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "puddle")
        {
            Kill(gameObject);
        }
    }
    private Vector3 startPos;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if(anim == null)
        {
            Debug.Log("Error: Did not find animation!");
        } else
        {
            //Debug.Log("Got animation");
        }
        startPos = transform.position;
        UpdateAnimClipTimes();
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "death":
                    deathTime = clip.length;
                    break;
            }
        }
    }

    public void Kill(GameObject gameObject)
    {
        anim.Play("death");
        Destroy(gameObject, deathTime);
        //transform.position = startPos;
    }
}
