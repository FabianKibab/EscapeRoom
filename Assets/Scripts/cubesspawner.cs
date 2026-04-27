using UnityEngine;
using Meta.XR.MRUtilityKit; // EXAKT SO IST ES RICHTIG!

public class RoomCubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;

    void Start()
    {
        // Sicherstellen, dass MRUK da ist
        if (MRUK.Instance != null)
        {
            // Wir hängen uns an das Event, wenn der Raum fertig geladen ist
            MRUK.Instance.RegisterSceneLoadedCallback(SpawnCubes);
        }
        else
        {
            Debug.LogError("MRUK Instanz nicht gefunden! Ist das MRUK-Prefab in der Szene?");
        }
    }

    void SpawnCubes()
    {
        var room = MRUK.Instance.GetCurrentRoom();

        if (room == null)
        {
            Debug.LogWarning("Raum im Simulator noch nicht bereit.");
            return;
        }

        // Wir gehen alle Anker durch (Wände, Boden, etc.)
        foreach (var anchor in room.Anchors)
        {
            SpawnCube(anchor);
        }

        Debug.Log($"Simulator: {room.Anchors.Count} Cubes wurden verteilt!");
    }

    void SpawnCube(MRUKAnchor anchor)
    {
        if (cubePrefab == null) return;

        // Erschaffen des Cubes an der Position des Wand/Boden-Ankers
        GameObject cube = Instantiate(
            cubePrefab,
            anchor.transform.position,
            anchor.transform.rotation
        );

        // Den Cube 5cm in den Raum schieben, damit er nicht IN der Wand steckt
        cube.transform.position += anchor.transform.forward * 0.05f;

        // Den Cube an den Anchor binden (saubere Hierarchie)
        cube.transform.parent = anchor.transform;
    }
}