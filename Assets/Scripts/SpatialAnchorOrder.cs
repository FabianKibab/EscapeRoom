using System;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAnchorOrder : MonoBehaviour
{
    public List<Guid> ordered = new List<Guid>();

    const string KEY_COUNT = "anchor_order_count";
    const string KEY_PREFIX = "anchor_order_";

    private void Start()
    {
        Load();
    }

    public void OnAnchorCreated(OVRSpatialAnchor anchor, OVRSpatialAnchor.OperationResult result)
    {
        if (result != OVRSpatialAnchor.OperationResult.Success)
            return;

        ordered.Add(anchor.Uuid);
        Save();
    }

    public void RemoveLast()
    {
        if (ordered.Count == 0) return;

        ordered.RemoveAt(ordered.Count - 1);
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt(KEY_COUNT, ordered.Count);

        for (int i = 0; i < ordered.Count; i++)
        {
            PlayerPrefs.SetString(KEY_PREFIX + i, ordered[i].ToString());
        }

        PlayerPrefs.Save();
    }

    public void Load()
    {
        ordered.Clear();

        int count = PlayerPrefs.GetInt(KEY_COUNT, 0);

        for (int i = 0; i < count; i++)
        {
            string key = KEY_PREFIX + i;

            if (PlayerPrefs.HasKey(key))
            {
                if (Guid.TryParse(PlayerPrefs.GetString(key), out Guid guid))
                {
                    ordered.Add(guid);
                }
            }
        }
    }
}