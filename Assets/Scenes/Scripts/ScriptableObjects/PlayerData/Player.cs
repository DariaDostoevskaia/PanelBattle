using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects.PlayerData
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerData playerData;

        private void OnMouseDown()
        {
            Debug.Log(playerData.name);
            Debug.Log(playerData.Description);
            Debug.Log(playerData.Icon.name);
            Debug.Log(playerData.Speed);
            Debug.Log(playerData.JumpHeight);
        }
    }
}