using UnityEngine;
using VitaliyNULL.Managers;

namespace VitaliyNULL.PlayerController
{
    public class PlayerMovement : MonoBehaviour
    {
        private IController _controller;
        [SerializeField] private EditorController editorController;
        [SerializeField] private AndroidController androidController;

        private void Start()
        {
#if UNITY_EDITOR
            _controller = editorController;
            Destroy(androidController);
#elif UNITY_ANDROID
            _controller = androidController;
            Destroy(editorController);
#endif
        }


        private void FixedUpdate()
        {
            if (!GameManager.Instance.GamePlaying) return;
            _controller.Run();
        }
    }
}