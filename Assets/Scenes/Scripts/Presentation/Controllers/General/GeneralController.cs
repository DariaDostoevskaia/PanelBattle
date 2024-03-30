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

        public void ShowAdsPopup(Action callback)
        {
            var showButton = _generalPopup.CreateButton("Show Ads");
            showButton.onClick.AddListener(() =>
            {
                //showButton.interactable = false;
                callback?.Invoke();
            });

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

            _generalPopup.SetText($"There are {_walletController.GetCurrentMoney()} energy in your wallet");
            _generalPopup.Show();
        }

        //TODO DRY
        public void ShowWinLevelPopup(Action nextCallback, Action exitCallback)
        {
            _generalPopup.SetTitle("You Win!");
            var nextLevel = _levelRepository.GetNextLevel();
            var currentLevel = _levelRepository.GetCurrentLevel();

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

            _generalPopup.SetText($"You earn {currentLevel.Reward}.");
            _generalPopup.Show();
        }

        public void ShowWinGamePopup(Action restartCallback, Action exitCallback)
        {
            _generalPopup.SetTitle("You Won Game!");

            var currentLevel = _levelRepository.GetCurrentLevel();

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

            _generalPopup.SetText($"You earn {currentLevel.Reward}. Thanks for playing! Updates coming soon. Try the levels again?");
            _generalPopup.Show();
        }
    }
}