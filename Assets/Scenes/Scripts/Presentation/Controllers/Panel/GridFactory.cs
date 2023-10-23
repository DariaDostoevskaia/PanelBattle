using LegoBattaleRoyal.Core.Panels.Models;
using LegoBattaleRoyal.Presentation.GameView.Panel;
using LegoBattaleRoyal.ScriptableObjects;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LegoBattaleRoyal.Presentation.Controllers.Panel
{
    public class GridFactory
    {
        private readonly PanelSO[] _panelSettings;
        private GridPanelSettingsSO _gridPanelSettings;

        public GridFactory(PanelSO[] panelSettings, GridPanelSettingsSO gridPanelSettings)
        {
            _panelSettings = panelSettings;
            _gridPanelSettings = gridPanelSettings;
        }

        public (PanelModel panelModel, PanelView panelView)[] CreatePairs(Transform parent)
        {
            _gridPanelSettings = ScriptableObject.CreateInstance<GridPanelSettingsSO>();

            var grid = BlockMatrixGenerator.GenerateGrid(_gridPanelSettings.Rect);

            var polygon = BlockMatrixGenerator.GeneratePolygon(_gridPanelSettings.StartedPosition,
                _gridPanelSettings.Rect,
                _gridPanelSettings.Spacing);

            var pairs = polygon
                .Select((cell, i) =>
                {
                    var gridCell = grid[i];

                    var row = gridCell[0];
                    var column = gridCell[1];

                    var gridPosition = new GridPosition(row, column);

                    var pair = CreatePair(cell, parent, gridPosition);
                    return pair;
                })
                .ToArray();

            return pairs;
        }

        private (PanelModel panelModel, PanelView panelView) CreatePair(float[] cell, Transform parent, GridPosition gridPosition)
        {
            var lenght = _panelSettings.Length;
            var random = Random.Range(0, lenght);
            var panelSetting = _panelSettings[random];

            var panelModel = new PanelModel(panelSetting.IsJumpBlock, gridPosition);

            var panelView = Object
               .Instantiate(panelSetting.PanelView, new Vector3(cell[0], parent.position.y, cell[1]),
               Quaternion.identity, parent);

            return (panelModel, panelView);
        }
    }
}