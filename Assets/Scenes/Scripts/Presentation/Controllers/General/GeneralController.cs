using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.Presentation.Controllers.Wallet;
using LegoBattaleRoyal.Presentation.UI.General;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.General
{
    public class GeneralController
    {
        private readonly GeneralPopup _generalPopup;
        private readonly WalletController _walletController;
        private readonly ILevelRepository _levelRepository;

        public GeneralController(GeneralPopup generalPopup, WalletController walletController, ILevelRepository levelRepository)
        {
            _generalPopup = generalPopup;
            _walletController = walletController;
            _levelRepository = levelRepository;
        }

        public void ShowRefinementRemovePanel(Action callback)
        {
            _generalPopup.SetTitle("Remove progress");

            var removeProgressButton = _generalPopup.CreateButton("YES");
            removeProgressButton.onClick.AddListener(() =>
            {
                removeProgressButton.interactable = false;
                callback?.Invoke();
                _generalPopup.Close();
            });

            var nonRemoveButton = _generalPopup.CreateButton("NO");
            nonRemoveButton.onClick.AddListener(() =>
            {
                nonRemoveButton.interactable = false;
                _generalPopup.Close();
            });
            _generalPopup.SetActiveCloseButton(true);

            _generalPopup.SetText("This process is irreversible, and it will not be possible to restore it later. " +
                "Are you sure you want to delete all current progress?");
            _generalPopup.Show();
        }

        public void ShowAdsPopup(Func<UniTask<bool>> func)
        {
            var showButton = _generalPopup.CreateButton("Show Ads");
            showButton.onClick.AddListener(async () =>
            {
                func ??= () => UniTask.FromResult(false);
                var result = await func.Invoke();

                showButton.interactable = !result;
            });

            _generalPopup.SetActiveCloseButton(true);

            _generalPopup.SetTitle("Not enough energy.");
            _generalPopup.SetText("There is not enough energy to buy the next level. Watch an advertisement to replenish energy.");

            _generalPopup.Show();
        }

        public void ShowLosePopup(Action restartCallback, Action exitCallback)
        {
            _generalPopup.SetTitle("You Lose!");
            var currentLevel = _levelRepository.GetCurrentLevel();

            var restartButton = _generalPopup.CreateButton($"Restart for {currentLevel.Price}");
            restartButton.onClick.AddListener(() =>
            {
                restartButton.interactable = false;
                _generalPopup.Close();
                restartCallback?.Invoke();
            });

            var exitButton = _generalPopup.CreateButton("Exit");
            exitButton.onClick.AddListener(() =>
            {
                exitButton.interactable = false;
                _generalPopup.Close();
                exitCallback?.Invoke();
            });
            _generalPopup.SetActiveCloseButton(false);

            _generalPopup.SetText($"There are {_walletController.GetCurrentMoney()} energy in your wallet");
            _generalPopup.Show();
        }

        public GeneralPopup CreatePopup(string title, string text)
        {
            _generalPopup.SetTitle(title);
            _generalPopup.SetText(text);
            return _generalPopup;
        }
    }
}