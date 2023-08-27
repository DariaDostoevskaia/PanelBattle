using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using LegoBattaleRoyal.ScriptableObjects;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LegoBattaleRoyal.Panels.Controllers
{
    public class GridFactory
    {
        private readonly PanelSO[] _panelSettings;
        private int _count = 0;

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

            //_count = pairs.Length * 30 / 100;
            //var random = Random.Range(0, _count);

            return pairs;
        }

        private (PanelModel panelModel, PanelView panelView) CreatePair(float[] cell, Transform parent)
        {
            //var lenght = _panelSettings.Length;
            //var notJumpCountPairs = lenght * 30 / 100;

            //var randomNotJump = Random.Range(0, notJumpCountPairs);

            var panelSettings = _panelSettings.Length;

            var randomIsJump = Random.Range(0, panelSettings);

            var panelSettingIsJump = _panelSettings[randomIsJump];
            //var panelSettingNotJump = _panelSettings[randomNotJump];

            //var panelModel = new PanelModel(panelSetting.IsJumpBlock);

            var panelModelIsJump = new PanelModel(panelSettingIsJump.IsJumpBlock);

            panelModelIsJump.SetAvailable();
            //panelModelNotJump.SetUnavailable();

            //var panelViewIsJump = Object
            //    .Instantiate(panelSettingIsJump.PanelView,
            //    new Vector3(cell[0], parent.position.y, cell[1]),
            //    Quaternion.identity, parent);

            //var panelViewNotJump = Object
            //    .Instantiate(panelSettingNotJump.PanelView,
            //    new Vector3(cell[0], parent.position.y, cell[1]),
            //    Quaternion.identity, parent);

            var panelView = Object
               .Instantiate(panelSettingIsJump.PanelView,
               new Vector3(cell[0], parent.position.y, cell[1]),
               Quaternion.identity,
               parent);

            return (panelModelIsJump, panelView);
        }
    }
}