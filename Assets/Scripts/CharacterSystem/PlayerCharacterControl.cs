using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//namespace Team343.CharacterSystem
//{
public class PlayerCharacterControl : InsectCharacterControlBase
{
    public bool speedBoost = false;
    public bool speedDrag = false;
    public float slowDuration = 0;
    public float boostDuration = 0;

    protected override void DefaultUpdate()
    {
        if (speedBoost)
        {
            boostDuration += Time.deltaTime;
            if (boostDuration >= 10)
            {
                speedBoost = false;
                speed = 5;
            }
        }
        if (speedDrag)
        {
            slowDuration += Time.deltaTime;
            if (slowDuration >= 5)
            {
                speedDrag = false;
                speed = 5;
            }
        }

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
        //Debug.Log(speed);
        Move(dir);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TransitionStates(CharacterState.Falling);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "honey")
        {
            speed = 9;
            boostDuration = 0;
            speedBoost = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "poison")
        {
            speed = 3;
            slowDuration = 0;
            speedDrag = true;
            Destroy(other.gameObject);
        }
    }
}
//}
