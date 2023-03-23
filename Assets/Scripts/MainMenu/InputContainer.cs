using TMPro;
using UnityEngine;

namespace VitaliyNULL.MainMenu
{
    public enum ContainerEnum
    {
        Email,
        Name,
        Password,
        RepeatPassword
    }

    public class InputContainer : MonoBehaviour
    {
        #region Private Fields

        private string _containerText;
        private TMP_InputField _inputField;
        [SerializeField] private ContainerEnum containerEnum;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        #endregion

        #region Public Methods

        public void OnInputTextChange()
        {
            _containerText = _inputField.text;
        }

        public ContainerEnum GetContainerEnum() => containerEnum;

        public string GetInputText() => _containerText;

        #endregion
    }
}