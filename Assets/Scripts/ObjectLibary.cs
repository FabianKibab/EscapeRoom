using UnityEngine;
using System.Collections.Generic;

public class ObjectLibrary : MonoBehaviour
{
    public List<GameObject> allePrefabs; // Hier ziehst du Auto, Baum, etc. rein

    public GameObject GetPrefabByName(string name)
    {
        return allePrefabs.Find(p => p.name == name);
    }
}