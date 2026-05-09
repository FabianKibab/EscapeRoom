using System.Collections.Generic;
using UnityEngine;

public class Liste : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;

    public GameObject GetPrefab(int index)
    {
        if (index < 0 || index >= prefabs.Count)
            return null;

        return prefabs[index];
    }

    public GameObject SpawnPrefabAtAnchor(int index, Transform anchorTransform)
    {
        GameObject prefab = GetPrefab(index);
        if (prefab == null) return null;

        GameObject obj = Instantiate(
            prefab,
            anchorTransform.position,
            anchorTransform.rotation
        );

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        return obj;
    }
}