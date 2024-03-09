using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.Presentation.UI.LoadingPopup;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Loading
{
    public class LoadingController
    {
        private readonly LoadingScreenUI _loadingScreen;
        private readonly int _total = 100;
        private readonly float _seconds = 0.2f;

        public LoadingController(LoadingScreenUI loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }

        public void ShowLoadingPopup()
        {
            _loadingScreen.Show();
        }

        public void CloseLoadingPopup()
        {
            _loadingScreen.Close();
        }

        public void SetProgress(float percent)
        {
            _loadingScreen.SetProgress(percent);
        }

        public async UniTask LoadMockAsync()
        {
            IProgress<int> progress = new Progress<int>((progressValue) =>
            {
                float percent = (float)progressValue / _total;
                SetProgress(percent);
            });

            await LoadDataAsync(progress);
        }

        private async UniTask LoadDataAsync(IProgress<int> progress)
        {
            for (int i = 0; i <= _total; i++)
            {
                progress.Report(i);
                await UniTask.Delay(TimeSpan.FromSeconds(_seconds));
            }
        }
    }
}