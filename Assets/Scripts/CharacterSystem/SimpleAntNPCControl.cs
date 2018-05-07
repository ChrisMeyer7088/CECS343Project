using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Team343.CharacterSystem
{
    public class SimpleAntNPCControl : InsectCharacterControlBase
    {
        [Header("AI Control")]
		private float timer = 0;
		public Vector2 WalkInput;
		private bool changeWalk = true;

        protected override void DefaultUpdate()
        {
			timer += Time.deltaTime;
			if (changeWalk || timer >= 30) 
			{
				WalkInput = startVector ();
				timer = 0;
			}
			changeWalk = false;
            var dir = transform.TransformDirection(WalkInput);
            if (dir.x != 0)
            {
                animator.SetBool("While Moving", true);
            }
            else
            {
                animator.SetBool("While Moving", false);
            }
            Move(dir);
        }

		private Vector2 startVector()
		{
			System.Random rnd = new System.Random();
			int randInt = rnd.Next(1, 3);
			if (randInt == 1)
				return (new Vector2(1, 0));
			return (new Vector2(-1, 0));
		}
    }
}
