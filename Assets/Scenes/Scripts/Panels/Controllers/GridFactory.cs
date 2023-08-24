using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
            var lenght = _panelSettings.Length;
            var notJumpNumber = lenght;
            notJumpNumber = lenght * 30 / 100;

            var random = Random.Range(0, lenght);

            var panelSettings = _panelSettings;
            var panelSetting = panelSettings[random - notJumpNumber];
            var panelSettingNotJump = panelSettings[notJumpNumber];

            var panelModel = new PanelModel(panelSetting.IsJumpBlock);
            panelModel.SetAvailable();

            var panelModelNotJump = new PanelModel(panelSettingNotJump.IsJumpBlock);
            panelModelNotJump.SetUnavailable();

            var panelView = Object
                .Instantiate(panelSettings[lenght].PanelView, new Vector3(cell[0], parent.position.y, cell[1]), Quaternion.identity, parent);

            return (panelModel, panelView);
        }
    }
}