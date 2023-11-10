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

        [SerializeField] private AudioClip _music;

        [SerializeField] private float _musicVolume = 1f;

        public CharacterSO CharacterSO => _characterSO;

        public LevelSO[] Levels => _levels;

        public CapturePathView CapturePathViewPrefab => _capturePathViewPrefab;

        public AudioClip MainMusic => _music;

        public float MusicVolume => _musicVolume;
    }
}