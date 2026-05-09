using UnityEngine;
using Meta.XR.MRUtilityKit;

public class QRSpawner : MonoBehaviour
{
    private MRUKTrackable trackedQR;
    private GameObject spawnedCube;

    public GameObject cubePrefab;

    // Abstand relativ zum QR-Code
    public float forwardOffset = 0.5f;
    public float upOffset = 0.01f;

    public void OnTrackableAdded(MRUKTrackable trackable)
    {
        if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
            return;

        Debug.Log("QR DETECTED");

        trackedQR = trackable;

        spawnedCube = Instantiate(cubePrefab);

        // Lokaler Offset relativ zur QR-Rotation
        Vector3 offset =
            trackable.transform.forward * forwardOffset +
            trackable.transform.up * upOffset;

        spawnedCube.transform.position =
            trackable.transform.position + offset;

        // Rotation anpassen
        spawnedCube.transform.rotation =
            trackable.transform.rotation *
            Quaternion.Euler(-90, 180, 0);

        spawnedCube.transform.localScale = Vector3.one;
    }

    void Update()
    {
        if (trackedQR == null || spawnedCube == null)
            return;

        // Offset relativ zur aktuellen QR-Ausrichtung
        Vector3 targetPosition =
            trackedQR.transform.position +
            trackedQR.transform.forward * forwardOffset +
            trackedQR.transform.up * upOffset;

        // Smooth Position
        spawnedCube.transform.position = Vector3.Lerp(
            spawnedCube.transform.position,
            targetPosition,
            Time.deltaTime * 10f
        );

        // Smooth Rotation
        spawnedCube.transform.rotation = Quaternion.Slerp(
            spawnedCube.transform.rotation,
            trackedQR.transform.rotation *
            Quaternion.Euler(-90, 180, 0),
            Time.deltaTime * 10f
        );
    }
}