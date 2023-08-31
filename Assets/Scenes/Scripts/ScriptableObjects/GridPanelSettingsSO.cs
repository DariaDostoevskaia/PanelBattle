using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(GridPanelSettingsSO), menuName = "Config/GridPanelSettingsSO")]
    public class GridPanelSettingsSO : ScriptableObject
    {
        [SerializeField] private int[] _rect = new int[] { 8, 8 };
        [SerializeField] private float[] _startedPosition = new float[] { 0, 0 };
        [SerializeField] private float _spacing = 10f;
        [SerializeField] private int _jumpLenght = 1;
        public int[] Rect => _rect;

        public float[] StartedPosition => _startedPosition;

        public float Spacing => _spacing;

        public int JumpLenght => _jumpLenght;
    }
}