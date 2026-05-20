using UnityEngine;

public class FreezePodest : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float movementThreshold = 0.01f;
    [SerializeField] private float freezeAfterSeconds = 1f;

    private float stillTimer;
    private bool frozen;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        frozen = false;
    }

    void Update()
    {
        // Bereits eingefroren?
        if (frozen)
            return;

        // Geschwindigkeit messen
        float speed = rb.linearVelocity.magnitude;

        // Fast keine Bewegung
        if (speed < movementThreshold)
        {
            stillTimer += Time.deltaTime;

            // Lange genug still?
            if (stillTimer >= freezeAfterSeconds)
            {
                FreezeObject();
            }
        }
        else
        {
            // Wieder bewegt
            stillTimer = 0f;
        }
    }

    void FreezeObject()
    {
        frozen = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;

        Debug.Log("Podest eingefroren");
    }
}