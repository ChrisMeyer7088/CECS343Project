using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Team343.CharacterSystem
{

    public class SimpleZombieMove : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        private Rigidbody2D rigidbody;
        public float speed = 10f;
        public LayerMask layerMask;
        public State currentState;
        public SurfaceState currentSurfaceState;
        public bool IsTransitoningState = true;
        public bool onWall;
        [Header("Walking")]
        public Vector2 walking;

        [Header("Investigating")]
        private Vector2 InvestigationSpot;
        private float timer = 0;
        public float investigateWait;

        [Header("Chasing")]
        public GameObject targetObject;

        [Header("Attacking")]
        public float killSpeed = 30;

        [Header("Sight")]
        public float sightDist = 20;
        public float movingSightDist = 15;
        public float heightModifier;
        private Vector3 rayCastBegin;

        [Header("Raycasting")]
        public Transform RaycastPoint;
        public LayerMask RaycastMask;
        public float CastDistance = 1.5f;

        protected Vector2 currentSurfaceNormal;

        public enum SurfaceState
        {
            Default,
            Falling
        }

        public enum State
        {
            Patrolling,
            Chase,
            Investigating,
            Attacking
        }

        public void Start()
        {
            if (!animator) animator = GetComponent<Animator>();
            if (!animator) animator = GetComponentInChildren<Animator>();
            if (!rigidbody) rigidbody = GetComponent<Rigidbody2D>();
            walking = startVector();
            currentState = State.Patrolling;
            heightModifier = 1f;
            rayCastBegin = new Vector2(transform.position.x, transform.position.y);
        }

        private void Update()
        {
            switch (currentSurfaceState)
            {
                case SurfaceState.Default:
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
                case SurfaceState.Falling:
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

        void DefaultUpdate()
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
            if (collision.gameObject.layer == 8)
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

        private void MovingUpdate()
        {
            RaycastHit2D hit = Physics2D.Raycast(rayCastBegin, new Vector3(transform.localScale.x, 0, 0) * movingSightDist, movingSightDist, layerMask.value);
            RaycastHit2D hit2 = Physics2D.Raycast(rayCastBegin + Vector3.down * heightModifier, new Vector3(transform.localScale.x, 0, 0) * movingSightDist, movingSightDist, layerMask.value);
            Debug.DrawRay(rayCastBegin + Vector3.down * heightModifier, new Vector3(transform.localScale.x, 0, 0) * movingSightDist, Color.green);
            Debug.DrawRay(rayCastBegin, new Vector3(transform.localScale.x, 0, 0) * movingSightDist, Color.green);
            if (hit.collider != null)
            {
                currentState = State.Investigating;
            }
            else if (hit2.collider != null)
            {
                currentState = State.Investigating;
            }
            else
            {

                var dir = transform.TransformDirection(walking);
                Move(dir);
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
            RaycastHit2D hit = Physics2D.Raycast(rayCastBegin, new Vector3(transform.localScale.x, 0, 0) * sightDist, sightDist, layerMask.value);
            RaycastHit2D hit2 = Physics2D.Raycast(rayCastBegin + Vector3.down * heightModifier, new Vector3(transform.localScale.x, 0, 0) * sightDist, sightDist, layerMask.value);
            Debug.DrawRay(rayCastBegin + Vector3.down * heightModifier, new Vector3(transform.localScale.x, 0, 0) * sightDist, Color.green);
            Debug.DrawRay(rayCastBegin, new Vector3(transform.localScale.x, 0, 0) * sightDist, Color.green);

            if (hit.collider != null)
            {
                currentState = State.Chase;
                targetObject = hit.collider.gameObject;
            }
            else if (hit2.collider != null)
            {
                currentState = State.Chase;
                targetObject = hit2.collider.gameObject;
                Debug.Log("I see you");
            }
            Move(new Vector3(0, 0, 0));
            if (timer >= investigateWait)
            {
                currentState = State.Patrolling;
            }
        }

        private RaycastHit2D lookFor(Vector2 origin, Vector2 direction, float distance, int layerMask)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);
            Debug.DrawRay(origin, direction, Color.green);
            return hit;
        }

        private Vector2 startVector()
        {
            System.Random rnd = new System.Random();
            int randInt = rnd.Next(1, 3);
            if (randInt == 1)
                return (new Vector2(1, 0));
            return (new Vector2(-1, 0));
        }

        protected virtual void Move(Vector3 direction)
        {
            if (direction.x != 0)
                animator.SetBool("Is Moving", true);
            else
                animator.SetBool("Is Moving", false);
            // Determine If On wall or ceiling
            float dot = Vector3.Dot(Vector3.up, currentSurfaceNormal);
            //Debug.Log(dot);
            bool onCeiling = (dot <= -.12);

            onWall = (Mathf.Abs(dot) <= 0.7); //or 0.7
            if (onWall)
            {
                //Debug.Log(onWall);
            }
            rayCastBegin = new Vector2(transform.position.x, transform.position.y + 1);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                transform.localScale = new Vector2(-.5f, transform.localScale.y);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                transform.localScale = new Vector2(.5f, transform.localScale.y);
            else if (direction.x > 0)
                transform.localScale = new Vector2(onCeiling ? -.5f : .5f, transform.localScale.y);
            else if (direction.x < 0)
                transform.localScale = new Vector2(onCeiling ? .5f : -.5f, transform.localScale.y);

            transform.position = transform.position + direction * speed * Time.deltaTime;
        }

        protected virtual void FixedUpdate()
        {
            switch (currentSurfaceState)
            {
                case SurfaceState.Default:
                    DefaultFixedUpdate();
                    break;
                case SurfaceState.Falling:
                    FallingFixedUpdate();
                    break;
                default:
                    break;
            }
        }

        protected virtual void TransitionStates(SurfaceState nextState)
        {
            Debug.Log("TRANSITIONING TO: " + nextState);
            currentSurfaceState = nextState;
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
                TransitionStates(SurfaceState.Default);
            }
        }
        #endregion
    }
}
