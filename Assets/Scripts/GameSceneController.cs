using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneController : MonoBehaviour
{
    public Camera MainCamera;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile hitObjectTile = hit.collider.GetComponent<Tile>();
            if (hitObjectTile != null)
            {
                hitObjectTile.Highlight();
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    hitObjectTile.Click();
                }
            }
        }
    }
}
