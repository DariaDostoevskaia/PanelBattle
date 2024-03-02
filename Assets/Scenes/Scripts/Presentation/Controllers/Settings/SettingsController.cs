using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsController : IDisposable
{
    public event Action Closed;

    private readonly SettingsPopup _settingsPopup;
    private readonly SoundController _soundController;

    public SettingsController(SettingsPopup settingsPopup, SoundController soundController)
    {
        _settingsPopup = settingsPopup;
        _soundController = soundController;

        _settingsPopup.OnMusicVolumeChanged += _soundController.SetMusicVolume;
        _settingsPopup.OnSoundVolumeChanged += _soundController.SetSoundVolume;

        _settingsPopup.Closed += OnClosed;
        _settingsPopup.OnHomeClicked += OnHomeClicked;
    }

    private void OnHomeClicked()
    {
        var progress = new Progress<float>((progressValue) =>
        {
            //var loadingController;
            Debug.Log(progressValue);
        });
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentSceneIndex).ToUniTask(progress).Forget();
    }

    private void OnClosed()
    {
        Closed?.Invoke();
    }

    public void ShowSettings()
    {
        _settingsPopup.Show();
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
    }
}