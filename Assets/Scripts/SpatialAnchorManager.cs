using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAnchorManager : MonoBehaviour
{
    [SerializeField] private Liste liste;
    List<Guid> _uuids = new List<Guid>();
    List<OVRSpatialAnchor.UnboundAnchor> _unboundAnchors = new();

    const string NumUuidsPlayerPref = "numUuids";

    // Dev Mode (Two Button) or (spawn physical prefab) --> cf. [BuildingBlock] Controller Buttons Mapper Dev 
    public void DevMode()
    {
        // Get all currently existing Spatial Anchors in the scene (they´ll be hidden)
        var anchors = FindObjectsOfType<OVRSpatialAnchor>();

        // Show all Spatial Anchors in the scene (no more hidden)
        foreach (var anchor in anchors)
        {
            var renderers = anchor.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers)
            {
                r.enabled = true;
            }
        }

        // Spawn on evry Anchor a physical Object
        StartCoroutine(PrintSceneAnchorsDelayed());
    }

    public void PlayerMode()
    {
        StartCoroutine(PlayerModeFlash());
    }

    // Flash the Anchor just for a short Time
    IEnumerator PlayerModeFlash()
    {
        GetAnchorAnchorUuidFromLocalStorage(_uuids);

        
        var anchors = FindObjectsOfType<OVRSpatialAnchor>();

        foreach (var anchor in anchors)
        {
            var renderers = anchor.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers)
            {
                r.enabled = true;
            }
        }

        yield return StartCoroutine(PrintSceneAnchorsDelayed());

        yield return anchors = FindObjectsOfType<OVRSpatialAnchor>();

        foreach (var anchor in anchors)
        {
            var renderers = anchor.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers)
            {
                r.enabled = false;
            }
        }
    }

    void GetAnchorAnchorUuidFromLocalStorage(List<Guid> uuids)
    {
        if (!PlayerPrefs.HasKey(NumUuidsPlayerPref))
        {
            Debug.Log("Keine Anchors gespeichert.");
            return;
        }

        uuids.Clear();
        int count = PlayerPrefs.GetInt(NumUuidsPlayerPref);

        for (int i = 0; i < count; i++)
        {
            string key = "uuid" + i;

            if (!PlayerPrefs.HasKey(key))
                continue;

            string uuidString = PlayerPrefs.GetString(key);

            if (Guid.TryParse(uuidString, out Guid guid))
            {
                uuids.Add(guid);
            }
        }
    }

    // For debugging get ID,Position and Rotation and then Spawn the Prefab there and give the Prefab a unique Name
    IEnumerator PrintSceneAnchorsDelayed()
    {
        yield return new WaitForSeconds(0.5f);

        var anchors = FindObjectsOfType<OVRSpatialAnchor>();

        Debug.Log("Gefundene Anchors: " + anchors.Length);

        int index = 0;

        foreach (var anchor in anchors)
        {
            Debug.Log($"UUID: {anchor.Uuid}");
            Debug.Log($"Pos: {anchor.transform.position}");
            Debug.Log($"Rot: {anchor.transform.rotation.eulerAngles}");

            GameObject obj = liste.SpawnPrefabAtAnchor(index, anchor.transform);

            if (obj != null)
            {
                obj.name = "SpawnedObject_"+ index;
            }

            index++;
        }
    }

    public void DeleteAllSpawnedObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.name.StartsWith("SpawnedObject_"))
            {
                Destroy(obj);
            }
        }
    }

    public async void DeleteLastAnchor()
    {
        SpatialAnchorOrder order = FindFirstObjectByType<SpatialAnchorOrder>();

        if (order == null || order.ordered.Count == 0)
        {
            Debug.Log("Keine gespeicherten Anchors in Reihenfolge.");
            return;
        }

        Guid lastUuid = order.ordered[^1];

        // finde Anchor in Szene über Component + UUID
        OVRSpatialAnchor target = null;

        var anchors = FindObjectsOfType<OVRSpatialAnchor>();
        foreach (var a in anchors)
        {
            if (a.Uuid == lastUuid)
            {
                target = a;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogWarning("Anchor nicht in Szene gefunden: " + lastUuid);
            order.ordered.RemoveAt(order.ordered.Count - 1);
            return;
        }

        Debug.Log("Erasing REAL last anchor: " + lastUuid);

        var result = await target.EraseAnchorAsync();

        if (!result.Success)
        {
            Debug.LogWarning("Anchor konnte nicht gelöscht werden!");
            return;
        }

        Destroy(target.gameObject);

        order.ordered.RemoveAt(order.ordered.Count - 1);
    }
}