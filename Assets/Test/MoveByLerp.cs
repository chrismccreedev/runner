using System.Collections;
using UnityEngine;

namespace VitaliyNULL.Test
{
    public class MoveByLerp : MonoBehaviour
    {
        public float _timeToLerp = 0.3f;

        public void GoLeft()
        {
            StartCoroutine(WaitGoLeft());
        }

        public void GoRight()
        {
            StartCoroutine(WaitGoRight());
        }

        private IEnumerator WaitGoLeft()
        {
            float toMove = transform.position.x + Vector3.left.x * 2;
            while (transform.position.x >= toMove)
            {
                float t = 0;
                t += Time.deltaTime * _timeToLerp;
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.left * 2, t);
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator WaitGoRight()
        {
            float toMove = transform.position.x + Vector3.right.x * 2;
            while (transform.position.x <= toMove)
            {
                float t = 0;
                t += Time.deltaTime * _timeToLerp;
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right * 2, t);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}