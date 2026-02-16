using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{    
    [SerializeField] private const float _CAMERA_MOVE_SPEED = 5f;

    private void Update()
    {
        Vector2 moveDirection = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
        transform.position += (new Vector3(moveDirection.x, 0, moveDirection.y) * _CAMERA_MOVE_SPEED) * Time.deltaTime;
    }
}
