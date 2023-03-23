using UnityEngine;

namespace VitaliyNULL.MainMenu
{
    public class ExitGame : MonoBehaviour
    {
        public void OnClick()
        {
            Application.Quit();
            Debug.Log("Quit from game");
        }
    }
}
