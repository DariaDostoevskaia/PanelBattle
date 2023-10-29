using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(LevelSO), menuName = "Config/LevelSO")]
    public class LevelSO : ScriptableObject
    {
        [SerializeField] private PanelSO[] _panelSettings;
        [SerializeField] private AICharacterSO[] _aICharacterSO;
        [SerializeField] private int[] _rect = new int[] { 5, 5 };
        [SerializeField] private float[] _startedPosition = new float[] { 0, 0 };
        [SerializeField] private float _spacing = 10f;
        [SerializeField] private int _price;
        [SerializeField] private int _reward;
        [SerializeField] private AudioClip _music;

        public int[] Rect => _rect;
        public int Price => _price;
        public int Reward => _reward;

        public float[] StartedPosition => _startedPosition;

        public float Spacing => _spacing;

        public AICharacterSO[] AICharactersSO => _aICharacterSO;

        public PanelSO[] PanelSettings => _panelSettings;
    }
}