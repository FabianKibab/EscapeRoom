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

    public void DevMode()
    {
        Debug.Log("Lese gespeicherte UUIDs...");

        GetAnchorAnchorUuidFromLocalStorage(_uuids);

        foreach (var uuid in _uuids)
        {
            Debug.Log("UUID (gespeichert): " + uuid);
        }
        var anchors = FindObjectsOfType<OVRSpatialAnchor>();

        foreach (var anchor in anchors)
        {
            var renderers = anchor.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers)
            {
                r.enabled = true;
            }
        }

        StartCoroutine(PrintSceneAnchorsDelayed());

    }

    public void PlayerMode()
    {
        StartCoroutine(PlayerModeFlash());
    }

    IEnumerator PlayerModeFlash()
    {
        // 1. UUIDs laden
        GetAnchorAnchorUuidFromLocalStorage(_uuids);

        foreach (var uuid in _uuids)
        {
            Debug.Log("UUID (gespeichert): " + uuid);
        }

        // 2. ALLE Anchors sichtbar machen
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

        // 4. Wieder verstecken
        foreach (var anchor in anchors)
        {
            Debug.Log("ICH BIN AM VERSTECKEN");
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

            liste.SpawnPrefabAtAnchor(index, anchor.transform);
            index++;
        }
    }
}