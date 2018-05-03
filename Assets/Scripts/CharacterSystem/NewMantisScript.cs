using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMantisScript : MonoBehaviour {

    [SerializeField]
    protected Animator animator;
    private float speed = 6f;
    public LayerMask layerMask;
    public enum State
    {
        Patrolling,
        Chase,
        Investigating,
        Attacking
    }
    public State currentState = State.Patrolling;

    [Header("Patrolling")]
    public GameObject rightRange;
    public GameObject leftRange;
    private Vector2 walkInput;
    private bool walkingLeft = true;
    private float target;

    [Header("Investigating")]
    private Vector2 InvestigationSpot;
    private float timer = 0;
    public float investigateWait;

    [Header("Chasing")]
    public GameObject targetObject;

    [Header("Attacking")]
    public float killSpeed = 30;

    [Header("Sight")]
    public float sightDist = 10;
    private float movingSightDist = 8;
    public float heightModifier;

    void Start ()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        rightRange.transform.parent = null;
        leftRange.transform.parent = null;
        target = leftRange.transform.position.x;
        heightModifier = 1f;
    }

	void Update ()
    {
        switch (currentState)
        {
            case State.Patrolling:
                {
                    timer = 0;
                    MovingUpdate();
                    break;
                }

            case State.Chase:
                {
                    Chase();
                    break;
                }
            case State.Investigating:
                {
                    Investigate();
                    break;
                }
            case State.Attacking:
                {
                    Attacking(targetObject);
                    break;
                }
            default:
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        if(collision.gameObject.layer == 8)
        {
            animator.SetTrigger("Attack");
            currentState = State.Attacking;
            targetObject = collision.gameObject;
        }
    }

    private void Attacking(GameObject obj)
    {
        timer += .1f;
        if (timer >= killSpeed)
        {
            currentState = State.Patrolling;
            obj.active = false;
        }
    }

    private void Chase()
    {
        Vector3 offset = this.transform.position - targetObject.transform.position;
        if (offset.x < 10 && offset.y < 4)
        {
            if (offset.x > 0)
                Move(new Vector3(-1, 0, 0));
            else
                Move(new Vector3(1, 0, 0));
        }
        else
            currentState = State.Investigating;
    }

    private void Investigate()
    {
        timer += Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(transform.localScale.x, 0, 0) * sightDist, sightDist, layerMask.value);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + Vector3.down * heightModifier, new Vector3(transform.localScale.x, 0, 0) * sightDist, sightDist, layerMask.value);
        Debug.DrawRay(transform.position + Vector3.down * heightModifier, new Vector3(transform.localScale.x, 0, 0) * sightDist, Color.green);
        Debug.DrawRay(transform.position,new Vector3(transform.localScale.x,0,0) * sightDist, Color.green);
        if (hit.collider != null)
        {
            currentState = State.Chase;
            targetObject = hit.collider.gameObject;
            Debug.Log("I see you");
        }
        else if(hit2.collider != null)
        {
            currentState = State.Chase;
            targetObject = hit2.collider.gameObject;
            Debug.Log("I see you");
        }
        Move(new Vector3(0,0,0));
        if(timer >= investigateWait)
        {
            currentState = State.Patrolling;
        }
    }

    private void MovingUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(transform.localScale.x, 0, 0) * movingSightDist, movingSightDist, layerMask.value);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + Vector3.down * heightModifier, new Vector3(transform.localScale.x, 0, 0) * movingSightDist, movingSightDist, layerMask.value);
        Debug.DrawRay(transform.position + Vector3.down * heightModifier, new Vector3(transform.localScale.x, 0, 0) * movingSightDist, Color.green);
        Debug.DrawRay(transform.position, new Vector3(transform.localScale.x, 0, 0) * movingSightDist, Color.green);
        if (hit.collider != null)
        {
            currentState = State.Investigating;
        }
        else if(hit2.collider != null)
        {
            currentState = State.Investigating;
        }
        else
        {
            if (this.transform.position.x < target)
            {
                walkingLeft = false;
                target = rightRange.transform.position.x;
            }
            else if (this.transform.position.x > target)
            {
                walkingLeft = true;
                target = leftRange.transform.position.x;
            }

            if (walkingLeft) walkInput = new Vector2(-1, 0);
            else walkInput = new Vector2(1, 0);
            var dir = transform.TransformDirection(walkInput);
            Move(dir);
        }
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
