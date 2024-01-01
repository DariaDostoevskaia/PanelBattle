using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.General
{
    public class GeneralPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private RectTransform _buttonContainer;
        [SerializeField] private Button _buttonPrefab;

        private readonly List<Button> _buttons = new();

        private void Start()
        {
            _buttonPrefab.gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public async UniTask ShowAsync()
        {
            Show();

            await UniTask.WaitWhile(() => gameObject.activeSelf);
        }

        public void Close()
        {
            gameObject.SetActive(false);

            ClearButtons();
        }

        private void ClearButtons()
        {
            foreach (var button in _buttons)
            {
                button.onClick.RemoveAllListeners();
                button.DestroyGameObject();
            }
            _buttons.Clear();
        }

        public void SetTitle(string title)
        {
            _title.SetText(title);
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }

        public Button CreateButton(string nameButton)
        {
            var button = Instantiate(_buttonPrefab, _buttonContainer);
            button.gameObject.SetActive(true);
            button.name = nameButton;
            button.GetComponentInChildren<TextMeshProUGUI>().SetText(nameButton);

            _buttons.Add(button);
            return button;
        }
    }
}