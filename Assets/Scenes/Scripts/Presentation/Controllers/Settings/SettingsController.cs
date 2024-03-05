using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.Presentation.Controllers.Loading;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.Controllers.Topbar;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LegoBattaleRoyal.Presentation.Controllers.Settings
{
    public class SettingsController : IDisposable
    {
        private readonly TopbarController _topbarController;
        private readonly SettingsPopup _settingsPopup;
        private readonly SoundController _soundController;
        private readonly LoadingController _loadingController;

        public SettingsController(TopbarController topbarController, SettingsPopup settingsPopup,
            SoundController soundController, LoadingController loadingController)
        {
            _topbarController = topbarController;
            _settingsPopup = settingsPopup;
            _soundController = soundController;
            _loadingController = loadingController;

            _settingsPopup.OnMusicVolumeChanged += _soundController.SetMusicVolume;
            _settingsPopup.OnSoundVolumeChanged += _soundController.SetSoundVolume;

            _topbarController.OnButtonClicked += ShowSettings;
        }

        private void OnHomeClicked()
        {
            // for settings popup branch:
            // _settingsPopup.OnHomeClicked += OnHomeClicked;

            var progress = new Progress<float>((progressValue) =>
            {
                _loadingController.LoadMockAsync().Forget();
                Debug.Log(progressValue);
            });
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadSceneAsync(currentSceneIndex).ToUniTask(progress).Forget();
        }

        public void ShowSettings()
        {
            _settingsPopup.Show();
        }

        public void Dispose()
        {
            _settingsPopup.OnMusicVolumeChanged -= _soundController.SetMusicVolume;
            _settingsPopup.OnSoundVolumeChanged -= _soundController.SetSoundVolume;

            _topbarController.OnButtonClicked -= ShowSettings;
        }
    }
}