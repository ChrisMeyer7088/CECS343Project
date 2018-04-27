using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

namespace Team343.CharacterSystem
{
    public class SivSimpleAntNPCControl : InsectCharacterControlBase
    {
     //   [Header("AI Control")]

		public float speed = 5.0f;
		public void Start() 
		{
			Vector2 randomDirection = new Vector2(UnityEngine.Random.value,UnityEngine.Random.value);
			transform.Rotate(randomDirection);
		}

        protected override void DefaultUpdate()
        {
			Vector2 WalkInput = new Vector2(1, 0);
            var dir = transform.TransformDirection(WalkInput);
            if (dir.x != 0)
            {
                animator.SetBool("While Moving", true);
            }
            else
            {
                animator.SetBool("While Moving", false);
            }
			transform.position += transform.forward;
			/*//pick random
			ArrayList list = new ArrayList();
			list.Add (1);
			list.Add (-1);
			int randDir = UnityEngine.Random.Range (0, 1);
			dir.x = (int) list[randDir];*/

			Move (dir);
        }
    }
}
