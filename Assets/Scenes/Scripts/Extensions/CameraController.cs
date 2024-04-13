using Cinemachine;
using UnityEngine.EventSystems;

namespace LegoBattaleRoyal.Extensions
{
    public class CameraController
    {
        private PhysicsRaycaster _raycaster;
        private CinemachineBrain _cinemachine;

        public CameraController(PhysicsRaycaster raycaster, CinemachineBrain cinemachine)
        {
            _raycaster = raycaster;
            _cinemachine = cinemachine;
        }

        public void ShowRaycaster()
        {
            _raycaster.enabled = true;
            _cinemachine.enabled = true;
        }

        public void CloseRaycaster()
        {
            _raycaster.enabled = false;
            _cinemachine.enabled = false;
        }
    }
}