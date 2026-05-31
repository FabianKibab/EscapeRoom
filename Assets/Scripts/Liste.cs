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

        // NEU
        if (prefab.name.Contains("Alice"))
        {
            // Objekt in der Hierarchie suchen
            GameObject aliceInScene = GameObject.Find("Alice");

            if (aliceInScene != null)
            {
                Rigidbody aliceRb = aliceInScene.GetComponent<Rigidbody>();

                if (aliceRb != null)
                {
                    aliceRb.isKinematic = true; // kurz stoppen f�r Teleport
                    aliceRb.linearVelocity = Vector3.zero;
                    aliceRb.angularVelocity = Vector3.zero;
                }

                // Position & Rotation auf Anchor setzen
                aliceInScene.transform.position = anchorTransform.position;
                aliceInScene.transform.rotation = anchorTransform.rotation;

                if (aliceRb != null)
                {
                    aliceRb.isKinematic = false;
                    aliceRb.useGravity = true;
                    aliceRb.WakeUp();
                }

                FreezePodest freeze = aliceInScene.GetComponent<FreezePodest>();
                if (freeze != null)
                {
                    freeze.enabled = true;
                }

                Debug.Log("Alice wurde auf Anchor gesetzt.");

                return aliceInScene;
            }
            else
            {
                Debug.LogWarning("Alice nicht in Hierarchie gefunden!");
            }
        }
        //NEU Ende

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