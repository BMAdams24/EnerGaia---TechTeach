using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPostition = new Vector3(target.position.x, target.position.y + 4f, -10f); //Sets desiredPosition to the player x, y, and z coordinates. Force Z to be -10 so that it sees every asset (if at 0 everything disappears)

        transform.position = Vector3.Lerp(transform.position, desiredPostition, smoothSpeed *  Time.deltaTime); //Gets the position, desired position, and speed for the camera times time for moving the camera
    }
}
