using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//namespace Team343.CharacterSystem
//{
    public abstract class InsectCharacterControlBase : MonoBehaviour
    {
        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected Rigidbody2D rigidbody;
        [SerializeField]
        public float speed = 5f;
        float Xscale;

        [Header("Raycasting")]
        public Transform RaycastPoint;
        public LayerMask RaycastMask;
        public float CastDistance = 1.5f;

        [Header("States")]
        public CharacterState CurrentState;

        public enum CharacterState
        {
            Default,
            Falling
        }
        public bool IsTransitoningState = true;

        protected Vector2 currentSurfaceNormal;

        protected virtual void Start()
        {
            if (!animator) animator = GetComponent<Animator>();
            if (!animator) animator = GetComponentInChildren<Animator>();
            if (!rigidbody) rigidbody = GetComponent<Rigidbody2D>();
            Xscale = transform.localScale.x;
        }

        protected virtual void Update()
        {
            switch (CurrentState)
            {
                case CharacterState.Default:
                    if (IsTransitoningState) // Note: Only handle transitions in UPDATE
                    {
                        // On Enter Default
                        rigidbody.isKinematic = true;
                        rigidbody.velocity = Vector3.zero;
                        rigidbody.angularVelocity = 0f;
                        IsTransitoningState = false;
                    }
                    DefaultUpdate();
                    break;
                case CharacterState.Falling:
                    if (IsTransitoningState)
                    {
                        // On Enter Falling
                        rigidbody.velocity = Vector3.zero;
                        rigidbody.angularVelocity = 0f;
                        rigidbody.isKinematic = false;
                        IsTransitoningState = false;
                    }
                    // Rotate to default while falling.
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 0.1f);
                    break;
                default:
                    break;
            }
        }

        protected abstract void DefaultUpdate();

        protected virtual void Move(Vector3 direction)
        {
            // Determine If On wall or ceiling
            float dot = Vector3.Dot(Vector3.up, currentSurfaceNormal);
            //Debug.Log(dot);
            bool onCeiling = (dot <= -.12);

            bool onWall = (Mathf.Abs(dot) <= 0.7); //or 0.7
            if(onWall)
            {
                //Debug.Log(onWall);
            }          

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                 transform.localScale = new Vector2(-1, transform.localScale.y);
            else if(Input.GetKeyDown(KeyCode.RightArrow))
                 transform.localScale = new Vector2(1, transform.localScale.y);
            else if (direction.x > 0)
                transform.localScale = new Vector2(onCeiling ? -1 : 1, transform.localScale.y);
            else if(direction.x < 0 )
                transform.localScale = new Vector2(onCeiling? 1:-1, transform.localScale.y);
                
            transform.position = transform.position + direction * speed * Time.deltaTime;
        }

        protected virtual void FixedUpdate()
        {
            switch (CurrentState)
            {
                case CharacterState.Default:
                    DefaultFixedUpdate();
                    break;
                case CharacterState.Falling:
                    FallingFixedUpdate();
                    break;
                default:
                    break;
            }
        }

        protected virtual void TransitionStates(CharacterState nextState)
        {
            Debug.Log("TRANSITIONING TO: " + nextState);
            CurrentState = nextState;
            IsTransitoningState = true;
        }

        #region FixedUpdateStates

        protected virtual void DefaultFixedUpdate()
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

        protected virtual void FallingFixedUpdate()
        {
            // Check if Landing (Raycasts down)

            RaycastHit2D hit = Physics2D.Raycast(RaycastPoint.position, Vector2.down, CastDistance, RaycastMask.value);
            if (hit.collider != null) // Check to see if Raycast hit something/
            {
                Debug.DrawLine(RaycastPoint.position, hit.point, Color.red, 0.5f);
                //Debug.Log("HIT " + hit.collider.gameObject);

                // ToDo: Make sure it's GROUND we're landing on.

                //Cache hit results.
                currentSurfaceNormal = hit.normal;

                // Rotate To match ground.
                transform.position = hit.point;
                var q = Quaternion.FromToRotation(transform.up, hit.normal);
                transform.rotation = q * transform.rotation;// Quaternion.Slerp(transform.rotation, q * transform.rotation, 0.2f);
                TransitionStates(CharacterState.Default);
            }
        }
        #endregion

    }
//}
