using LegoBattaleRoyal.App;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameBootstrap _gameBootstrap;
    void Start()
    {
        _gameBootstrap.Configur();
    }

}
