using System.Collections;
using UnityEngine;

namespace VitaliyNULL.StateMachine
{
    public class SlideState : State
    {
        private BoxCollider _boxCollider;
        private Rigidbody _rigidbody;
        private bool _isSliding;
        private float _downForce = 6f;
        Vector3 center = Vector3.zero;
        Vector3 size = Vector3.zero;

        public SlideState(Animator animator, BoxCollider boxCollider, Rigidbody rigidbody) : base(animator)
        {
            _boxCollider = boxCollider;
            _rigidbody = rigidbody;
            _isSliding = false;
        }

        public override IEnumerator Start(StateMachine stateMachine)
        {
            if (!_isSliding)
            {
                _isSliding = true;
                center = _boxCollider.center;
                size = _boxCollider.size;
                _boxCollider.center = new Vector3(0, center.y / 5, 0);
                _boxCollider.size = new Vector3(1, size.y / 4, 1);
                Animator.CrossFade(AnimationsName.Slide, 0.1f);
                _rigidbody.AddForce(Vector3.down * _downForce, ForceMode.Impulse);
                yield return new WaitForSeconds(Animator.GetCurrentAnimatorClipInfo(0)[0].weight);
                _boxCollider.center = center;
                _boxCollider.size = size;
                if (stateMachine.CurrentState != stateMachine.dictionary[typeof(JumpState)])
                {
                    stateMachine.SwitchState<RunState>();
                }

                _isSliding = false;
            }
        }

        public override IEnumerator Stop(StateMachine stateMachine)
        {
            Animator.StopPlayback();
            yield return null;
        }

        public override IEnumerator StopImmediate(StateMachine stateMachine)
        {
            _isSliding = false;
            _boxCollider.center = center;
            _boxCollider.size = size;
            yield return null;
        }
    }
}