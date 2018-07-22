using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{


    [CreateAssetMenu(fileName = "ObjectPoolProfile", menuName = "Custom Assets/Object Pool Profile")]
    public class ObjectPoolProfile : ScriptableObject
    {
        public ObjectPoolData[] pools;

    }



    [System.Serializable]
    public struct ObjectPoolData
    {
        [Tooltip("The GameObject prefab of the pool")]
        public GameObject prefab;

        [Tooltip("Initial pool capacity")]
        [Range(1, 100)]
        public int poolStartSize;

        [Tooltip("Allow Expand")]
        public bool allowExpand;

    }

  
}
