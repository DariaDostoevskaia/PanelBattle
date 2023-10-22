using UnityEngine;

namespace LegoBattaleRoyal.Exceptions
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// using null-conditional and null-coalescing operator won't work on Unity objects because it "bypasses the lifetime check on the underlying Unity engine object"
        /// </summary>
        /// https://forum.unity.com/threads/c-null-coalescing-operator-does-not-work-for-unityengine-object-types.543484/

        public static T Value<T>(this T component) where T : Object
        {
            return component
                ? component
                : null;
        }

        public static void DestroyGameObject<T>(this T component) where T : MonoBehaviour
        {
            if (component.Value())
                Object.Destroy(component.gameObject);
        }
    }
}

