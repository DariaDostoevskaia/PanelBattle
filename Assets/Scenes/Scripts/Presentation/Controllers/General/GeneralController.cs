using LegoBattaleRoyal.Presentation.UI.General;

namespace LegoBattaleRoyal.Presentation.Controllers.General
{
    public class GeneralController
    {
        private readonly GeneralPopup _generalPopup;

        public GeneralController(GeneralPopup generalPopup)
        {
            this._generalPopup = generalPopup;
        }

        public void ShowAdsPopup(System.Action callback)
        {
            var showButton = _generalPopup.CreateButton("Show Ads");
            showButton.onClick.AddListener(() =>
            {
                showButton.interactable = false;
                callback?.Invoke();
            });

            _generalPopup.SetTitle("Not enough energy.");
            _generalPopup.SetText("There is not enough energy to buy the next level. Watch an advertisement to replenish energy.");

            _generalPopup.Show();
        }

        // TODO

        //var continueButton = generalPopup.CreateButton("Continue");
        //continueButton.onClick.AddListener(() =>
        //{
        //    continueButton.interactable = false;
        //    generalPopup.Close();
        //});

        //var exitButton = generalPopup.CreateButton("Exit");
        //exitButton.onClick.AddListener(() =>
        //{
        //    exitButton.interactable = false;
        //    generalPopup.Close();
        //    menuController.ShowMenu();
        //});

        //generalPopup.SetTitle("Economic and awards.");
        //generalPopup.SetText($"There are {_gameSettingsSO.Money} energy in your wallet. " +
        //    $"The reward for the next level is {level.Reward} energy.");

        //generalPopup.Show();

        //TODO
    }
}