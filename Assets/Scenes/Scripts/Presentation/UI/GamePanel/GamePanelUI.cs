using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.GamePanel
{
    public class GamePanelUI : MonoBehaviour
    {
        public event Action OnRestartClicked;

        public event Action OnNextLevelClicked;

        public event Action OnRemoveAllProgressClicked;

        public event Action OnExitMainMenuClicked;

        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Button _restartGameButton;
        [SerializeField] private Button _nextLevelGameButton;
        [SerializeField] private Button _removeAllProgressGameButton;
        [SerializeField] private Button _exitMainMenuGameButton;

        private void Start()
        {
            _restartGameButton.onClick.AddListener(() => OnRestartClicked?.Invoke());
            _nextLevelGameButton.onClick.AddListener(() => OnNextLevelClicked?.Invoke());
            _removeAllProgressGameButton.onClick.AddListener(() => OnRemoveAllProgressClicked?.Invoke());
            _exitMainMenuGameButton.onClick.AddListener(() => OnExitMainMenuClicked?.Invoke());
        }

        public void SetTitle(string text)
        {
            _titleText.SetText(text);
        }

        public void SetActiveRestartButton(bool value)
        {
            _restartGameButton.gameObject.SetActive(value);
        }

        public void SetActiveNextLevelButton(bool value)
        {
            _nextLevelGameButton.gameObject.SetActive(value);
        }

        public void SetActiveRemoveAllProgress(bool value)
        {
            _removeAllProgressGameButton.gameObject.SetActive(value);
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
            OnNextLevelClicked = null;
            OnRemoveAllProgressClicked = null;
            OnExitMainMenuClicked = null;

            _restartGameButton.onClick.RemoveAllListeners();
            _nextLevelGameButton.onClick.RemoveAllListeners();
            _removeAllProgressGameButton.onClick.RemoveAllListeners();
            _exitMainMenuGameButton.onClick.RemoveAllListeners();
        }
    }
}