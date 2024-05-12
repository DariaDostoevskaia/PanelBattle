using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Presentation.Controllers.Loading;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LegoBattaleRoyal.Presentation.Controllers.Settings
{
    public class SettingsController : IDisposable
    {
        public event Action Closed;

        private readonly SettingsPopup _settingsPopup;
        private readonly SoundController _soundController;
        private readonly CameraController _cameraController;
        private readonly LoadingController _loadingController;

        public SettingsController(SettingsPopup settingsPopup,
            SoundController soundController, LoadingController loadingController, CameraController cameraController)
        {
            _settingsPopup = settingsPopup;
            _soundController = soundController;
            _loadingController = loadingController;
            _cameraController = cameraController;

            _settingsPopup.OnMusicVolumeChanged += _soundController.SetMusicVolume;
            _settingsPopup.OnSoundVolumeChanged += _soundController.SetSoundVolume;

            _settingsPopup.Closed += OnClosed;
            _settingsPopup.OnHomeClicked += OnHomeClicked;

            _settingsPopup.OnOkClicked += _cameraController.ShowRaycaster;
            _settingsPopup.OnCloseClicked += _cameraController.ShowRaycaster;
        }

        private void OnHomeClicked()
        {
            var progress = new Progress<float>((progressValue) =>
            {
                _loadingController.SetProgress(progressValue);
                Debug.Log(progressValue);
            });
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadSceneAsync(currentSceneIndex).ToUniTask(progress).Forget();
            _cameraController.ShowRaycaster();
        }

        private void OnClosed()
        {
            Closed?.Invoke();
            _cameraController.ShowRaycaster();
        }

        public void ShowSettings()
        {
            _settingsPopup.Show();
            _cameraController.CloseRaycaster();
        }

        public void CloseSettings()
        {
            _settingsPopup.Close();
        }

        public void Dispose()
        {
            Closed = null;

            _settingsPopup.OnMusicVolumeChanged -= _soundController.SetMusicVolume;
            _settingsPopup.OnSoundVolumeChanged -= _soundController.SetSoundVolume;

            _settingsPopup.Closed -= OnClosed;
            _settingsPopup.OnHomeClicked -= OnHomeClicked;

            _settingsPopup.OnOkClicked -= _cameraController.ShowRaycaster;
            _settingsPopup.OnCloseClicked -= _cameraController.ShowRaycaster;
        }
    }
}