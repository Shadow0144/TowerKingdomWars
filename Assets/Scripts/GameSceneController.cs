using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneController : MonoBehaviour
{
    public static GameSceneController Instance { get; private set; }

    [SerializeField] private Camera _mainCamera;
    public Camera MainCamera => _mainCamera;

    [SerializeField] private PlayerController _localPlayer;
    public PlayerController LocalPlayer => _localPlayer;

    [SerializeField] private HUDController _hud;
    public HUDController HUD => _hud;

    [SerializeField] private MapController _map;
    public MapController Map => _map;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile hitObjectTile = hit.collider.GetComponent<Tile>();
            if (hitObjectTile != null && LocalPlayer.CurrentTowerSelected != PlayerController.TowerSelected.None)
            {
                hitObjectTile.Highlight();
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    hitObjectTile.Click();
                }
            }
        }
    }

    public void DeselectTower()
    {
        HUD.DeselectTower();
    }
}
