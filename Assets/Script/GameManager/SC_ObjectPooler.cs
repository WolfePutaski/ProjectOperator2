using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand;
}

public class SC_ObjectPooler : MonoBehaviour
{
    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    public void DeactivePooledObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void DeactivePooledObject(GameObject gameObject, float delay = 0)
    {
        StartCoroutine(_DeactivePooledObject(gameObject, delay));
    }

    IEnumerator _DeactivePooledObject(GameObject gameObject, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

}
