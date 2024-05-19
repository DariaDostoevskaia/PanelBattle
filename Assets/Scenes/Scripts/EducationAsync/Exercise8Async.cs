using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise8Async : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;

        private void Start()
        {
            SetText("Text for exercise 8");

            _button.onClick.AddListener(() => OnButtonClick());
            //множество нажатий на кнопку - предотвратить    (состояние гонки)
        }

        private void SetText(string text)
        {
            _text.SetText(text);
        }

        private async void OnButtonClick()
        {
            await ShowTextForUniTask(2);
            await ShowTextForTask(4);
            StartCoroutine(ShowTextForCoroutine(6));
        }

        private IEnumerator ShowTextForCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            var text = "Text updated after delay";
            SetText(text);
        }

        private async UniTask ShowTextForUniTask(float seconds)
        {
            await UniTask.WaitForSeconds(seconds);

            SetText("Text updated after delay");
        }

        private async UniTask ShowTextForTask(float seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));

            SetText("Text updated after delay");
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}