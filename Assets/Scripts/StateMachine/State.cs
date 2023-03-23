using System.Collections;
using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public abstract class State
    {
        protected Animator Animator;
        protected HashAnimationsName AnimationsName = new HashAnimationsName();
        // protected StateMachine StateMachine;

        public State(Animator animator)
        {
            Animator = animator;
        }

        public abstract IEnumerator Start(StateMachine stateMachine);

        public abstract IEnumerator Stop(StateMachine stateMachine);

        public virtual IEnumerator StopImmediate(StateMachine stateMachine)
        {
            yield return null;
        }
    }
}