using System.Collections;
using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class JumpState : State
    {
        private Rigidbody _rigidbody;
        private float _jumpForce = 6f;
        private bool _isJumping;


        public JumpState(Animator animator, Rigidbody rigidbody) : base(animator)
        {
            _rigidbody = rigidbody;
        }

        public override IEnumerator Start(StateMachine stateMachine)
        {
            if (!_isJumping)
            {
                _isJumping = true;
                Animator.CrossFade(AnimationsName.Jump, 0.1f);
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                yield return new WaitForSeconds(Animator.GetCurrentAnimatorClipInfo(0)[0].weight /
                                                Animator.GetCurrentAnimatorStateInfo(0).speed);
                if (stateMachine.CurrentState != stateMachine.dictionary[typeof(SlideState)])
                {
                    stateMachine.SwitchState<RunState>();
                }

                _isJumping = false;
            }
        }

        public override IEnumerator Stop(StateMachine stateMachine)
        {
            Animator.StopPlayback();
            // _isJumping = false;
            yield return null;
        }

        public override IEnumerator StopImmediate(StateMachine stateMachine)
        {
            _isJumping = false;
            yield return null;
        }
    }
}