using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.UI.GamePanel
{
    public class GamePanelUI : MonoBehaviour
    {
        public event Action OnRestartClicked;

        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Button _endGameButton;
        [SerializeField] private Button _restartGameButton;

        private void Start()
        {
            _restartGameButton.onClick.AddListener(() => OnRestartClicked?.Invoke());
            _endGameButton.onClick.AddListener(EndGame);
        }

        private void EndGame()
        {
            Application.Quit();
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
            _restartGameButton.onClick.RemoveAllListeners();
            _endGameButton.onClick.RemoveAllListeners();
        }
    }
}
