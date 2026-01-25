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
            TileScript hitObjectTileScript = hit.collider.GetComponent<TileScript>();
            if (hitObjectTileScript != null)
            {
                hitObjectTileScript.Highlight();
            }
        }
    }
}
