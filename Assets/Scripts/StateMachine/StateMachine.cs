using System;
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
        }

        public void SwitchState<T>() where T : State
        {
            StartCoroutine(CurrentState.Stop(this));
            if (typeof(T) == typeof(DeathState))
            {
                StopAllCoroutines();
                StartCoroutine(dictionary[typeof(JumpState)].StopImmediate(this));
                StartCoroutine(dictionary[typeof(SlideState)].StopImmediate(this));
            }

            CurrentState = dictionary[typeof(T)];
            _startCoroutine = StartCoroutine(CurrentState.Start(this));
        }
    }
}