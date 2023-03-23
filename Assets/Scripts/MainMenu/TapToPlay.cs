using UnityEngine;
using VitaliyNULL.Managers;

namespace VitaliyNULL.MainMenu
{
    public class TapToPlay : MonoBehaviour
    {
        public void OnClick()
        {
            GameManager.Instance.StartGame();
            UIManager.Instance.OpenGameWindow();
        }


    }
}