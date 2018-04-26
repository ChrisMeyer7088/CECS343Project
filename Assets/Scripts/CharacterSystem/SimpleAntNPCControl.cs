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
        public Vector2 WalkInput = new Vector2(1, 0);

        protected override void DefaultUpdate()
        {
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
    }
}
