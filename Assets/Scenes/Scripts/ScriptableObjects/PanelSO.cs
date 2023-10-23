using LegoBattaleRoyal.Presentation.GameView.Panel;
using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(PanelSO), menuName = "Config/PanelSO")]
    public class PanelSO : ScriptableObject
    {
        [SerializeField] private PanelView _panelViewPrefab;
        [SerializeField] private bool _isJumpBlock = true;

        public PanelView PanelView => _panelViewPrefab;

        public bool IsJumpBlock => _isJumpBlock;
    }
}