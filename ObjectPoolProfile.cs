
using System.Collections.Generic;
using UnityEngine;


namespace Core.Pools
{
    [CreateAssetMenu(fileName = "ObjectPoolProfile", menuName = "Custom Assets/ObjectPoolProfile", order = 1)]
    [System.Serializable]
    public class ObjectPoolProfile : ScriptableObject
    {
        public ObjectPoolData[] data;
    }

    public class ObjectPool
    {


        public ObjectPoolData _poolData { get; protected set; }

        public List<GameObject> _spawnedPool { get; protected set; }
        public List<GameObject> _availablePool { get; protected set; }

        public Transform container;

        public ObjectPool(ObjectPoolData poolData, Transform container)
        {
            _poolData = poolData;
            this.container = container;

            _spawnedPool = new List<GameObject>();
            _availablePool = new List<GameObject>();



            for (int i = 0; i < poolData.poolStartSize; i++)
            {
                GameObject obj = GameObject.Instantiate(poolData.prefab, container);
                obj.SetActive(false);
                obj.name = _poolData.prefab.name;
                _availablePool.Add(obj);

            }

        }


        public GameObject RetrievePooledObject(Vector3 position, Quaternion rotation)
        {



            if (_availablePool.Count > 0)
            {
                GameObject pObj;
                pObj = _availablePool[_availablePool.Count - 1];
                _spawnedPool.Add(pObj);
                _availablePool.RemoveAt(_availablePool.Count - 1);



                pObj.transform.SetParent(null);
                pObj.transform.rotation = rotation;
                pObj.transform.position = position;


                pObj.SetActive(true);

                return pObj;

            }
            else
            {
                GameObject pObj;
                switch (_poolData.emptyPoolHandling)
                {
                    case EmptyPoolHandling.none:
                        break;

                    case EmptyPoolHandling.recycle:
                        pObj = _spawnedPool[0];
                        pObj.SetActive(false);

                        _spawnedPool.RemoveAt(0);
                        _spawnedPool.Add(pObj);

                        pObj.transform.SetParent(null);
                        pObj.transform.rotation = rotation;
                        pObj.transform.position = position;
                        pObj.name = _poolData.prefab.name;

                        pObj.SetActive(true);

                        return pObj;

                    case EmptyPoolHandling.expand:

                        GameObject obj = GameObject.Instantiate(_poolData.prefab, container);
                        obj.name = _poolData.prefab.name;
                        _availablePool.Add(obj);
                        return RetrievePooledObject(position, rotation);

                    default:
                        return null;

                }

            }
            return null;
        }

        public void PoolObjectBack(GameObject obj)
        {
            if (_spawnedPool.Contains(obj))
            {
                obj.SetActive(false);
                _spawnedPool.Remove(obj);
                _availablePool.Add(obj);
                obj.transform.SetParent(container);
            }
            else
            {



                switch (_poolData.emptyPoolHandling)
                {
                    case EmptyPoolHandling.none:
                        obj.SetActive(false);
                        break;

                    case EmptyPoolHandling.recycle:
                        GameObject.Destroy(obj);


                        break;
                    case EmptyPoolHandling.expand:
                        obj.SetActive(false);
                        obj.transform.SetParent(container);
                        _availablePool.Add(obj);
                        obj.name = _poolData.prefab.name;
                        break;

                    default:
                        break;




                }

            }
        }


    }
     
    [System.Serializable]
    public struct ObjectPoolData
    {
        [Tooltip("The GameObject prefab of the pool")]
        public GameObject prefab;
        [Tooltip("Initial pool capacity")]
        [Range(1, 100)]
        public int poolStartSize;
        [Tooltip("How will the pool react when empty")]
        public EmptyPoolHandling emptyPoolHandling;

    }

    public enum EmptyPoolHandling
    {
        none, recycle, expand
    }


}