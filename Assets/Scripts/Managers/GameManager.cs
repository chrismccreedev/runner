using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using VitaliyNULL.PlayerController;
using VitaliyNULL.StateMachine;

namespace VitaliyNULL.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuWindow;
        public static GameManager Instance;
        [SerializeField] private GameObject gameOver;
        [SerializeField] private Image panel;
        [SerializeField] private Image continueImage;
        [SerializeField] private TMP_Text continueText;
        [SerializeField] private Button continueButton;
        private int _rewardedCount = 0;
        private float _timeToWait = 4f;
        private bool _isGamePlaying;

        public bool GamePlaying
        {
            get => _isGamePlaying;
            set
            {
                SpawnManager.Instance.SetSpawner(value);
                _isGamePlaying = value;
            }
        }

        private SwipeController _swipeController;


        private void Awake()
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
        }

        public void StartGame()
        {
            GamePlaying = true;
            _swipeController.stateMachine.SwitchState<RunState>();
            StartCoroutine(WaitingForStart());
            mainMenuWindow.SetActive(false);
        }

        private IEnumerator WaitingForStart()
        {
            yield return new WaitForSeconds(_timeToWait);
            SpawnManager.Instance.SetSpawner(true);
        }

        public void GetSwipeController(SwipeController swipeController)
        {
            _swipeController = swipeController;
        }


        public void PlayGameOver()
        {
            continueButton.interactable = false;
            GamePlaying = false;
            gameOver.SetActive(true);
            panel.gameObject.SetActive(true);
            if (_rewardedCount == 0)
            {
                continueImage.gameObject.SetActive(true);
                continueImage.DOColor(new Color(1, 1, 1, 1), 2);
                continueText.DOColor(new Color(0, 0, 0, 1), 2);
            }
            StartCoroutine(WaitUntilDoTween());
        }

        private IEnumerator WaitUntilDoTween()
        {
            gameOver.transform.position = new Vector3(Screen.width / 2, Screen.height / 2 - Screen.height, 0);
            gameOver.transform.DOMoveY(Screen.height / 2, 1);
            panel.DOColor(new Color(1, 1, 1, 0.5f), 1);
            yield return new WaitUntil(predicate: () => panel.color.a >= 0.5f);
            continueButton.interactable = true;
        }

        public void ContinueGame()
        {
            ObjectPoolManager.Instance.CleanAllObjects();
            GamePlaying = true;
            gameOver.SetActive(false);
            panel.gameObject.SetActive(false);
            continueImage.gameObject.SetActive(false);
            gameOver.transform.position = new Vector3(Screen.width / 2, Screen.height / 2 - Screen.height, 0);
            panel.DOColor(new Color(1, 1, 1, 0), 0);
            StopAllCoroutines();
            _swipeController.stateMachine.SwitchState<RunState>();
            _rewardedCount++;
        }
    }
}