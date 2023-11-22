using LegoBattaleRoyal.Presentation.GameView.CapturePath;
using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(GameSettingsSO), menuName = "Config/GameSettingsSO")]
    public class GameSettingsSO : ScriptableObject
    {
        [SerializeField] private CharacterSO _characterSO;

        [SerializeField] private CapturePathView _capturePathViewPrefab;

        [SerializeField] private LevelSO[] _levels;

        [SerializeField] private int _money = 0;

        public CharacterSO CharacterSO => _characterSO;

        public LevelSO[] Levels => _levels;

        public CapturePathView CapturePathViewPrefab => _capturePathViewPrefab;

        public int Money => _money;
    }
}