using LegoBattaleRoyal.Presentation.Controllers.Sound;
using UnityEngine;

public class TopbarController : MonoBehaviour
{
    private readonly SettingsPopup _settingsPopup;

    public TopbarController(SettingsPopup settingsPopup)
    {
        _settingsPopup = settingsPopup;
    }

    public void OpenSettings()
    {
        _settingsPopup.gameObject.SetActive(true);
    }
}