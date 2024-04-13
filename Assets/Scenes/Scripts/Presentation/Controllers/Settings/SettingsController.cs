using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.Controllers.Topbar;
using System;

public class SettingsController : IDisposable
{
    private readonly TopbarController _topbarController;
    private readonly SettingsPopup _settingsPopup;
    private readonly SoundController _soundController;
    private readonly CameraController _cameraController;

    public SettingsController
        (TopbarController topbarController,
        SettingsPopup settingsPopup,
        SoundController soundController,
        CameraController cameraController)
    {
        _topbarController = topbarController;
        _settingsPopup = settingsPopup;
        _soundController = soundController;
        _cameraController = cameraController;

        _settingsPopup.OnMusicVolumeChanged += _soundController.SetMusicVolume;
        _settingsPopup.OnSoundVolumeChanged += _soundController.SetSoundVolume;

        _topbarController.OnButtonClicked += ShowSettings;

        _settingsPopup.OnOkClicked += _cameraController.ShowRaycaster;
        _settingsPopup.OnCloseClicked += _cameraController.ShowRaycaster;
        _settingsPopup.OnHomeClicked += _cameraController.ShowRaycaster;
    }

    public void ShowSettings()
    {
        _settingsPopup.Show();
        _cameraController.CloseRaycaster();
    }

    public void Dispose()
    {
        _settingsPopup.OnMusicVolumeChanged -= _soundController.SetMusicVolume;
        _settingsPopup.OnSoundVolumeChanged -= _soundController.SetSoundVolume;

        _topbarController.OnButtonClicked -= ShowSettings;

        _settingsPopup.OnOkClicked -= _cameraController.ShowRaycaster;
        _settingsPopup.OnCloseClicked -= _cameraController.ShowRaycaster;
        _settingsPopup.OnHomeClicked -= _cameraController.ShowRaycaster;
    }
}