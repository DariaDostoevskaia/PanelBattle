using LegoBattaleRoyal.Panels.View;
using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(PanelSO), menuName = "Config/PanelSO")]
    public class PanelSO : ScriptableObject
    {
        [SerializeField] private PanelView _panelViewPrefab;
        [SerializeField] private bool _isJumpBlock = true;
        [SerializeField] private int _jumpLenght = 1;

        public PanelView PanelView => _panelViewPrefab;

        public bool IsJumpBlock => _isJumpBlock;

        public int JumpLenght => _jumpLenght;
    }
}