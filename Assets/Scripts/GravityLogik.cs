using UnityEngine;
using Oculus.Interaction;

public class GravityObject : MonoBehaviour
{
    private Rigidbody rb;
    private bool wurdeAktiviert = false;

    private Grabbable grabbable;
    private bool detached = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();

        Fixieren();
    }

    void Update()
    {
        // Sobald das Objekt gegrabt wird
        if (!detached && grabbable != null && grabbable.SelectingPointsCount > 0)
        {
            detached = true;

            // Vom Parent lösen
            transform.SetParent(null, true);

            // Objekt 10x kleiner machen
            transform.localScale *= 0.1f;

            Debug.Log("Objekt detached und kleiner gemacht!");
        }

        // Wenn losgelassen → Physik aktivieren
        if (detached && grabbable != null && grabbable.SelectingPointsCount == 0)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            if (rb.IsSleeping())
            {
                rb.WakeUp();
            }

            Debug.Log("Gravity aktiviert!");
        }
    }

    public void Fixieren()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void StartePhysik()
    {
        if (wurdeAktiviert) return;

        wurdeAktiviert = true;

        Invoke(nameof(DoFall), 0.5f);
    }

    public void DoFall()
    {
        transform.SetParent(null, true);

        rb.isKinematic = false;
        rb.useGravity = true;

        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }

        rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);

        Debug.Log("Physik ist jetzt scharf und Rigidbody wurde geweckt!");
    }
}