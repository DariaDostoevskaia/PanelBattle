using LegoBattaleRoyal.Presentation.Controllers.Wallet;
using LegoBattaleRoyal.Presentation.UI.TopbarPanel;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Topbar
{
    public class TopbarController : IDisposable
    {
        private readonly TopbarScreenPanel _topbarPopup;
        private readonly SettingsController _settingsController;
        private readonly WalletController _walletController;

        public TopbarController(TopbarScreenPanel topbarPopup, SettingsController settingsController, WalletController walletController)
        {
            _topbarPopup = topbarPopup;
            _settingsController = settingsController;
            _walletController = walletController;

            _topbarPopup.OnSettingsButtonClicked += OnSettingsButtonClicked;
            _walletController.Changed += SetCount;
        }

        public void OnSettingsButtonClicked()
        {
            _settingsController.ShowSettings();
        }
        
        private void SetCount(int count)
        {
            _topbarPopup.SetText(count);
        }

        public void ShowTopbar()
        {
            _topbarPopup.Show();
        }

        public void Dispose()
        {
            _topbarPopup.OnSettingsButtonClicked -= OnSettingsButtonClicked;
            _walletController.Changed -= SetCount;
        }
    }
}