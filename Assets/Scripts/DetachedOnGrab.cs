using UnityEngine;
using Oculus.Interaction;

public class DetachOnGrab : MonoBehaviour
{
    private Grabbable grabbable;
    private Rigidbody rb;

    private bool detached = false;

    void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (grabbable == null || rb == null) return;

        if (!detached && grabbable.SelectingPointsCount > 0)
        {
            detached = true;

            transform.SetParent(null, true);

            // 👉 FIX: direkt 0.1 setzen
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.WakeUp();

            Debug.Log("Detached + scale 0.1 + gravity ON");
        }
    }
}