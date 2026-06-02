using UnityEngine;

public class Door : MonoBehaviour
{
    public Rotate slot1;
    public Rotate slot2;
    public Rotate slot3;

    void Update()
    {
        if (slot1.GetNormalized() == 210f &&   // 4
            slot2.GetNormalized() == 50f &&    // 9
            slot3.GetNormalized() == 130f)     // 2
        {
            Debug.Log("CODE RICHTIG!");
            GameObject tuer = GameObject.FindWithTag("Tuer");
            if (tuer != null)
            {
                tuer.SetActive(false);
            }
        }
    }
}