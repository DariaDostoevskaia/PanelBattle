using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.Base
{
    public abstract class BaseViewUI : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}