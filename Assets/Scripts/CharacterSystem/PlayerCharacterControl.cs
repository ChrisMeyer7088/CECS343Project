using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Team343.CharacterSystem
{
    public class PlayerCharacterControl : InsectCharacterControlBase
    {
        protected override void DefaultUpdate()
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
            Move(dir);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TransitionStates(CharacterState.Falling);
            }
        }
    }
}
