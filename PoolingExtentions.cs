using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

namespace ObjectPooling
{

    public static class PoolingExtentions
    {

        public static GameObject PooledSpawn(this GameObject prefabToSpawn, Vector3 pos, Quaternion rot)
        {
            if (prefabToSpawn == null)
                return null;

            if (ObjectPoolingManager._instances == null)
            {
                Debug.LogWarning("The pooling manager is not properly instanced");
                return null;

            }

            ObjectPool pool = ObjectPoolingManager.GetMatchingPool(prefabToSpawn);
            if (pool == null)
            {
                Debug.LogWarning("No object pools of type " + prefabToSpawn.name);

                return GameObject.Instantiate(prefabToSpawn, pos, rot);

              
            }

            return pool.SpawnObjectFromPool(pos, rot);

             

        }

        public static void PooledDespawn(this GameObject gameObject)
        {
            if (gameObject == null || gameObject.activeSelf == false)
                return;

            if (ObjectPoolingManager._instances == null)
            {
                Debug.LogWarning("The pooling manager is not properly instanced"); 

                return;

            }

            ObjectPool pool = ObjectPoolingManager.GetMatchingPool(gameObject);
            if (pool == null)
            {
                Debug.LogWarning("No object pools of type " + gameObject.name);

                GameObject.Destroy(gameObject);
                return;
            }

            pool.PoolObjectBack(gameObject);

        }

    }
}