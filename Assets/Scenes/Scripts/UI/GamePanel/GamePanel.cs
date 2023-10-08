using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Button _endGameButton;
        [SerializeField] private Button _restartGameButton;
        public event Action OnRestartClicked;

        private void Start()
        {
            //_returnGameButton.onClick.AddListener(ReturnGame);
            //_endGameButton.onClick.AddListener(EndGame);
        }

        private void ReturnGame()
        {
            //_startGameButton.interactable = false;
        }

        private void EndGame()
        {
            //_endGameButton.interactable = false;
            //Application.Quit();
        }

        private void OnDestroy()
        {
            //_returnGameButton.onClick.RemoveAllListeners();
            //_endGameButton.onClick.RemoveAllListeners();
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

        internal void Close()
        {
            throw new NotImplementedException();
        }
    }
}
