using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    InputAction moveAction;
    public const float CAMERA_MOVE_SPEED = 5f;

    void Update()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        Vector2 moveDirection = moveAction.ReadValue<Vector2>();
        transform.position += (new Vector3(moveDirection.x, 0, moveDirection.y) * CAMERA_MOVE_SPEED) * Time.deltaTime;
    }
}
