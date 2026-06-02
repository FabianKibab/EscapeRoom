using System.Collections;
using UnityEngine;

public class PodestManager : MonoBehaviour
{
    [Header("Die 3 Trigger Collider")]
    public Collider trigger1;
    public Collider trigger2;
    public Collider trigger3;

    [Header("Renderer vom Podest")]
    public Renderer pedestalRenderer;

    private bool slot1Correct = false;
    private bool slot2Correct = false;
    private bool slot3Correct = false;

    private bool solved = false;

    private void Update()
    {
        CheckObjects();

        if (!solved &&
            slot1Correct &&
            slot2Correct &&
            slot3Correct)
        {
            solved = true;
            StartCoroutine(DisappearSequence());
        }
    }

    void CheckObjects()
    {
        slot1Correct = false;
        slot2Correct = false;
        slot3Correct = false;

        Collider[] allColliders = FindObjectsOfType<Collider>();

        foreach (Collider col in allColliders)
        {
            if (col.transform == transform)
                continue;

            // Trigger 1 -> Tag CarNb1
            if (trigger1.bounds.Intersects(col.bounds))
            {
                if (col.CompareTag("CarNb1"))
                {
                    slot1Correct = true;
                }
            }

            // Trigger 2 -> Tag CarNb2
            if (trigger2.bounds.Intersects(col.bounds))
            {
                if (col.CompareTag("CarNb3"))
                {
                    slot2Correct = true;
                }
            }

            // Trigger 3 -> Tag CarNb3
            if (trigger3.bounds.Intersects(col.bounds))
            {
                if (col.CompareTag("CarNb5"))
                {
                    slot3Correct = true;
                }
            }
        }
    }

    IEnumerator DisappearSequence()
    {
        Material mat = pedestalRenderer.material;

        // Grün leuchten
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.green * 5f);

        yield return new WaitForSeconds(1f);

        // Langsam verschwinden
        float duration = 3f;
        float timer = 0f;

        Color startColor = mat.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, timer / duration);

            Color c = startColor;
            c.a = alpha;

            mat.color = c;

            yield return null;
        }

        gameObject.SetActive(false);

        GameObject CarNb1 = GameObject.FindWithTag("CarNb1");
        if (CarNb1 != null)
        {
            CarNb1.SetActive(false);
        }
        GameObject CarNb2 = GameObject.FindWithTag("CarNb2");
        if (CarNb2 != null)
        {
            CarNb2.SetActive(false);
        }
        GameObject CarNb3 = GameObject.FindWithTag("CarNb3");
        if (CarNb3 != null)
        {
            CarNb3.SetActive(false);
        }
        GameObject CarNb4 = GameObject.FindWithTag("CarNb4");
        if (CarNb4 != null)
        {
            CarNb4.SetActive(false);
        }
        GameObject CarNb5 = GameObject.FindWithTag("CarNb5");
        if (CarNb5 != null)
        {
            CarNb5.SetActive(false);
        }
        GameObject CarNb6 = GameObject.FindWithTag("CarNb6");
        if (CarNb6 != null)
        {
            CarNb6.SetActive(false);
        }
    }
}