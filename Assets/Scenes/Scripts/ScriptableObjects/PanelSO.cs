using LegoBattaleRoyal.Panels.View;
using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(PanelSO), menuName = "Config/PanelSO")]
    public class PanelSO : ScriptableObject
    {
        [SerializeField] private PanelView _panelViewPrefab;

        public PanelView PanelView => _panelViewPrefab;
    }
}