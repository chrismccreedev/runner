using System;
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
        private float _minDeltaSwipe = 60f;
        private readonly float _timeToLerp = 9f;
        protected bool IsSwiping;
        [HideInInspector] public Rigidbody playerRigidbody;
        [HideInInspector] public Animator animator;
        [HideInInspector] public BoxCollider boxCollider;
        private readonly float _bounds = 2;
        private Coroutine _rolling;
        private Coroutine _turning;
        private Coroutine _destroyCoroutine;
        private Vector3 _toMove = new Vector3();
        private bool _isGameOver = false;

        //this needs for check what swipe is larger horizontal or vertical
        private float _horizontalSwipe;
        private float _verticalSwipe;

        [HideInInspector] public StateMachine.StateMachine stateMachine;

        private readonly List<Vector3> _positions = new List<Vector3>()
            { new Vector3(0, 0.5f, 0), new Vector3(2, 0.5f, 0), new Vector3(-2, 0.5f, 0) };


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
                _destroyCoroutine ??=StartCoroutine(WaitForDestroy(moveObstacle));
            }
        }

        private IEnumerator WaitForDestroy(MoveObstacle obstacle)
        {
            while (!GameManager.Instance.GamePlaying)
            {
                yield return new WaitForFixedUpdate();
            }
            obstacle.DeleteObstacle();
        }


        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
            playerRigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider>();
            stateMachine = GetComponent<StateMachine.StateMachine>();
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

            _horizontalSwipe = Mathf.Abs(SwipeDelta.x - StartTouchPos.x);
            _verticalSwipe = Mathf.Abs(SwipeDelta.y - StartTouchPos.y);
            // if vertical > that is true, if horizontal > that is false
            if (_horizontalSwipe > _minDeltaSwipe && _horizontalSwipe > _verticalSwipe)
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
            else if (_verticalSwipe > _minDeltaSwipe && _verticalSwipe > _horizontalSwipe)
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
            _turning = StartCoroutine(WaitToGoLeft());
        }

        private void GoRight()
        {
            _turning = StartCoroutine(WaitToGoRight());
        }

        private IEnumerator WaitToGoLeft()
        {
            bool hasPos = false;
            foreach (var position in _positions)
            {
                if (Mathf.RoundToInt(transform.position.x - position.x) == _bounds)
                {
                    _toMove = position;
                    hasPos = true;
                    break;
                }
            }

            if (hasPos)
            {
                while (transform.position.x > _toMove.x)
                {
                    float t = 0;
                    t += Time.deltaTime * _timeToLerp;
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(_toMove.x, transform.position.y, _toMove.z), t);
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        private IEnumerator WaitToGoRight()
        {
            bool hasPos = false;
            foreach (var position in _positions)
            {
                if (Mathf.RoundToInt(transform.position.x - position.x) == -_bounds)
                {
                    _toMove = position;
                    hasPos = true;
                    break;
                }
            }

            if (hasPos)
            {
                while (transform.position.x < _toMove.x)
                {
                    float t = 0;
                    t += Time.deltaTime * _timeToLerp;
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(_toMove.x, transform.position.y, _toMove.z), t);
                    yield return new WaitForFixedUpdate();
                }
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