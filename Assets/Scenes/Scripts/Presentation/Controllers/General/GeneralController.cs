using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Presentation.Controllers.Wallet;
using LegoBattaleRoyal.Presentation.UI.General;
using System;

namespace LegoBattaleRoyal.Presentation.Controllers.General
{
    public class GeneralController : IDisposable
    {
        private readonly GeneralPopup _generalPopup;
        private readonly WalletController _walletController;
        private readonly ILevelRepository _levelRepository;
        private readonly CameraController _cameraController;

        public GeneralController
            (GeneralPopup generalPopup,
            WalletController walletController,
            ILevelRepository levelRepository,
            CameraController cameraController)
        {
            _generalPopup = generalPopup;
            _walletController = walletController;
            _levelRepository = levelRepository;
            _cameraController = cameraController;

            _generalPopup.Shown += _cameraController.CloseRaycaster;
            _generalPopup.Closed += _cameraController.ShowRaycaster;
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

        public void ShowAdsPopup(Func<UniTask<bool>> func)
        {
            _generalPopup.CloseEnergyContainer();
            var showButton = _generalPopup.CreateButton("Show Ads");
            showButton.onClick.AddListener(async () =>
            {
                func ??= () => UniTask.FromResult(false);
                var result = await func.Invoke();

                showButton.interactable = !result;
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

        public GeneralPopup CreatePopup(string title, string text)
        {
            _generalPopup.SetTitle(title);
            _generalPopup.SetText(text);
            return _generalPopup;
        }

        public void Dispose()
        {
            _generalPopup.Shown -= _cameraController.CloseRaycaster;
            _generalPopup.Closed -= _cameraController.ShowRaycaster;
        }
    }
}