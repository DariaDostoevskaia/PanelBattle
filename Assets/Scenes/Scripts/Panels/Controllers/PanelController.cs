using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using LegoBattaleRoyal.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LegoBattaleRoyal.Panels.Controllers
{
    public class PanelController
    {
        private PanelView _panelView;
        private PanelModel _panelModel;
        private Vector3 _panelViewPosition;

        public PanelController(PanelModel panelModel, PanelView panelView, Vector3 panelViewPosition)
        {
            _panelModel = panelModel;
            _panelView = panelView;
            _panelViewPosition = panelViewPosition;
        }
    }
}