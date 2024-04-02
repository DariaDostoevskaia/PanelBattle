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
            _generalPopup.CloseEnergyContainer();
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

        public void ShowAdsPopup(Action callback)
        {
            _generalPopup.CloseEnergyContainer();
            var showButton = _generalPopup.CreateButton("Show Ads");

            showButton.onClick.AddListener(() =>
            {
                showButton.interactable = false;
                callback?.Invoke();
            });
            _generalPopup.SetActiveCloseButton(true);

            _generalPopup.SetTitle("No energy.");
            _generalPopup.SetText("There is not enough energy to buy the next level. Watch an advertisement to replenish energy.");

            _generalPopup.Show();
        }

        public void ShowLosePopup(Action restartCallback, Action exitCallback)
        {
            _generalPopup.SetTitle("You Lose!");

            var currentLevel = _levelRepository.GetCurrentLevel();

            _generalPopup.SetEnergyCount(currentLevel.Price);

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

        //TODO DRY
        public void ShowWinLevelPopup(Action nextCallback, Action exitCallback)
        {
            _generalPopup.SetTitle("You Win!");

            var nextLevel = _levelRepository.GetNextLevel();
            var currentLevel = _levelRepository.GetCurrentLevel();
            _generalPopup.SetEnergyCount(currentLevel.Reward);

            var nextButton = _generalPopup.CreateButton($"Next for {nextLevel.Price}");
            nextButton.onClick.AddListener(() =>
            {
                nextButton.interactable = false;
                _generalPopup.Close();
                nextCallback?.Invoke();
            });

            var exitButton = _generalPopup.CreateButton("Exit");
            exitButton.onClick.AddListener(() =>
            {
                exitButton.interactable = false;
                _generalPopup.Close();
                exitCallback?.Invoke();
            });
            _generalPopup.SetActiveCloseButton(false);

            _generalPopup.SetText($"You earn {currentLevel.Reward}.");
            _generalPopup.Show();
        }

        public void ShowWinGamePopup(Action restartCallback, Action exitCallback)
        {
            _generalPopup.SetTitle("You Won Game!");

            var currentLevel = _levelRepository.GetCurrentLevel();
            _generalPopup.SetEnergyCount(currentLevel.Reward);

            var restartGameButton = _generalPopup.CreateButton($"Restart Game");
            restartGameButton.onClick.AddListener(() =>
            {
                restartGameButton.interactable = false;
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

            _generalPopup.SetText($"You earn {currentLevel.Reward}. Thanks for playing! Updates coming soon. Try the levels again?");
            _generalPopup.Show();
        }
    }
}