using System.Collections;
using UnityEngine;

namespace VitaliyNULL.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private int _score = 0;
        private int _bestScore = 0;
        private Coroutine _coroutine;
        public static ScoreManager Instance;

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Update()
        {
            if (GameManager.Instance.GamePlaying)
            {
                _coroutine ??=StartCoroutine(WaitForUpdateScore());
            }
        }

        private IEnumerator WaitForUpdateScore()
        {
            yield return new WaitForSeconds(0.2f);
            UIManager.Instance.UpdateScoreUI(++_score);
            SetBestScore(_score);
            _coroutine = null;
        }

        public void SetBestScore(int val)
        {
            if (val > _bestScore)
            {
                _bestScore = val;
                UIManager.Instance.UpdateBestScoreUI(_bestScore);
            }
        }

        public void ResetBestScoreForNewCustomer()
        {
            _bestScore = 0;
        }

        public int GetBestScore() => _bestScore;

        public int GetScore() => _score;
    }
}