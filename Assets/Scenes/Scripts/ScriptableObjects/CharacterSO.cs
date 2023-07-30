using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(CharacterSO), menuName = "Config/Character SO")]
    public class CharacterSO : ScriptableObject
    {
        [SerializeField] private float _moveDuration = 3f;

        public float MoveDuration => _moveDuration;
        //[SerializeField]
        //public string JumpHeight
        //{
        //    get
        //    {
        //        return JumpHeight;
        //    }
        //}
    }
}