using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{

    public class ObjectPool  
    {
        public ObjectPoolData _poolData { get; protected set; }

        public List<GameObject> _pool { get; protected set; }
        
        
        Transform parent;

        public ObjectPool(ObjectPoolData poolData, Transform parent)
        {
            //init
            _poolData = poolData;
            this.parent = parent;

            _pool = new List<GameObject>();

            //instantiate things

            for (int i = 0; i < poolData.poolStartSize; i++)
            { 
                AddOneObjectToPool();
            }
             
        }
         

        public GameObject SpawnObjectFromPool(Vector3 position, Quaternion rotation)
        {
            for (int i = 0; i < _pool.Count; i++){
                if (_pool[i].activeSelf == false)
                {
                    _pool[i].transform.SetParent(null);
                    _pool[i].transform.position = position;
                    _pool[i].transform.rotation = rotation;
                    _pool[i].SetActive(true);
                    return _pool[i];

                }

            }

            //no available objects here

            if (_poolData.allowExpand == true)
            {
                Debug.LogWarning(_poolData.prefab.name + "Pool was expanded");
                GameObject newObj = AddOneObjectToPool();
                newObj.transform.SetParent(null);
                newObj.transform.position = position;
                newObj.transform.rotation = rotation;
                newObj.SetActive(true);

                return newObj;
            }

            return null;
        }


        public void PoolObjectBack(GameObject obj)
        { 
          

            if (_pool.Contains(obj))
            { 

                obj.transform.SetParent(parent);
                obj.SetActive(false);
                return;
            }

            //rogue object detected
            Debug.Log(obj + " Rogue object detected");

            if (_poolData.allowExpand)
            {
                obj.transform.SetParent(parent);
                obj.name = _poolData.prefab.name;
                _pool.Add(obj);
                 
            }

            obj.SetActive(false);

        }

        GameObject AddOneObjectToPool()
        {
            GameObject obj = GameObject.Instantiate(_poolData.prefab, parent);
            obj.SetActive(false);
            obj.name = _poolData.prefab.name;
            _pool.Add(obj);

            return obj;

        }



    }

}