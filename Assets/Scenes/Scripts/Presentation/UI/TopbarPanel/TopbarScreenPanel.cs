using UnityEngine;
using UnityEngine.UI;

public class TopbarScreenPanel : MonoBehaviour
{
    [SerializeField] private Button _settingsPopupButton;

    public Button SettingsPopupButton => _settingsPopupButton;
}