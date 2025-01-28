using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    private Vector3 offset = new Vector3(0, 7.16f, -4.86f);

    private void Update()
    {
        if (target != null)
            transform.position = target.position + offset;
    }
}
