using Firebase;
using UnityEngine;
using UnityEngine.SceneManagement;
using VitaliyNULL.Firebase;

namespace VitaliyNULL.MainMenu
{
    public class ReloadScene: MonoBehaviour
    {
        public void OnClick()
        {
            FirebaseManager.Instance.SaveData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        

    }
}