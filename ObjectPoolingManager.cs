using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPoolingManager : MonoBehaviour
    {
        public static List< ObjectPoolingManager> _instances { get; protected set; } 
        public List<ObjectPool> _objectPools { get; protected set; }

        [Header("Manager Behaviour")]
        [SerializeField]
        bool dontDestroyOnLoadFlag = false; 

        [Header("Object Pools Profiles")]
        public List<ObjectPoolProfile> objectPoolProfiles;


        bool poolsInitialised = false;

        private void Awake()
        {
            if (poolsInitialised == false)
                StartCoroutine(InitialisationRoutine());

            if (dontDestroyOnLoadFlag == true)
                DontDestroyOnLoad(this.gameObject);

            if (_instances == null)
                _instances = new List<ObjectPoolingManager>();

            if (_instances.Contains(this) == false)
                _instances.Add(this);
        }


        IEnumerator InitialisationRoutine()
        {
            _objectPools = new List<ObjectPool>();

            GameObject poolsParent = new GameObject("pooledObjectContainer");
            for (int i = 0; i < objectPoolProfiles.Count; i++)
            {
                for(int j = 0; j <objectPoolProfiles[i].pools.Length;j++)

                _objectPools.Add(new ObjectPool(objectPoolProfiles[i].pools[j], poolsParent.transform));

            }


            yield return null;
            poolsInitialised = true;
        }

        private void OnDestroy()
        {
            if (_instances.Contains(this) == false)
                _instances.Remove(this);
        }


        public static ObjectPool GetMatchingPool(GameObject prefab)
        {
            for (int i = 0; i < _instances.Count; i++)
            {
                for (int j = 0; j < _instances[i]._objectPools.Count; j++)
                {
                    if (prefab.name.Equals(_instances[i]._objectPools[j]._poolData.prefab.name))
                        return _instances[i]._objectPools[j];

                }

            }
             
            return null;
        }
         
    }

}