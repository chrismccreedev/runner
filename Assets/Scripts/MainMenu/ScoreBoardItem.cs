using TMPro;
using UnityEngine;

namespace VitaliyNULL.MainMenu
{
    public class ScoreBoardItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _username;
        [SerializeField] private TMP_Text _score;

        public void SetUserAndScore(string username, int score)
        {
            _username.text = username;
            _score.text = score.ToString();
        }
    }
}