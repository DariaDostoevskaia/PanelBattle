using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.Controllers.Topbar;
using System;

public class SettingsController : IDisposable
{
    private readonly TopbarController _topbarController;
    private readonly SettingsPopup _settingsPopup;
    private readonly SoundController _soundController;

    public SettingsController(TopbarController topbarController, SettingsPopup settingsPopup, SoundController soundController)
    {
        _topbarController = topbarController;
        _settingsPopup = settingsPopup;
        _soundController = soundController;

        _settingsPopup.OnMusicVolumeChanged += _soundController.SetMusicVolume;
        _settingsPopup.OnSoundVolumeChanged += _soundController.SetSoundVolume;

        _topbarController.OnButtonClicked += ShowSettings;
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