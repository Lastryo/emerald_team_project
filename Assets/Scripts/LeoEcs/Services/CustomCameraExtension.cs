using UnityEngine;

public class CustomCameraExtension
{
    public static Vector3 GetCameraDirection(Transform t, Vector2 moveInput)
    {
        //reading the input:
        float horizontalAxis = moveInput.x;
        float verticalAxis = moveInput.y;

        //assuming we only using the single camera:

        //camera forward and right vectors:
        var forward = t.forward;
        var right = t.right;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //this is the direction in the world space we want to move:
        var desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;

        desiredMoveDirection.x = Mathf.Clamp(desiredMoveDirection.x, -1, 1);
        desiredMoveDirection.z = Mathf.Clamp(desiredMoveDirection.z, -1, 1);
        return desiredMoveDirection;
    }

    public static Vector2 RotateToMouse(Camera camera, Vector2 direction)
    {

        var rawDirection = (camera.ScreenToViewportPoint(direction) - new Vector3(.5f, .5f, 0)) * 2;
        //return new Vector3(rawDirection.x, 0, rawDirection.y);
        return rawDirection;
    }

    public static Vector3 ViewToWorldPosition(Vector2 screenPosition)
    {
        Plane plane = new Plane(Vector3.up, 0);

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (plane.Raycast(ray, out var distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    public static Vector2 GetViewPortOnCenter(Vector2 screenPosition)
    {
        return (Vector2)Camera.main.ScreenToViewportPoint(screenPosition) - new Vector2(0.5f, 0.45f);
    }
}
