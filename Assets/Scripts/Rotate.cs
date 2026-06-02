using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float deltaX = 20f;

    private float currentX = 90f;

    private const float fixedY = 90f;
    private const float fixedZ = 90f;

    void Start()
    {
        ApplyRotation();
    }

    public void RotateX()
    {
        currentX += deltaX;

        ApplyRotation();

        Debug.Log("X = " + currentX);
    }

    private void ApplyRotation()
    {
        transform.localRotation = Quaternion.Euler(
            currentX,
            fixedY,
            fixedZ
        );
    }

    public float GetNormalized()
    {
        return currentX % 360f;
    }
}