using System.Collections;
using UnityEngine;
using VitaliyNULL.Obstacle;
using Random = UnityEngine.Random;

namespace VitaliyNULL.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        //something
        [SerializeField] private Vector3[] spawnCords;
        private float _spawnRate = 2f;
        private float _speed;
        private bool _canSpawn;
        public static SpawnManager Instance;

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
                Instance = this;
            }
            else
            {
                Instance = this;
            }
            _speed = 20f;
            StartSpawning();
        }

        public void StartSpawning()
        {
            StartCoroutine(SpawnObstacle());
        }

        private IEnumerator SpawnObstacle()
        {
            while (true)
            {
                if (_canSpawn)
                {
                    MoveObstacle spawnObj = ObjectPoolManager.Instance.GetPooledObject();
                    int firstIndex = Random.Range(0, spawnCords.Length);
                    spawnObj.transform.position = spawnCords[firstIndex];
                    spawnObj.SetSpeed(_speed);
                    spawnObj.gameObject.SetActive(true);
                    if (Random.Range(0, 100) < 50)
                    {
                        int secondIndex = firstIndex;
                        while (secondIndex == firstIndex)
                        {
                            secondIndex = Random.Range(0, spawnCords.Length);
                        }
                        spawnObj = ObjectPoolManager.Instance.GetPooledObject();
                        spawnObj.transform.position = spawnCords[secondIndex];
                        spawnObj.SetSpeed(_speed);
                        spawnObj.gameObject.SetActive(true);
                    }
                    _speed += 0.1f;
                    _spawnRate -= 0.01f;
                }
                WaitForSeconds waitForSeconds = new WaitForSeconds(_spawnRate);
                yield return waitForSeconds;
            }
        }

        public void SetSpawner(bool value)
        {
            _canSpawn = value;
        }
    }
}