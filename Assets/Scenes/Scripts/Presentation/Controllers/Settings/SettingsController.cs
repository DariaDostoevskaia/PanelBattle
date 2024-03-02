using LegoBattaleRoyal.Presentation.Controllers.Sound;
using System;

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
    }
}