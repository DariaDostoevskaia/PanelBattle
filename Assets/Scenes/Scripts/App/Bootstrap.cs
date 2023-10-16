using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private GameBootstrap _gameBootstrap;
        void Start()
        {
            _gameBootstrap.Configur();
        }

    }
}
