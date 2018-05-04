using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    public GameObject PlayerAnimator;
    [SerializeField]
    private Rigidbody2D PlayerRigidbody;
    [SerializeField] private float Speed = 5.0f;
    float Xscale;
    [Header("Raycasting")]
    public Transform RaycastPoint;
    public LayerMask RaycastMask;
    public float CastDistance = 1f;

    [Header("States")]
    public PlayerState CurrentState;

    public enum PlayerState
    {
        Default,
        Falling
    }
    private bool IsTransiioningState = true;

    private Vector2 currentSurfaceNormal;

    void Start ()
    {
        animator = PlayerAnimator.GetComponent<Animator>();
        Xscale = transform.localScale.x;
	}
	
	void Update ()
    {
        switch (CurrentState)
        {
            case PlayerState.Default:
                if (IsTransiioningState) // Note: Only handle transitions in UPDATE
                {
                    // On Enter Default
                    PlayerRigidbody.isKinematic = true;
                    PlayerRigidbody.velocity = Vector3.zero;
                    PlayerRigidbody.angularVelocity = 0f;
                    IsTransiioningState = false;
                }
                if(Input.GetKeyDown(KeyCode.Alpha1))
                {
                    TransitionStates(PlayerState.Falling);
                }
                DefaultUpdate();
                break;
            case PlayerState.Falling:
                if (IsTransiioningState)
                {
                    // On Enter Falling
                    PlayerRigidbody.velocity = Vector3.zero;
                    PlayerRigidbody.angularVelocity = 0f;
                    PlayerRigidbody.isKinematic = false;
                    IsTransiioningState = false;
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 0.1f);
                // Do nothing
                break;
            default:
                break;
        }
        
    }

    private void DefaultUpdate()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = 0;
        if (xAxis != 0)
        {
            animator.SetBool("While Moving", true);
        }
        else
        {
            animator.SetBool("While Moving", false);
        }
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else if (xAxis > 0)
            transform.localScale = new Vector2(1, transform.localScale.y);

        var dir = transform.TransformDirection(new Vector3(xAxis, yAxis));
        //Debug.Log(dir);
        Debug.DrawLine(transform.position, transform.position + dir, Color.blue, 0.5f);
        transform.position = transform.position + dir * Speed * Time.deltaTime;
        //transform.Translate(dir * Speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case PlayerState.Default:
                DefaultFixedUpdate();
                break;
            case PlayerState.Falling:
                // Check if Landing (Raycasts down)
                FallingFixedUpdate();
                break;
            default:
                break;
        }
    }

    private void TransitionStates(PlayerState nextState)
    {
        Debug.Log("TRANSITIONING TO: " + nextState);
        CurrentState = nextState;
        IsTransiioningState = true;
    }

    #region FixedUpdateStates
    private void DefaultFixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(RaycastPoint.position, -RaycastPoint.up, CastDistance, RaycastMask.value);
        if (hit.collider != null) // Check to see if Raycast hit something/
        {
            Debug.DrawLine(RaycastPoint.position, hit.point, Color.red, 0.5f);
            //Debug.Log("HIT " + hit.collider.gameObject);

            // ToDo: Check to see if hit ground.

            //Cache hit results.
            currentSurfaceNormal = hit.normal;

            // Rotate To match ground.
            transform.position = hit.point;
            var q = Quaternion.FromToRotation(transform.up, hit.normal);
            transform.rotation = Quaternion.Slerp(transform.rotation, q * transform.rotation, 0.2f);
            //transform.rotation = q * transform.rotation;
        }
    }

    private void FallingFixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(RaycastPoint.position, Vector2.down, CastDistance, RaycastMask.value);
        if (hit.collider != null) // Check to see if Raycast hit something/
        {
            Debug.DrawLine(RaycastPoint.position, hit.point, Color.red, 0.5f);
            //Debug.Log("HIT " + hit.collider.gameObject);

            // ToDo: Check to see if hit ground.

            //Cache hit results.
            currentSurfaceNormal = hit.normal;

            // Rotate To match ground.
            transform.position = hit.point;
            var q = Quaternion.FromToRotation(transform.up, hit.normal);
            transform.rotation = q * transform.rotation;// Quaternion.Slerp(transform.rotation, q * transform.rotation, 0.2f);
            TransitionStates(PlayerState.Default);
        }
    }
    #endregion
}
