using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform rocket;
    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void LateUpdate()
    {
        if (rocket != null)
        {
            float targetY = Mathf.Max(rocket.position.y, startY);
            transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        }
    }
}