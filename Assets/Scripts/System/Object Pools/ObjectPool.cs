using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool Instance;

    public Pool[] Pools;

    Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    string processObjectName(string objectName){
        int lastIndex = objectName.LastIndexOf('(');

        if(lastIndex > -1){
            objectName = objectName.Remove(lastIndex, objectName.Length - lastIndex).Trim();
        }

        return objectName;
    }

    void Awake() {
        if(Instance){
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach(Pool pool in Pools){
            string poolName = processObjectName(pool.PoolObject.name);
            pools.Add(poolName, new Queue<GameObject>());
            for(int i = 0; i < pool.InitCount; i++){
                GameObject obj = Instantiate(pool.PoolObject, transform);
                obj.SetActive(false);
                pools[poolName].Enqueue(obj);
            }
        }
    }

    public GameObject Spawn(GameObject obj, Vector3 position) => Spawn(obj.name, position, Quaternion.identity, null);

    public GameObject Spawn(GameObject obj, Vector3 position, Vector3 rotation) => Spawn(obj.name, position, Quaternion.Euler(rotation.x, rotation.y, rotation.z), null);

    public GameObject Spawn(GameObject obj, Vector3 position, Quaternion rotation) => Spawn(obj.name, position, rotation, null);

    public GameObject Spawn(GameObject obj, Transform parent) => Spawn(obj.name, parent.position, parent.rotation, parent);

    public GameObject Spawn(GameObject obj, Transform parent, Vector3 rotation) => Spawn(obj.name, parent.position, Quaternion.Euler(rotation.x, rotation.y, rotation.z), parent);

    public GameObject Spawn(GameObject obj, Transform parent, Quaternion rotation) => Spawn(obj.name, parent.position, rotation, parent);

    public GameObject Spawn(GameObject obj, Vector3 position, Quaternion rotation, Transform parent) => Spawn(obj.name, position, rotation, parent);

    public GameObject Spawn(string obj, Vector3 position) => Spawn(obj, position, Quaternion.identity, null);

    public GameObject Spawn(string obj, Vector3 position, Vector3 rotation) => Spawn(obj, position, Quaternion.Euler(rotation.x, rotation.y, rotation.z), null);

    public GameObject Spawn(string obj, Vector3 position, Quaternion rotation) => Spawn(obj, position, rotation, null);

    public GameObject Spawn(string obj, Transform parent) => Spawn(obj, parent.position, parent.rotation, parent);

    public GameObject Spawn(string obj, Transform parent, Vector3 rotation) => Spawn(obj, parent.position, Quaternion.Euler(rotation.x, rotation.y, rotation.z), parent);

    public GameObject Spawn(string obj, Transform parent, Quaternion rotation) => Spawn(obj, parent.position, rotation, parent);

    public GameObject Spawn(string obj, Vector3 position, Quaternion rotation, Transform parent) {
        string poolName = processObjectName(obj);
        if(!pools.ContainsKey(poolName)){
            Debug.LogError($"Object pool contains no pool for '{poolName}'");
            return null;
        }

        Queue<GameObject> queue = pools[poolName];

        if(queue.Count <= 0){
            Debug.LogError($"Cannot spawn another object from pool '{poolName}' as the pool is depleted");
            return null;
        }

        GameObject spawn = queue.Dequeue();

        spawn.transform.position = position;
        spawn.transform.rotation = rotation;
        spawn.transform.parent = parent;

        spawn.SetActive(true);

        return spawn;
    }

    public void Despawn(GameObject obj){
        string poolName = processObjectName(obj.name);
        if(!pools.ContainsKey(poolName)){
            Debug.LogError($"Object pool does not contain pool {poolName}");
            return;
        }

        obj.SetActive(false);
        obj.transform.parent = transform;
        pools[poolName].Enqueue(obj);
    }
}

[System.Serializable]
public struct Pool{
    public GameObject PoolObject;
    public int InitCount;
}