using UnityEngine;
using UnityEngine.EventSystems;

namespace LegoBattaleRoyal.Extensions
{
    public class CameraController
    {
        private PhysicsRaycaster _raycaster;

        public CameraController(Camera camera)
        {
            _raycaster = camera.GetComponent<PhysicsRaycaster>();
        }

        public void ShowRaycaster()
        {
            _raycaster.gameObject.SetActive(true);
        }

        public void CloseRaycaster()
        {
            _raycaster.gameObject.SetActive(false);
        }
    }
}