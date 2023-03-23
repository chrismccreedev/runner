using System.Collections.Generic;
using UnityEngine;
using VitaliyNULL.Obstacle;

namespace VitaliyNULL.Managers
{
    public class ObjectPoolManager : MonoBehaviour
    {
        [SerializeField] private List<MoveObstacle> obstacles;
        private List<MoveObstacle> _pooledObjects = new List<MoveObstacle>();
        public static ObjectPoolManager Instance;

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

            for (int i = 0; i < obstacles.Count; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    MoveObstacle gameObject = Instantiate(obstacles[i]);
                    _pooledObjects.Add(gameObject);
                    gameObject.gameObject.SetActive(false);
                }
            }

            _pooledObjects = Shuffle(_pooledObjects);
        }

        public void CleanAllObjects()
        {
            foreach (var obstacle in _pooledObjects)
            {
                obstacle.gameObject.SetActive(false);
            }
        }
        public MoveObstacle GetPooledObject()
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].gameObject.activeSelf)
                {

                    return _pooledObjects[i];
                }
            }

            return null;
        }

        private List<MoveObstacle> Shuffle(List<MoveObstacle> aList)
        {
            System.Random random = new System.Random();

            MoveObstacle myGo;

            int n = aList.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(random.NextDouble() * (n - i - 1));
                myGo = aList[r];
                aList[r] = aList[i];
                aList[i] = myGo;
            }

            return aList;
        }
    }
}