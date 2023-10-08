using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace LegoBattaleRoyal.UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private Button _endGameButton;
        [SerializeField] private Button _returnGameButton;

        private void Start()
        {
            //_returnGameButton.onClick.AddListener(ReturnGame);
            //_endGameButton.onClick.AddListener(EndGame);
        }

        private void ReturnGame()
        {
            //_startGameButton.interactable = false;
        }

        private void EndGame()
        {
            //_endGameButton.interactable = false;
            Application.Quit();
        }

        private void OnDestroy()
        {
            //_returnGameButton.onClick.RemoveAllListeners();
            //_endGameButton.onClick.RemoveAllListeners();
        }
    }
}
