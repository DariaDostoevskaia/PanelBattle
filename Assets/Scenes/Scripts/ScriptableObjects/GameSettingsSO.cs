using LegoBattaleRoyal.Presentation.CapturePath;
using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(GameSettingsSO), menuName = "Config/GameSettingsSO")]
    public class GameSettingsSO : ScriptableObject
    {
        [SerializeField] private CharacterSO _characterSO;
        [SerializeField] private AICharacterSO _aICharacterSO;
        [SerializeField] private PanelSO[] _panelSettings;
        [SerializeField] private GridPanelSettingsSO _gridPanelSettings;
        [SerializeField] private CapturePathView _capturePathViewPrefab;
        [SerializeField] private int _botCount;
        [SerializeField] private CapturePathView _capturePathViewPrefab;

        public CharacterSO CharacterSO => _characterSO;

        public AICharacterSO AICharacterSO => _aICharacterSO;

        public PanelSO[] PanelSettings => _panelSettings;

        public GridPanelSettingsSO GridPanelSettings => _gridPanelSettings;

        public CapturePathView CapturePathViewPrefab => _capturePathViewPrefab;

        public int BotCount => _botCount;

        public CapturePathView CapturePathViewPrefab => _capturePathViewPrefab;
    }
}