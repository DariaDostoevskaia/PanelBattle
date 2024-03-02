using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.LoadingPopup
{
    public class LoadingScreenUI : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private float _seconds = 0.2f;
        private readonly int _total = 100;

        public async UniTask LoadMockAsync()
        {
            IProgress<int> progress = new Progress<int>((progressValue) =>
            {
                float percent = (float)progressValue / _total;
                SetProgress(percent);
            });

            // Имитация загрузки данных
            await LoadDataAsync(progress);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private async UniTask LoadDataAsync(IProgress<int> progress)
        {
            // Логика загрузки данных с обновлением прогресса
            for (int i = 0; i <= _total; i++)
            {
                progress.Report(i); // Сообщить о прогрессе загрузки
                await UniTask.Delay(TimeSpan.FromSeconds(_seconds));
            }
        }

        private void SetProgress(float percent)
        {
            if (_progressBar != null)
                _progressBar.value = percent;
        }
    }
}