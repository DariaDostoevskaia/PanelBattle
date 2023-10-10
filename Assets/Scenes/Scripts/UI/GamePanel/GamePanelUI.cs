using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.UI.GamePanel
{
    public class GamePanelUI : MonoBehaviour
    {
        public event Action OnRestartClicked;

        public event Action OnExitMainMenu;

        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Button _restartGameButton;
        [SerializeField] private Button _exitMainMenuGameButton;

        private void Start()
        {
            _restartGameButton.onClick.AddListener(() => OnRestartClicked?.Invoke());
            _exitMainMenuGameButton.onClick.AddListener(() => OnExitMainMenu?.Invoke());
        }

        public void SetTitle(string text)
        {
            _titleText.SetText(text);
        }

        public void SetActiveRestartButton(bool value)
        {
            _restartGameButton.gameObject.SetActive(value);
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
            OnRestartClicked = null;
            OnExitMainMenu = null;

            _restartGameButton.onClick.RemoveAllListeners();
            _exitMainMenuGameButton.onClick.RemoveAllListeners();
        }
    }
}