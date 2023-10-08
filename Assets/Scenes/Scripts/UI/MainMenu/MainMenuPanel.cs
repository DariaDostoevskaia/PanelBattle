using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace LegoBattaleRoyal.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _endGameButton;

        private void Start()
        {
            //_startGameButton.onClick.AddListener(StartNewGame);
            //_endGameButton.onClick.AddListener(EndGame);

        }

        private void StartNewGame()
        {
            //_startGameButton.interactable = false;
            //SceneManager.LoadScene(0);
        }

        private void EndGame()
        {
            //_endGameButton.interactable = false;
            //Application.Quit();
        }

        private void OnDestroy()
        {
            //_startGameButton.onClick.RemoveAllListeners();
            //_endGameButton.onClick.RemoveAllListeners();
        }
    }
}
