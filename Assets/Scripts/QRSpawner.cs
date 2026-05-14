using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class QRSpawner : MonoBehaviour
{
    // Alle gefundenen QR Codes speichern
    private List<MRUKTrackable> qrCodes = new List<MRUKTrackable>();

    // Aktuell verwendeter QR
    private MRUKTrackable activeQR;

    // Aktuelles Objekt
    private GameObject spawnedObject;

    // Welcher QR aktuell aktiv ist
    private string activeQRText = "";

    // Prefabs
    public GameObject prefabA;
    public GameObject prefabB;
    public GameObject prefabC;
    public GameObject prefabD;
    public GameObject prefabE;
    public GameObject prefabF;
    public GameObject prefabG;
    public GameObject raetselText1;
    public GameObject raetselText2;
    public GameObject raetselText3;
    public GameObject raetselText4;
    public GameObject defaultPrefab;

    public float forwardOffset = 0.5f;
    public float upOffset = 0.01f;

    private Dictionary<string, GameObject> prefabMap;

    void Start()
    {
        prefabMap = new Dictionary<string, GameObject>()
        {
            { "The_Racing_Story", prefabA },
            { "The_Racing_Story2", prefabB },
            { "The_Racing_Story3", prefabC },
            { "The_Racing_Story4", prefabD },
            { "The_Racing_Story5", prefabE },
            { "The_Racing_Story6", prefabF },
            { "The_Racing_Story7", prefabG }

            { "Zettel1", raetselText1 }, 
            { "Zettel2", raetselText2 },
            { "Zettel3", raetselText3 },
            { "Zettel4", raetselText4 }
        };
    }

    void Update()
    {
        if (qrCodes.Count == 0)
            return;

        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;

        MRUKTrackable bestQR = null;
        float bestDot = 0.7f;

        foreach (MRUKTrackable qr in qrCodes)
        {
            if (qr == null)
                continue;

            Vector3 dirToQR =
                (qr.transform.position - cameraPos).normalized;

            float dot = Vector3.Dot(cameraForward, dirToQR);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestQR = qr;
            }
        }

        if (bestQR == null)
            return;

        activeQR = bestQR;

        string qrText = activeQR.MarkerPayloadString;

        if (qrText != activeQRText)
        {
            activeQRText = qrText;

            Debug.Log("ACTIVE QR: " + qrText);

            GameObject prefabToSpawn;

            if (!prefabMap.TryGetValue(qrText, out prefabToSpawn))
            {
                prefabToSpawn = defaultPrefab;
            }

            SpawnPrefab(prefabToSpawn);
        }

        if (spawnedObject != null)
        {
            ApplyTransform(spawnedObject, activeQR);
        }
    }

    public void OnTrackableAdded(MRUKTrackable trackable)
    {
        if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
            return;

        if (!qrCodes.Contains(trackable))
        {
            qrCodes.Add(trackable);
            Debug.Log("QR REGISTERED: " + trackable.MarkerPayloadString);
        }
    }

    void SpawnPrefab(GameObject prefab)
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
        }

        spawnedObject = Instantiate(prefab);
        ApplyTransform(spawnedObject, activeQR);
    }

    void ApplyTransform(GameObject obj, MRUKTrackable qr)
    {
        Vector3 targetPosition =
            qr.transform.position +
            qr.transform.forward * forwardOffset +
            qr.transform.up * upOffset;

        obj.transform.position = Vector3.Lerp(
            obj.transform.position,
            targetPosition,
            Time.deltaTime * 10f
        );

        obj.transform.rotation = Quaternion.Slerp(
            obj.transform.rotation,
            qr.transform.rotation * Quaternion.Euler(-90, 180, 0),
            Time.deltaTime * 10f
        );
    }
}