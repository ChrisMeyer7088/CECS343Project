using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisScript : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;
    private float speed = 7f;
    [Header("Walk Range")]
    public GameObject rightRange;
    public GameObject leftRange;
    private Vector2 walkInput;
    private bool walkingLeft = true;
    private float target;
    void Start ()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        rightRange.transform.parent = null;
        leftRange.transform.parent = null;
        target = leftRange.transform.position.x;
        Debug.Log("Transform position: " + this.transform.position.x + " target: " + target);
    }
	
	void Update ()
    {
        if (this.transform.position.x < target)
        {
            walkingLeft = false;
            target = rightRange.transform.position.x;
        }
        else if(this.transform.position.x > target)
        {
            walkingLeft = true;
            target = leftRange.transform.position.x;
        }
         
        if(walkingLeft) walkInput = new Vector2(-1, 0);
        else walkInput = new Vector2(1, 0);
        var dir = transform.TransformDirection(walkInput);
        if (dir.x != 0)
        {
            animator.SetBool("Is Moving", true);
        }
        else
        {
            animator.SetBool("Is Moving", false);
        }
        if (dir.x < 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (dir.x > 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        transform.position = transform.position + dir * speed * Time.deltaTime;
    }
}
