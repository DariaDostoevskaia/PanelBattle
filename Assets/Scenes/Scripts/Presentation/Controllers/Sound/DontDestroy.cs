using UnityEngine;

namespace LegoBattaleRoyal.Presentation.Controllers.Sound
{
    public class DontDestroy : MonoBehaviour
    {
        public static DontDestroy _instance;

        private void Start()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}