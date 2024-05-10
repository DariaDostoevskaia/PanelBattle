using Cysharp.Threading.Tasks;
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
            await UpdateTextWithDelay(2);
        }

        private async UniTask UpdateTextWithDelay(float delayInSeconds)
        {
            await UniTask.Delay((int)(delayInSeconds * 1000));

            SetText("Text updated after delay");
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}