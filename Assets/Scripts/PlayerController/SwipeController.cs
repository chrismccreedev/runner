using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VitaliyNULL.Managers;
using VitaliyNULL.Obstacle;
using VitaliyNULL.StateMachine;

namespace VitaliyNULL.PlayerController
{
    public class SwipeController : MonoBehaviour
    {
        protected Vector2 StartTouchPos;
        protected Vector2 SwipeDelta;
        public float minDeltaSwipe = 40f;
        private float _timeToLerp = 5f;
        protected bool IsSwiping;
        private bool _isTurning;
        [HideInInspector] public Rigidbody playerRigidbody;
        [HideInInspector] public Animator animator;
        [HideInInspector] public BoxCollider boxCollider;
        private readonly float _horizontalForce = 2;
        private readonly float _bounds = 2;
        private Coroutine _rolling;
        [HideInInspector] public StateMachine.StateMachine stateMachine;

        private readonly List<Vector3> _positions = new List<Vector3>()
            { new Vector3(0, 0, 0), new Vector3(2, 0, 0), new Vector3(-2, 0, 0) };


        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                MoveObstacle moveObstacle = other.gameObject.GetComponent<MoveObstacle>();
                if (moveObstacle.isLowerObstacle)
                {
                    if (stateMachine.CurrentState == stateMachine.dictionary[typeof(JumpState)])
                    {
                        moveObstacle.MakeObstacleTriggered();
                        return;
                    }
                }
                GameManager.Instance.PlayGameOver();
                stateMachine.SwitchState<DeathState>();
            }
        }


        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
            playerRigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider>();
            stateMachine = GetComponent<StateMachine.StateMachine>();
            _isTurning = false;
        }

        private void Start()
        {
            GameManager.Instance.GetSwipeController(this);
        }

        protected void CheckSwipe()
        {
            SwipeDelta = Vector2.zero;
            if (IsSwiping)
            {
                SwipeDelta = Input.mousePosition;
            }

            if (Mathf.Abs(SwipeDelta.x - StartTouchPos.x) > minDeltaSwipe)
            {
                if (SwipeDelta.x < StartTouchPos.x)
                {
                    GoLeft();
                }
                else if (SwipeDelta.x > StartTouchPos.x)
                {
                    GoRight();
                }

                ResetSwipe();
            }
            else if (Mathf.Abs(SwipeDelta.y - StartTouchPos.y) > minDeltaSwipe)
            {
                if (SwipeDelta.y < StartTouchPos.y)
                {
                    stateMachine.SwitchState<SlideState>();
                }
                else if (SwipeDelta.y > StartTouchPos.y)
                {
                    stateMachine.SwitchState<JumpState>();
                }

                ResetSwipe();
            }
        }

        private void GoLeft()
        {
            if (Mathf.RoundToInt(transform.position.x) > -_bounds && !_isTurning)
            {
                StartCoroutine(WaitToGoLeft());
                //
                // playerRigidbody.MovePosition(transform.position +
                //                              Vector3.left * HorizontalForce);
            }
        }

        private IEnumerator WaitToGoLeft()
        {
            _isTurning = true;
            float moveX = transform.position.x + Vector3.left.x * _horizontalForce;
            while (transform.position.x > moveX)
            {
                float t = Time.deltaTime * _timeToLerp;
                transform.position = Vector3.MoveTowards(transform.position,
                    transform.position + Vector3.left * _horizontalForce, t);
                yield return null;
            }

            _isTurning = false;
        }

        private IEnumerator WaitToGoRight()
        {
            _isTurning = true;
            float moveX = transform.position.x + Vector3.right.x * _horizontalForce;
            while (transform.position.x < moveX)
            {
                float t = Time.deltaTime * _timeToLerp;
                transform.position = Vector3.MoveTowards(transform.position,
                    transform.position + Vector3.right * _horizontalForce, t);
                yield return null;
            }

            _isTurning = false;
        }

        private void GoRight()
        {
            if (Mathf.RoundToInt(transform.position.x) < _bounds && !_isTurning)
            {
                StartCoroutine(WaitToGoRight());
                // playerRigidbody.MovePosition(transform.position +
                //                              Vector3.right * HorizontalForce);
            }
        }

        protected void ResetSwipe()
        {
            IsSwiping = false;
            StartTouchPos = Vector2.zero;
            SwipeDelta = Vector2.zero;
        }
    }
}