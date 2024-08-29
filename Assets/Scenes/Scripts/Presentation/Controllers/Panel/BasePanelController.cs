using LegoBattaleRoyal.Core.Characters.Models;
using LegoBattaleRoyal.Core.Panels.Models;
using LegoBattaleRoyal.Presentation.GameView.Panel;
using System.Collections.Generic;
using System.Linq;

namespace LegoBattaleRoyal.Presentation.Controllers.Panel
{
    public class BasePanelController
    {
        private readonly (PanelModel panelModel, PanelView panelView)[] _pairs;
        private readonly List<CharacterModel> _players;
        private readonly int[] _rect;

        private const int _minX = 0;
        private const int _minY = 0;
        private int _maxX;
        private int _maxY;

        public BasePanelController(
            (PanelModel panelModel, PanelView panelView)[] pairs, List<CharacterModel> players, int[] rect)
        {
            _pairs = pairs;
            _players = players;
            _rect = rect;

            _maxX = _rect[0] - 1;
            _maxY = _rect[1] - 1;
        }

        private List<(PanelModel panelModel, PanelView panelView)> GetPerimeterBlocksList()
        {
            // Извлечение list блоков по периметру из массива pairs

            var perimeterBlocksList = _pairs
                .Where(pair => pair.panelModel.GridPosition.Row == _minX
                || pair.panelModel.GridPosition.Row == _maxX
                || pair.panelModel.GridPosition.Column == _minY
                || pair.panelModel.GridPosition.Column == _maxY)
                .ToList();

            // Очистка блоков по углам
            perimeterBlocksList.RemoveAll(pair =>
            (pair.panelModel.GridPosition.Row == _minX && pair.panelModel.GridPosition.Column == _minY)
            || (pair.panelModel.GridPosition.Row == _minX && pair.panelModel.GridPosition.Column == _maxY)
            || (pair.panelModel.GridPosition.Row == _maxY && pair.panelModel.GridPosition.Column == _maxY)
            || (pair.panelModel.GridPosition.Row == _maxX && pair.panelModel.GridPosition.Column == _minY));

            return perimeterBlocksList;
        }

        public List<(int row, int column)> GetBaseBlocks()
        {
            var perimeterBlocksList = GetPerimeterBlocksList();

            // Выделение 4 сторон
            var leftSide = perimeterBlocksList
                .Where(pair => pair.panelModel.GridPosition.Row == _minX)
                .ToList();

            var rightSide = perimeterBlocksList
                .Where(pair => pair.panelModel.GridPosition.Row == _maxX)
                .ToList();

            var topSide = perimeterBlocksList
                .Where(pair => pair.panelModel.GridPosition.Column == _minY)
                .ToList();

            var bottomSide = perimeterBlocksList
                .Where(pair => pair.panelModel.GridPosition.Column == _maxX)
                .ToList();

            // Количество игроков и распределение по сторонам
            var playersCount = _players.Count;
            var sides = 4;
            var x = playersCount / sides;
            var y = playersCount % sides;

            // Расстановка игроков на сторонах
            //var sideLists = new List<List<(PanelModel panelModel, PanelView panelView)>>()
            //{leftSide, rightSide, topSide, bottomSide };

            // Алгоритм расстановки игроков на равной отдаленности
            var playerCoordinates = new List<(int row, int column)>(_players.Count);

            if (y > 0)
            {
                for (int i = 0; i < x; i++)
                {
                    playerCoordinates
                        .Add((leftSide[i].panelModel.GridPosition.Row, leftSide[i].panelModel.GridPosition.Column));

                    playerCoordinates
                        .Add((rightSide[i].panelModel.GridPosition.Row, rightSide[i].panelModel.GridPosition.Column));

                    playerCoordinates
                        .Add((topSide[i].panelModel.GridPosition.Row, topSide[i].panelModel.GridPosition.Column));

                    playerCoordinates
                        .Add((bottomSide[i].panelModel.GridPosition.Row, bottomSide[i].panelModel.GridPosition.Column));
                }
            }
            if (y == 0)
            {
                // Дополнительное распределение игроков по сторонам, если есть остаток
                for (int i = 0; i < y; i++)
                {
                    playerCoordinates
                        .Add((leftSide[x + i].panelModel.GridPosition.Row, leftSide[x + i].panelModel.GridPosition.Column));

                    playerCoordinates
                        .Add((rightSide[x + i].panelModel.GridPosition.Row, rightSide[x + i].panelModel.GridPosition.Column));

                    playerCoordinates
                        .Add((topSide[x + i].panelModel.GridPosition.Row, topSide[x + i].panelModel.GridPosition.Column));

                    playerCoordinates
                        .Add((bottomSide[x + i].panelModel.GridPosition.Row, bottomSide[x + i].panelModel.GridPosition.Column));
                }
            }

            return playerCoordinates;
        }
    }
}