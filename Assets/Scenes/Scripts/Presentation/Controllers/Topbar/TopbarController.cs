using LegoBattaleRoyal.Presentation.Controllers.Wallet;
using LegoBattaleRoyal.Presentation.UI.TopbarPanel;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.Topbar
{
    public class TopbarController : IDisposable
    {
        public event Action OnButtonClicked;

        private readonly TopbarScreenPanel _topbarPopup;
        private readonly WalletController _walletController;

        public TopbarController(TopbarScreenPanel topbarPopup, WalletController walletController)
        {
            _topbarPopup = topbarPopup;
            _walletController = walletController;

            _topbarPopup.OnSettingsButtonClicked += ButtonClicked;
            _walletController.Changed += SetCount;
        }

        private void SetCount(int count)
        {
            _topbarPopup.SetText(count);
        }

        public void ShowTopbar()
        {
            _topbarPopup.Show();
        }

        private void ButtonClicked()
        {
            OnButtonClicked?.Invoke();
        }

        public void Dispose()
        {
            OnButtonClicked = null;

            _topbarPopup.OnSettingsButtonClicked -= ButtonClicked;
            _walletController.Changed -= SetCount;
        }
    }
}