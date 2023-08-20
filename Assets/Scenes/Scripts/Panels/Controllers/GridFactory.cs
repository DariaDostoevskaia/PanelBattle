using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using LegoBattaleRoyal.ScriptableObjects;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.Panels.Controllers
{
    public class GridFactory
    {
        private readonly PanelSO[] _panelSettings;

        public GridFactory(PanelSO[] panelSettings)
        {
            _panelSettings = panelSettings;
        }

        public (PanelModel panelModel, PanelView panelView)[] CreatePairs(Transform parent)
        {
            var gridPanelSettings = ScriptableObject.CreateInstance<GridPanelSettingsSO>();
            var grid = BlockMatrixGenerator
                .GenerateGrid(gridPanelSettings.Rect);

            var polygon = BlockMatrixGenerator
                .GeneratePolygon(gridPanelSettings.StartedPosition,
                gridPanelSettings.Rect,
                gridPanelSettings.Spacing);

            var pairs = polygon
                .Select((cell, i) =>
                {
                    var gridCell = grid[i];
                    var pair = CreatePair(cell, parent);
                    return pair;
                })
                .ToArray();

            return pairs;
        }

        private (PanelModel panelModel, PanelView panelView) CreatePair(float[] cell, Transform parent)
        {
            var random = Random.Range(0, _panelSettings.Length);

            var panelSetting = _panelSettings[random];

            var panelModel = new PanelModel(panelSetting.IsJumpBlock);

            panelModel.SetAvailable();

            var panelView = Object
                .Instantiate(panelSetting.PanelView, new Vector3(cell[0], parent.position.y, cell[1]), Quaternion.identity, parent);

            return (panelModel, panelView);
        }
    }
}