using LegoBattaleRoyal.Presentation.UI.TopbarPanel;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Topbar
{
    public class TopbarController : IDisposable
    {
        private readonly TopbarScreenPanel _topbarPopup;
        private readonly SettingsController _settingsController;

        public TopbarController(TopbarScreenPanel topbarPopup, SettingsController settingsController)
        {
            _topbarPopup = topbarPopup;
            _settingsController = settingsController;

            _topbarPopup.OnSettingsButtonClicked += OnSettingsButtonClicked;
        }

        public void OnSettingsButtonClicked()
        {
            _settingsController.ShowSettings();
        }

        public void ShowTopbar()
        {
            _topbarPopup.Show();
        }

        public void Dispose()
        {
            _topbarPopup.OnSettingsButtonClicked -= OnSettingsButtonClicked;
        }
    }
}