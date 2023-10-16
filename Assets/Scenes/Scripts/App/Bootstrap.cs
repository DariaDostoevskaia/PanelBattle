using LegoBattaleRoyal.App;
using LegoBattaleRoyal.Controllers.EndGame;
using LegoBattaleRoyal.UI.Container;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameBootstrap _gameBootstrap;
    [SerializeField] private UIContainer _uIContainer;
    [SerializeField] private EndGameController _endGameController;

    private void Start()
    {
        _uIContainer.MenuPanel.Show();
        _uIContainer.MenuPanel.OnStartGameClicked += StartGame;

        _uIContainer.GamePanel.OnExitMainMenuClicked += ExitMainMenu;
    }

    private void StartGame()
    {
        _gameBootstrap.Dispose();
        // subscribe again after dispose

        _gameBootstrap.OnRestarted += StartGame;

        _uIContainer.MenuPanel.Close();

        _gameBootstrap.Configure();
    }

    private void ExitMainMenu()
    {
        _gameBootstrap.OnExited += ExitMainMenu;

        _uIContainer.GamePanel.Close();
        _uIContainer.MenuPanel.Show();
    }

    private void OnDestroy()
    {
        _uIContainer.MenuPanel.OnStartGameClicked -= StartGame;
        _gameBootstrap.OnRestarted -= StartGame;

        _uIContainer.GamePanel.OnExitMainMenuClicked -= ExitMainMenu;
        _gameBootstrap.OnExited -= ExitMainMenu;
    }
}