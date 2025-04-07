using UnityEngine;

public class ShieldRotator : MonoBehaviour
{
    private float rotationSpeed = 90f;

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
