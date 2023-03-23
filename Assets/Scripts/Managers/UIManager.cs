using TMPro;
using UnityEngine;
using VitaliyNULL.MainMenu;

namespace VitaliyNULL.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject registrationWindow;
        [SerializeField] private GameObject authorizationWindow;
        [SerializeField] private GameObject mainMenuWindow;
        [SerializeField] private GameObject gameWindow;
        [SerializeField] private TMP_Text bestScoreText;
        [SerializeField] private GameObject loadingWindow;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private ScoreBoardItem scoreBoardItem;
        [SerializeField] private Transform scoreBoardContent;
        private int _scoreboardCount = 0;
        public static UIManager Instance;

        private void Awake()
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

        public void OpenLoadingWidnow()
        {
            loadingWindow.SetActive(true);
        }

        public void CloseLoadingWindow()
        {
            loadingWindow.SetActive(false);
        }

        public void OpenGameWindow()
        {
            gameWindow.SetActive(true);
        }

        public void CloseGameWindow()
        {
            gameWindow.SetActive(false);
        }

        public void OpenMainMenu()
        {
            mainMenuWindow.SetActive(true);
            authorizationWindow.SetActive(false);
            registrationWindow.SetActive(false);
        }

        public void OpenAuthWindow()
        {
            authorizationWindow.SetActive(true);
            mainMenuWindow.SetActive(false);
            registrationWindow.SetActive(false);
        }

        public void OpenRegistrationWindow()
        {
            registrationWindow.SetActive(true);
            authorizationWindow.SetActive(false);
            mainMenuWindow.SetActive(false);
        }

        public void UpdateScoreUI(int val)
        {
            scoreText.text = val.ToString();
        }

        public void UpdateBestScoreUI(int val)
        {
            bestScoreText.text = val.ToString();
        }

        public void AddScoreBoardItem(string username, int score)
        {
            if (_scoreboardCount < 10)
            {
                ScoreBoardItem item = Instantiate(scoreBoardItem, scoreBoardContent);
                item.SetUserAndScore(username, score);
                _scoreboardCount++;
            }
        }

        public void AddTest()
        {
            if (_scoreboardCount < 10)
            {
                ScoreBoardItem item = Instantiate(scoreBoardItem);
                _scoreboardCount++;
            }
        }

        public void DestroyAllScoreBoardItems()
        {
            foreach (Transform child in scoreBoardContent.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}