using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class HashAnimationsName
    {
        public int Run => Animator.StringToHash("Run");
        public int Jump => Animator.StringToHash("Jump");
        public int Death => Animator.StringToHash("Death");
        public int Slide => Animator.StringToHash("Slide");
    }
}