using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzExamples
{
  public class BB_Pool
  {
    List<GameObject> pool;
    int next = 0;

    public void GeneratePool(int size, GameObject prefab, Transform parent)
    {
      pool = new List<GameObject>();
      for(int i = 0; i < size; i++)
      {
        GameObject newObject = GameObject.Instantiate(prefab, parent);
        newObject.SetActive(false);
        pool.Add(newObject);
      }
    }

    public GameObject GetNext()
    {
      if (pool.Count == 0)
        return null;

      if (next >= pool.Count)
        next = 0;

      GameObject nextPoolObject = pool[next];

      next++;

      return nextPoolObject;
    }
  }
}
