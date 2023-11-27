using LegoBattaleRoyal.Presentation.UI.TopbarPanel;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Topbar
{
    public class TopbarController : IDisposable
    {
        private readonly SettingsController _settingsController;
        private readonly TopbarScreenPanel _topbarPopup;

        public TopbarController(SettingsController settingsController, TopbarScreenPanel topbarPopup)
        {
            _settingsController = settingsController;
            _topbarPopup = topbarPopup;

            _topbarPopup.OnOpenSettings += OpenSettingsPopup;
        }

        public void ShowTopbar()
        {
            _topbarPopup.Show();
        }

        public void OpenSettingsPopup()
        {
            _settingsController.ShowSettings();
        }

        public void Dispose()
        {
            _topbarPopup.OnOpenSettings -= OpenSettingsPopup;
        }
    }
}