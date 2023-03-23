using System.Collections;
using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class DeathState : State
    {
        public DeathState(Animator animator) : base(animator)
        {
        }

        public override IEnumerator Start(StateMachine stateMachine)
        {
            Animator.CrossFade(AnimationsName.Death, 0.1f);
            yield return new WaitForSeconds(Animator.GetCurrentAnimatorClipInfo(0)[0].weight);
        }

        public override IEnumerator Stop(StateMachine stateMachine)
        {
            Animator.StopPlayback();
            yield return null;
        }

    }
}