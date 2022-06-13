using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    [SerializeField] GameObject popupTextprefab;
    [SerializeField] int prefabCount = 15;
    public List<GameObject> pool;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        AddToPool(prefabCount);

    }
    private void AddToPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newObj = Instantiate(popupTextprefab);
            newObj.transform.SetParent(this.gameObject.transform);
            newObj.SetActive(false);
            pool.Add(newObj);
        }
    }
    public GameObject GetObjFromPool()
    {
        if (pool.Any(e => e.activeInHierarchy == false))
        {
            return pool.First(e => e.activeInHierarchy == false);
        }
        else
        {
            AddToPool(1);

            return pool.First(e => e.activeInHierarchy == false);
        }
    }

}
