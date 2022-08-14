using System.Collections;
using UnityEngine;

namespace Common
{
    public static class TransformUtils
    {
        public static Transform SpawnChild(this Transform parent, string name)
        {
            var result = new GameObject(name).transform;
            result.parent = parent.transform;
            return result;
        }

        public static T SpawnChild<T>(this Transform parent, string name) where T: MonoBehaviour
        {
            return parent.SpawnChild(name).gameObject.AddComponent<T>();
        }
    }
}