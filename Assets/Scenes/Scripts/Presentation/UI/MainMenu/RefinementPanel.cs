using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.MainMenu
{
    public class RefinementPanel : MonoBehaviour
    {
        public event Action<bool> RemoveProgressClicked;

        [SerializeField] private TextMeshProUGUI _textPanel;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        private void Start()
        {
            _yesButton.onClick.AddListener(() =>
            {
                RemoveProgressClicked?.Invoke(true);
                Close();
            });

            _noButton.onClick.AddListener(() =>
            {
                RemoveProgressClicked?.Invoke(false);
                Close();
            });
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            RemoveProgressClicked = null;

            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
        }
    }
}