using System.Collections;
using UnityEngine;

namespace Common
{
    public static class TransformUtils
    {
        public static Transform SpawnChild(this Transform parent, string name)
        {
            var result = new GameObject("[ACTORS]").transform;
            result.parent = parent.transform;
            return result;
        }
    }
}