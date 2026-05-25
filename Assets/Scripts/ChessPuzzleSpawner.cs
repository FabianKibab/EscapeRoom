using System.Collections.Generic;
using UnityEngine;

public class ChessPuzzleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class PieceSpawn
    {
        public string id;
        public GameObject prefab;
        public Transform spawnPoint;
        public Vector3 localPositionOffset;
        public Vector3 localEulerOffset;
        public Vector3 localScale = Vector3.one;
    }

    [Header("Spawns")]
    [SerializeField] private List<PieceSpawn> pieces = new List<PieceSpawn>();

    [Header("Options")]
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private bool resetAlsoRespawns = true;

    private readonly List<GameObject> spawned = new List<GameObject>();
    private readonly Dictionary<string, GameObject> spawnedById = new Dictionary<string, GameObject>();

    private void OnValidate()
    {
        foreach (var piece in pieces)
        {
            if (piece == null)
            {
                continue;
            }

            if (piece.localScale == Vector3.zero)
            {
                piece.localScale = Vector3.one;
            }
        }
    }

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnAll();
        }
    }

    public void SpawnAll()
    {
        ClearSpawned();

        foreach (var piece in pieces)
        {
            if (piece == null || piece.prefab == null || piece.spawnPoint == null)
            {
                continue;
            }

            var spawnRotation = piece.spawnPoint.rotation * Quaternion.Euler(piece.localEulerOffset);
            var spawnPosition = piece.spawnPoint.TransformPoint(piece.localPositionOffset);

            var instance = Instantiate(piece.prefab, spawnPosition, spawnRotation);
            instance.transform.localScale = piece.localScale;
            spawned.Add(instance);
            if (!string.IsNullOrEmpty(piece.id))
            {
                spawnedById[piece.id] = instance;
            }
        }
    }

    public void ResetPieces()
    {
        if (resetAlsoRespawns)
        {
            SpawnAll();
            return;
        }

        for (var i = 0; i < pieces.Count; i++)
        {
            var piece = pieces[i];
            if (piece == null || piece.spawnPoint == null)
            {
                continue;
            }

            if (i >= spawned.Count || spawned[i] == null)
            {
                continue;
            }

            var instance = spawned[i].transform;
            var spawnRotation = piece.spawnPoint.rotation * Quaternion.Euler(piece.localEulerOffset);
            var spawnPosition = piece.spawnPoint.TransformPoint(piece.localPositionOffset);

            instance.SetParent(null, true);
            instance.position = spawnPosition;
            instance.rotation = spawnRotation;
            instance.localScale = piece.localScale;
        }
    }

    public void ClearSpawned()
    {
        for (var i = 0; i < spawned.Count; i++)
        {
            if (spawned[i] != null)
            {
                Destroy(spawned[i]);
            }
        }

        spawned.Clear();
        spawnedById.Clear();
    }

    public Transform GetSpawnedById(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }

        if (spawnedById.TryGetValue(id, out var instance) && instance != null)
        {
            return instance.transform;
        }

        return null;
    }
}
