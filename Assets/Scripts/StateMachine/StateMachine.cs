using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VitaliyNULL.PlayerController;

namespace VitaliyNULL.StateMachine
{
    public class StateMachine : MonoBehaviour, IStateSwitcher
    {
        public Dictionary<Type, State> dictionary = new Dictionary<Type, State>();
        public State CurrentState;
        private SwipeController _swipeController;
        private Animator _animator;
        private Rigidbody _rigidbody;
        private BoxCollider _boxCollider;
        private Coroutine _stopCoroutine;
        private Coroutine _startCoroutine;
        private State _stateToStop;

        private void Start()
        {
            _swipeController = GetComponent<SwipeController>();
            _animator = _swipeController.animator;
            _rigidbody = _swipeController.playerRigidbody;
            _boxCollider = _swipeController.boxCollider;
            dictionary.Add(typeof(RunState), new RunState(_animator));
            dictionary.Add(typeof(SlideState), new SlideState(_animator, _boxCollider, _rigidbody));
            dictionary.Add(typeof(JumpState), new JumpState(_animator, _rigidbody));
            dictionary.Add(typeof(DeathState), new DeathState(_animator));
            CurrentState = dictionary[typeof(RunState)];
            _stateToStop = dictionary[typeof(JumpState)];
        }

        public void SwitchState<T>() where T : State
        {
            bool jump = false;
            bool slide = false;
            if (CurrentState == dictionary[typeof(JumpState)])
            {
                _stateToStop = CurrentState;
                jump = true;
            }
            else if (CurrentState == dictionary[typeof(SlideState)])
            {
                _stateToStop = CurrentState;
                slide = true;
            }

            StartCoroutine(CurrentState.Stop(this));
            CurrentState = dictionary[typeof(T)];

            if (CurrentState == dictionary[typeof(JumpState)])
            {
                jump = true;
            }
            else if (CurrentState == dictionary[typeof(SlideState)])
            {
                slide = true;
            }

            if (jump && slide)
            {
                if (_startCoroutine != null) StopCoroutine(_startCoroutine);
                StartCoroutine(_stateToStop.StopImmediate(this));
            }

            if (typeof(T) == typeof(DeathState))
            {
                StopAllCoroutines();
                StartCoroutine(dictionary[typeof(JumpState)].StopImmediate(this));
                StartCoroutine(dictionary[typeof(SlideState)].StopImmediate(this));
            }

            StartCoroutine(WaitBetweenStates());
            _startCoroutine = StartCoroutine(CurrentState.Start(this));
        }

        private IEnumerator WaitBetweenStates()
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}