using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using LegoBattaleRoyal.ScriptableObjects;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static LegoBattaleRoyal.Panels.Controllers.PanelController;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LegoBattaleRoyal.Panels.Controllers
{
    public class GridFactory
    {
        private readonly PanelSO[] _panelSettings;
        private GridPanelSettingsSO _gridPanelSettings;
        private GridPosition _gridPosition;
        private PanelController _panelController;

        public GridFactory(PanelSO[] panelSettings)
        {
            _panelSettings = panelSettings;
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

                    _gridPosition = new GridPosition(row, column);

                    var pair = CreatePair(cell, parent);
                    return pair;
                })
                .ToArray();

            return pairs;
        }

        private (PanelModel panelModel, PanelView panelView) CreatePair(float[] cell, Transform parent)
        {
            var lenght = _panelSettings.Length;
            var random = Random.Range(0, lenght);
            var panelSetting = _panelSettings[random];

            var panelModel = new PanelModel(panelSetting.IsJumpBlock);

            var available = /*_panelController.*/MarkToAvailableNeighborPanels(_gridPosition, _gridPanelSettings.JumpLenght);

            if (panelModel.IsJumpBlock
                && available == true)
                panelModel.SetAvailable();
            else
                panelModel.SetUnavailable();

            var panelView = Object
               .Instantiate(panelSetting.PanelView,
               new Vector3(cell[0], parent.position.y, cell[1]),
               Quaternion.identity,
               parent);

            return (panelModel, panelView);
        }

        public bool MarkToAvailableNeighborPanels(GridPosition gridPosition, int movementRadius)
        {
            var panelController = _panelController;
            for (var rowOffset = -movementRadius; rowOffset <= movementRadius; rowOffset++)
            {
                var neighborRow = gridPosition.Row + rowOffset;
                if (neighborRow < 0)
                    continue;

                for (var columnOffset = -movementRadius; columnOffset <= movementRadius; columnOffset++)
                {
                    if (rowOffset == 0 && columnOffset == 0)
                        continue;

                    var neighborColumn = gridPosition.Column + columnOffset;
                    if (neighborColumn < 0)
                        continue;

                    var neighborGridPosition = new GridPosition(neighborRow, neighborColumn);
                    var (neighborPanelModel, _) = panelController.Pairs.FirstOrDefault(pair => pair.panelModel.GridPosition
                    .Equals(neighborGridPosition));

                    neighborPanelModel?.SetAvailable(/*_mainCharacterId*/);
                }
            }

            return false;
        }
    }
}