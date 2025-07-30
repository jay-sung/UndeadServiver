using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject GetObject(int index)
    {
        GameObject selectedObject = null;

        foreach (GameObject obj in pools[index])
        {
            if (!obj.activeSelf)
            {
                selectedObject = obj;
                break;
            }
        }

        if (selectedObject == null)
        {
            selectedObject = Instantiate(prefabs[index]);
            pools[index].Add(selectedObject);
        }

        return selectedObject;
    }
}
