using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects.PlayerData
{
    [CreateAssetMenu(fileName = "New PlayerData", menuName = "Player Data", order = 51)]
    public class PlayerData : ScriptableObject
    {
        [SerializeField]
        public string SwordName
        {
            get
            {
                return SwordName;
            }
        }

        [SerializeField]
        public string Description
        {
            get
            {
                return Description;
            }
        }

        [SerializeField]
        public Sprite Icon
        {
            get
            {
                return Icon;
            }
        }

        [SerializeField]
        public string Speed
        {
            get
            {
                return Speed;
            }
        }

        [SerializeField]
        public string JumpHeight
        {
            get
            {
                return JumpHeight;
            }
        }
    }
}