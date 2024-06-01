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
        private readonly LevelSO _levelSettings;

        public GridFactory(LevelSO levelSettings)
        {
            _panelSettings = levelSettings.PanelSettings;
            _levelSettings = levelSettings;
        }

        public (PanelModel panelModel, PanelView panelView)[] CreatePairs(Transform parent)
        {
            var grid = BlockMatrixGenerator.GenerateGrid(_levelSettings.Rect);

            var polygon = BlockMatrixGenerator.GeneratePolygon(_levelSettings.StartedPosition,
                _levelSettings.Rect,
                _levelSettings.Spacing);

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
            PanelSO panelSetting;

            //TODO min and max - x and y
            var jumpBlockRequired = false;
            if (jumpBlockRequired)
            {
                var jumpPanelSettings = _panelSettings
                    .Where((panel) => panel.IsJumpBlock)
                    .ToList();

                var random = Random.Range(0, jumpPanelSettings.Count);
                panelSetting = _panelSettings[random];
            }
            else
            {
                var random = Random.Range(0, _panelSettings.Length);
                panelSetting = _panelSettings[random];
            }

            var panelModel = new PanelModel(panelSetting.IsJumpBlock, gridPosition); //+ IsJumpRequired

            var panelView = Object
               .Instantiate(panelSetting.PanelView, new Vector3(cell[0], parent.position.y, cell[1]),
               Quaternion.identity, parent);

            return (panelModel, panelView);
        }
    }
}