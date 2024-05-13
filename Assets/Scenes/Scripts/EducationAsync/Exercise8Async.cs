using Cysharp.Threading.Tasks;
using System.Collections.Generic;
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

            _button.onClick.AddListener(async () =>
            {
                await OnButtonClickAsync();
            });
        }

        private void SetText(string text)
        {
            _text.SetText(text);
        }

        public async UniTask OnButtonClickAsync()
        {
            await ShowTextForUniTask(2);
            await ShowTextForTask(4);

            StartCoroutine(ShowTextForCoroutine());
        }

        private IEnumerator<WaitForSeconds> ShowTextForCoroutine()
        {
            yield return new WaitForSeconds(6);
            SetText("Text updated after delay");
        }

        private async UniTask ShowTextForUniTask(float seconds)
        {
            await UniTask.WaitForSeconds(seconds);

            SetText("Text updated after delay");
        }

        private async UniTask ShowTextForTask(float seconds)
        {
            await Task.Delay((int)(seconds * 1000));

            SetText("Text updated after delay");
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}