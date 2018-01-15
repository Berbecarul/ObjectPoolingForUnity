using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Core.Pools
{

    public static class Extension
    {
        /// <summary>
        /// Pooled spawn from the object pool, if there is no pool, it will create a new instance, also it will move the
        /// object first then enable it
        /// </summary>
        /// <param name="prefabToSpawn">The game object prefab</param>
        /// <param name="pos">Position in world space</param>
        /// <param name="rot">Rotation in world space</param>
        /// <returns>GameObject component of the spawned object</returns>
        public static GameObject PooledSpawn(this GameObject prefabToSpawn, Vector3 pos, Quaternion rot)
        {
            if (prefabToSpawn == null)
                return null;
            if (PoolsManager._instance != null)
            {
                ObjectPool targetPool =
                    PoolsManager._instance.pools.Where(pool => pool._poolData.prefab == prefabToSpawn).First();

                if (targetPool != null)
                {
                    GameObject obj = targetPool.RetrievePooledObject(pos, rot);


                    if (obj != null)
                    {
                        return obj;
                    }
                    else
                    {
                        Debug.Log("No more " + targetPool._poolData.prefab.name + " objects available");
                        return null;
                    }


                }
                else
                {
                    Debug.LogWarning("Invalid target pool of type: " + prefabToSpawn.name);
                    GameObject.Instantiate(prefabToSpawn, pos, rot);
                }

            }
            Debug.Log("No PoolsManager object Found");
            return null;
        }





        /// <summary>
        /// Despawns a previously spawned object to be recycled if possible
        /// </summary>
        /// <param name="obj">GameObject to despawn</param>
        public static void PooledDespawn(this GameObject obj)
        {
            if (obj == null)
                return;
            if (PoolsManager._instance != null)
            {
                ObjectPool targetPool =
                     PoolsManager._instance.pools.Where(pool => pool._spawnedPool.Contains(obj)).FirstOrDefault();

                if (targetPool != null)
                    targetPool.PoolObjectBack(obj);
                else
                {
                    Debug.LogWarning("No Suitable pool object Found");
                    obj.SetActive(false);
                }
                return;

            }
            Debug.Log("No PoolsManager object Found");

        }
    }

   
    
    /// <summary>
    /// Object pooling component
    /// </summary>
    public class PoolsManager : MonoBehaviour
    {
        public static PoolsManager _instance { get; protected set; }

        public List<ObjectPool> pools = new List<ObjectPool>();

        public List<ObjectPoolProfile> profiles = new List<ObjectPoolProfile>();


        GameObject poolsRootContainter;


        private void Awake()
        {
            if (_instance)
                return;
            else
                _instance = this;


            InitialisePools();

        }
         
        void InitialisePools()
        {
            poolsRootContainter = new GameObject
            {
                name = "_RootPoolsContainer"
            };
            poolsRootContainter.transform.SetParent(null);

            
            for (int i = 0; i < profiles.Count; i++)
            {

                for (int j = 0; j < profiles[i].data.Length; j++)
                {
                    GameObject poolContainer = new GameObject(profiles[i].data[j].prefab.name+"_Pool");
                    poolContainer.transform.SetParent(poolsRootContainter.transform);


                    ObjectPool pool = new ObjectPool(profiles[i].data[j], poolContainer.transform);
                    pools.Add(pool);
                }
                 
            } 

        } 

    }



}
