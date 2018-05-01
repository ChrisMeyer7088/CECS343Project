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

    [Header("Raycasting")]
    public Transform sightEnd;
    public LayerMask RaycastMask;
    public float CastDistance = 6.0f;
    public bool Spotted = false;

    private MantisState currentState;
    void Start ()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        rightRange.transform.parent = null;
        leftRange.transform.parent = null;
        target = leftRange.transform.position.x;
    }

    public enum MantisState
    {
        Moving,
        Chasing
    }

    private void Update()
    {
        switch(currentState)
        {
            case MantisState.Moving:
                {
                    MovingUpdate();
                    break;
                }

            case MantisState.Chasing:
                {
                    ChasingUpdate();
                    break;
                }
            default:
                break;           
        }
        stateUpdate();
    }

    private void stateUpdate()
    {
        Debug.Log(sightEnd.position);
        Vector2 direction = (this.transform.position - sightEnd.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, CastDistance, RaycastMask.value);
        Debug.DrawLine(this.transform.position, direction * CastDistance, Color.green, .5f);
        if (hit.collider != null)
            currentState = MantisState.Chasing;
        else
            currentState = MantisState.Moving;
    }

    private void ChasingUpdate()
    {
        Vector2 direction = (sightEnd.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, CastDistance, RaycastMask.value);
        if (hit.collider != null)
        {
            GameObject go = hit.collider.gameObject;
            target = go.transform.position.x;

            if ((target - this.transform.position.x) > 0) walkInput = new Vector2(1, 0);
            else walkInput = new Vector2(-1, 0);
            var dir = transform.TransformDirection(walkInput);
            Move(dir);
        }
    }

    private void MovingUpdate()
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
        Move(dir);
    }

    private void Move(Vector3 dir)
    {
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
