using UnityEngine;
using System;
using System.Collections.Generic;
using Meta.XR.BuildingBlocks;
using TMPro;

public class MyAnchorManager : MonoBehaviour
{
    [Header("Meta Building Blocks")]
    public SpatialAnchorCoreBuildingBlock anchorCore;
    // Wir nutzen hier den Loader statt den StorageManager direkt
    public SpatialAnchorLoaderBuildingBlock anchorLoader;
    public SpatialAnchorSpawnerBuildingBlock spawner;

    [Header("Setup")]
    public ObjectLibrary library;
    public TextMeshProUGUI infoText;

    [Header("Status")]
    public string aktuellerTyp = "";
    private int index = 0;

    void Start()
    {
        // 1. Automatisch laden
        Invoke("AutoLoad", 2.0f);

        if (anchorCore != null)
            anchorCore.OnAnchorCreateCompleted.AddListener(OnAnchorCreated);

        UpdateSelection();
    }

    void Update()
    {
        // Wechseln mit Joystick Rechts oder Pfeiltaste Rechts
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextObject();
        }
    }

    public void NextObject()
    {
        if (library.allePrefabs.Count == 0) return;
        index = (index + 1) % library.allePrefabs.Count;
        UpdateSelection();
    }

    void UpdateSelection()
    {
        if (library.allePrefabs.Count > 0)
        {
            GameObject gewaehltesPrefab = library.allePrefabs[index];
            aktuellerTyp = gewaehltesPrefab.name;

            if (spawner != null)
            {
                spawner.AnchorPrefab = gewaehltesPrefab;

                // Wir müssen das Objekt an der Hand sofort fixieren
                // Der Spawner erstellt intern eine Instanz, die wir finden müssen
                var previewObj = spawner.GetComponentInChildren<GravityObject>();
                if (previewObj != null) previewObj.Fixieren();
            }

            if (infoText != null)
                infoText.text = "Objekt: " + aktuellerTyp;
        }
    }

    void OnAnchorCreated(OVRSpatialAnchor anchor, OVRSpatialAnchor.OperationResult result)
    {
        if (result == OVRSpatialAnchor.OperationResult.Success)
        {
            PlayerPrefs.SetString("Type_" + anchor.Uuid.ToString(), aktuellerTyp);
            PlayerPrefs.Save();

            // Wir warten einen ganz kurzen Moment, bis die Instanz unter dem Anker existiert
            StartCoroutine(WaitAndStartPhysic(anchor));
        }
    }

    System.Collections.IEnumerator WaitAndStartPhysic(OVRSpatialAnchor anchor)
    {
        // Warte einen Frame
        yield return null;

        GravityObject grav = anchor.GetComponentInChildren<GravityObject>();
        if (grav != null)
        {
            grav.StartePhysik();
        }
        else
        {
            // Zweiter Versuch nach kurzer Zeit (manchmal braucht Meta länger)
            yield return new WaitForSeconds(0.2f);
            grav = anchor.GetComponentInChildren<GravityObject>();
            if (grav != null) grav.StartePhysik();
        }
    }

    void AutoLoad()
    {
        // Hier greifen wir auf den StorageManager zu, ohne die geschützte Methode zu rufen
        var storage = FindAnyObjectByType<SpatialAnchorLocalStorageManagerBuildingBlock>();
        if (storage == null) return;

        // Wir nutzen eine Liste, um die IDs zu sammeln
        List<Guid> uuids = new List<Guid>();

        // Da die Methode internal ist, nutzen wir den Umweg über die PlayerPrefs direkt, 
        // genau wie es das Meta-Skript intern auch macht:
        int count = PlayerPrefs.GetInt("numUuids", 0);
        for (int i = 0; i < count; i++)
        {
            string uuidStr = PlayerPrefs.GetString("uuid" + i);
            if (!string.IsNullOrEmpty(uuidStr))
            {
                uuids.Add(new Guid(uuidStr));
            }
        }

        foreach (Guid id in uuids)
        {
            string savedType = PlayerPrefs.GetString("Type_" + id.ToString(), "Default");
            GameObject prefabToSpawn = library.GetPrefabByName(savedType);

            if (prefabToSpawn != null)
            {
                anchorCore.LoadAndInstantiateAnchors(prefabToSpawn, new List<Guid> { id });
            }
        }
    }
}