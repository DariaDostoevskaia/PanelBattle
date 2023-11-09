using Cinemachine;
using UnityEngine;

namespace LegoBattaleRoyal.Extensions
{
    public class TouchScreenCameraControl : MonoBehaviour
    {
        public CinemachineFreeLook freeLookCamera;
        public float rotationSpeed = 2.0f;

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    freeLookCamera.m_XAxis.Value += touch.deltaPosition.x * rotationSpeed;
                    freeLookCamera.m_YAxis.Value -= touch.deltaPosition.y * rotationSpeed;
                }
            }
        }
    }
}