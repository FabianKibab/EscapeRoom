using UnityEngine;

public class GravityObject : MonoBehaviour
{
    private Rigidbody rb;
    private bool wurdeAktiviert = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Fixieren();
    }

    public void Fixieren()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void StartePhysik()
    {
        if (wurdeAktiviert) return;
        wurdeAktiviert = true;

        // Wir warten 0.5 Sekunden, damit Meta den Anker fertig platziert hat
        Invoke("DoFall", 0.5f);
    }

    public void DoFall()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // 1. Vom Parent lösen (falls es noch an der Hand "klebt")
        transform.SetParent(null);

        // 2. Kinematik ausschalten
        rb.isKinematic = false;
        rb.useGravity = true;

        // 3. Rigidbody aufwecken (Ganz wichtig!)
        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }

        // 4. Ein kleiner Schubs (Optional, erzwingt Bewegung)
        rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);

        Debug.Log("Physik ist jetzt scharf und Rigidbody wurde geweckt!");
    }
}