using System;
using UnityEngine;
using UnityEngine.UIElements;
using VitaliyNULL.Managers;

namespace VitaliyNULL.Obstacle
{
    public class MoveObstacle : MonoBehaviour
    {
        private float _speed = 30f;
        private Rigidbody _rigidbody;
        private BoxCollider _boxCollider;
        public bool isLowerObstacle;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (isLowerObstacle)
            {
                _boxCollider = GetComponent<BoxCollider>();
            }
        }

        private void OnEnable()
        {
            if (isLowerObstacle)
            {
                if (_boxCollider != null && _rigidbody != null)
                {
                    _boxCollider.isTrigger = false;
                    _rigidbody.isKinematic = false;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.GamePlaying) return;
            _rigidbody.MovePosition(transform.position + Vector3.back * _speed * Time.deltaTime);
        }

        private void Update()
        {
            if (transform.position.z < -10f)
            {
                gameObject.SetActive(false);
            }
        }

        public void SetSpeed(float value)
        {
            _speed = value;
        }

        public void MakeObstacleTriggered()
        {
            if (isLowerObstacle)
            {
                _boxCollider.isTrigger = true;
                _rigidbody.isKinematic = true;
            }
        }
    }
}