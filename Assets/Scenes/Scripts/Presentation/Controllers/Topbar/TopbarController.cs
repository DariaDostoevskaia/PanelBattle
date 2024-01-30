using LegoBattaleRoyal.Presentation.UI.TopbarPanel;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Topbar
{
    public class TopbarController : IDisposable
    {
        public event Action OnButtonClicked;

        private readonly TopbarScreenPanel _topbarPopup;

        public TopbarController(TopbarScreenPanel topbarPopup)
        {
            _topbarPopup = topbarPopup;

            _topbarPopup.OnSettingsButtonClicked += OnButtonClicked;
        }

        public void ShowTopbar()
        {
            _topbarPopup.Show();
        }

        public void Dispose()
        {
            OnButtonClicked = null;
            _topbarPopup.OnSettingsButtonClicked -= OnButtonClicked;
        }
    }
}