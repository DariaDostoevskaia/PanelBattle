using LegoBattaleRoyal.Presentation.Controllers.Sound;
using System;

public class SettingController : IDisposable
{
    private readonly SettingsPopup _settingsPopup;
    private readonly SoundController _soundController;

    public SettingController(SettingsPopup settingsPopup, SoundController soundController)
    {
        _settingsPopup = settingsPopup;
        _soundController = soundController;

        _settingsPopup.OnMusicVolumeChanged += _soundController.SetMusicVolume;
        _settingsPopup.OnSoundVolumeChanged += _soundController.SetSoundVolume;
    }

    public void Dispose()
    {
        _settingsPopup.OnMusicVolumeChanged -= _soundController.SetMusicVolume;
        _settingsPopup.OnSoundVolumeChanged -= _soundController.SetSoundVolume;
    }
}