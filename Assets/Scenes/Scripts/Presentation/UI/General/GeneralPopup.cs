using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Presentation.UI.Base;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.General
{
    public class GeneralPopup : BaseViewUI
    {
        public event Action OnGeneralButtonClicked;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _title;

        [SerializeField] private TextMeshProUGUI _energyCountText;
        [SerializeField] private RectTransform _energyPanel;

        [SerializeField] private RectTransform _buttonContainer;
        [SerializeField] private Button _buttonPrefab;

        [SerializeField] private Button _closeButton;

        private readonly List<Button> _buttons = new();

        public RectTransform RectTransform { get; set; }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _buttonPrefab.gameObject.SetActive(false);
            _closeButton.onClick.AddListener(Close);
        }

        public override void Close()
        {
            base.Close();

            ClearButtons();
        }

        public void SetActiveCloseButton(bool isActive)
        {
            _closeButton.gameObject.SetActive(isActive);
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

        public void SetEnergyCount(int count)
        {
            ShowEnergyContainer();
            _energyCountText.SetText($"{count}");
        }

        public void CloseEnergyContainer()
        {
            _energyPanel.gameObject.SetActive(false);
        }

        private void ShowEnergyContainer()
        {
            _energyPanel.gameObject.SetActive(true);
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
            button.onClick.AddListener(() => OnGeneralButtonClicked?.Invoke());

            return button;
        }

        public async UniTask ShowAsync()
        {
            Show();

            await UniTask.WaitWhile(() => gameObject.activeSelf);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            OnGeneralButtonClicked = null;
        }
    }
}