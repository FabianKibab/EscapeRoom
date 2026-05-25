using UnityEngine;
using UnityEngine.UI;

public class ChessPuzzleResetButton : MonoBehaviour
{
    [SerializeField] private ChessPuzzleSpawner spawner;
    [SerializeField] private Button button;

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if (button != null)
        {
            button.onClick.AddListener(ResetNow);
        }
    }

    public void ResetNow()
    {
        if (spawner != null)
        {
            spawner.ResetPieces();
        }
    }
}
