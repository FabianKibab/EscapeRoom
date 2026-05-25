using UnityEngine;
using Oculus.Interaction;

[RequireComponent(typeof(Collider))]
public class ChessPuzzlePokeButton : MonoBehaviour
{
    [SerializeField] private ChessPuzzleSpawner spawner;
    [SerializeField] private bool requireRelease = true;
    [SerializeField] private float cooldownSeconds = 0.25f;

    private bool isPressed;
    private float lastPressTime;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPokeInteractor(other))
        {
            return;
        }

        if (requireRelease && isPressed)
        {
            return;
        }

        if (Time.time - lastPressTime < cooldownSeconds)
        {
            return;
        }

        isPressed = true;
        lastPressTime = Time.time;

        if (spawner != null)
        {
            spawner.ResetPieces();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPokeInteractor(other))
        {
            return;
        }

        isPressed = false;
    }

    private static bool IsPokeInteractor(Collider other)
    {
        return other != null && other.GetComponentInParent<PokeInteractor>() != null;
    }
}
