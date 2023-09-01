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
        private PanelModel _panelModel;
        private PanelController _panelController;

        private GridPanelSettingsSO gridPanelSettings;
        private int[] _gridPosition;

        public GridFactory(PanelSO[] panelSettings)
        {
            _panelSettings = panelSettings;
        }

        public (PanelModel panelModel, PanelView panelView)[] CreatePairs(Transform parent)
        {
            gridPanelSettings = ScriptableObject.CreateInstance<GridPanelSettingsSO>();
            var grid = BlockMatrixGenerator.GenerateGrid(gridPanelSettings.Rect);

            var polygon = BlockMatrixGenerator.GeneratePolygon(gridPanelSettings.StartedPosition,
                gridPanelSettings.Rect,
                gridPanelSettings.Spacing);

            var pairs = polygon
                .Select((cell, i) =>
                {
                    var gridCell = grid[i];
                    //_gridPosition = new GridPosition(gridCell[0], gridCell[1]);

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

            _panelModel = new PanelModel(panelSetting.IsJumpBlock);

            //if (_panelModel.IsJumpBlock)
            //    _panelModel.SetAvailable();
            //var available = MarkToAvailableNeighborPanels(_gridPosition, gridPanelSettings.JumpLenght);

            var panelView = Object
               .Instantiate(panelSetting.PanelView,
               new Vector3(cell[0], parent.position.y, cell[1]),
               Quaternion.identity,
               parent);

            return (_panelModel, panelView);
        }
    }
}