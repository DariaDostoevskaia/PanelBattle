using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using LegoBattaleRoyal.ScriptableObjects;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace LegoBattaleRoyal.Panels.Controllers
{
    public class GridFactory
    {
        private readonly GridPanelSettingsSO _gridPanelSettings;
        private readonly PanelSO[] _panelSettings;

        public GridFactory(PanelSO[] panelSettings)
        {
            this._panelSettings = panelSettings;
        }

        public (PanelModel panelModel, PanelView panelView)[] CreatePairs(Transform parent)
        {
            var grid = BlockMatrixGenerator.GenerateGrid(_gridPanelSettings.Rect);

            var polygon = BlockMatrixGenerator
                .GeneratePolygon(_gridPanelSettings.StartedPosition, _gridPanelSettings.Rect, _gridPanelSettings.Spacing);

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

            var panelModel = new PanelModel();

            panelModel.SetAvailable();

            var panelView = Object
                .Instantiate(panelSetting.PanelView, new Vector3(cell[0], parent.position.y, cell[1]), Quaternion.identity, parent);

            return (panelModel, panelView);
        }
    }
}