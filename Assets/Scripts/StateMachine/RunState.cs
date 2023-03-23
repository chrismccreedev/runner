using System.Collections;
using UnityEngine;
using VitaliyNULL.PlayerController;

namespace VitaliyNULL.StateMachine
{
    public class RunState : State
    {
        private SwipeController _swipeController;
        private Rigidbody _rigidbody;

        public RunState(Animator animator) : base(animator)
        {
        }

        public override IEnumerator Start(StateMachine stateMachine)
        {
            Animator.StopPlayback();
            Animator.CrossFade(AnimationsName.Run, 0.1f);
            yield return null;
        }

        public override IEnumerator Stop(StateMachine stateMachine)
        {
            Animator.StopPlayback();
            yield return null;
        }
        
    }
}