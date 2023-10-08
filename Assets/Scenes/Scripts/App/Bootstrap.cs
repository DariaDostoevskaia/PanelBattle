using LegoBattaleRoyal.App;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameBootstrap _gameBootstrap;

    void Start()
    {
        //uiConteiner.menu controller.OnStartGame += StartGame
    }

    void StartGame()
    {
        _gameBootstrap.Dispose();
        // subscribe again after dispose
        _gameBootstrap.OnRestarted += StartGame;
        _gameBootstrap.Configure();
    }
}
