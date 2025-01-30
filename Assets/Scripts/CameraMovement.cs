using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    private Vector3 offset = new Vector3(0, 12, -5);

    private void Update()
    {
        if (target != null)
            transform.position = target.position + offset;
    }
}
