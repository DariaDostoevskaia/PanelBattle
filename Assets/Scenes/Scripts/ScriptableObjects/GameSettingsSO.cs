using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(GameSettingsSO), menuName = "Config/GameSettingsSO")]
    public class GameSettingsSO : ScriptableObject
    {
        [SerializeField] private CharacterSO _characterSO;
        [SerializeField] private PanelSO[] _panelSettings;
        [SerializeField] private GridPanelSettingsSO _gridPanelSettings;

        public CharacterSO CharacterSO => _characterSO;
        public PanelSO[] PanelSettings => _panelSettings;
        public GridPanelSettingsSO GridPanelSettings => _gridPanelSettings;
    }
}