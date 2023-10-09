using LegoBattaleRoyal.App;
using LegoBattaleRoyal.UI.Container;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameBootstrap _gameBootstrap;
    [SerializeField] private UIContainer _uIContainer;

    void Start()
    {
        _uIContainer.MenuPanel.Show();
        _uIContainer.MenuPanel.OnStartGameClicked += StartGame;
    }

    private void StartGame()
    {
        _gameBootstrap.Dispose();
        // subscribe again after dispose

        _gameBootstrap.OnRestarted += StartGame;

        _uIContainer.MenuPanel.Close();

        _gameBootstrap.Configure();
    }

    private void OnDestroy()
    {
        _uIContainer.MenuPanel.OnStartGameClicked -= StartGame;

        _gameBootstrap.OnRestarted -= StartGame;
    }
}
