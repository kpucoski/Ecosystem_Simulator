using System.Collections.Generic;
using Animal;
using UnityEngine;

public class ObjectPool<T> where T: MonoBehaviour {
    private Queue<AbstractAnimal> pool = new Queue<AbstractAnimal>();
    private GameObject prefab;
    private Transform parent;

    public ObjectPool(GameObject prefab, int initialSize, Transform parent = null) {
        this.prefab = prefab;
        this.parent = parent;
        for (int i = 0; i < initialSize; i++) {
            GameObject obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj.GetComponent<AbstractAnimal>());
        }
    }

    public AbstractAnimal Get() {
        if (pool.Count > 0) {
            var obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        return Object.Instantiate(prefab, parent).GetComponent<AbstractAnimal>();
    }

    public void ReturnToPool(AbstractAnimal obj) {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}