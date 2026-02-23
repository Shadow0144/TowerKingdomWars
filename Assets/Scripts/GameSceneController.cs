using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneController : MonoBehaviour
{
    public static GameSceneController Instance { get; private set; }

    [SerializeField] private Camera _mainCamera;
    public Camera MainCamera => _mainCamera;

    [SerializeField] private PlayerController _localPlayer;
    public PlayerController LocalPlayer => _localPlayer;

    private List<PlayerController.PlayerInfo> _playerInfoList = new List<PlayerController.PlayerInfo>();
    public List<PlayerController.PlayerInfo> PlayerInfoList => _playerInfoList;

    [SerializeField] private HUDController _hud;
    public HUDController HUD => _hud;

    [SerializeField] private MapController _map;
    public MapController Map => _map;

    private LuaScriptHandler _luaScriptHandler = new LuaScriptHandler();

    private Structure ghostStructure;

    private void Awake()
    {
        Instance = this;

        _localPlayer.CurrentPlayerInfo = new PlayerController.PlayerInfo
        {
            username = "Player1",
            playerSlot = 1,
            playerNumber = 1,
            teamNumber = 1
        };

        _playerInfoList.Add(_localPlayer.CurrentPlayerInfo);
        _playerInfoList.Add(new PlayerController.PlayerInfo
        {
            username = "Player2",
            playerSlot = 2,
            playerNumber = 2,
            teamNumber = 2
        });

        _luaScriptHandler.Initialize();
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _luaScriptHandler.LoadMap("TestMap.lua");

        // Temporary
        foreach (GameObject structureGameObject in GameObject.FindGameObjectsWithTag("Structure"))
        {
            Structure structure = structureGameObject.GetComponent<Structure>();
            if (structure != null)
            {
                structure.OwningPlayerInfo = _playerInfoList[(int)structure.PlayerSlot];
            }
        }
    }

    private void Update()
    {
        Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile hitObjectTile = hit.collider.GetComponent<Tile>();
            if (hitObjectTile != null)
            {
                if (LocalPlayer.CurrentTowerTypeSelected == PlayerController.TowerTypeSelected.RemoveTower)
                {
                    if (hitObjectTile.ContainedStructure != null
                        && hitObjectTile.ContainedStructure.OwningPlayerInfo.playerNumber == LocalPlayer.CurrentPlayerInfo.playerNumber
                        && hitObjectTile.ContainedStructure.CanBeDeconstructed)
                    {
                        hitObjectTile.Highlight(true);
                        if (Mouse.current.leftButton.wasPressedThisFrame)
                        {
                            RemoveTower(hitObjectTile);
                        }
                    }
                    else
                    {
                        hitObjectTile.Highlight(false);
                    }
                }
                else if (LocalPlayer.CurrentTowerTypeSelected != PlayerController.TowerTypeSelected.None
                        && ghostStructure != null)
                {
                    ghostStructure.transform.position = hitObjectTile.transform.position;
                    // Find all the tiles in the footprint
                    bool valid = true;
                    List<Vector2Int> footprint = ghostStructure.GetFootprint();
                    List<Tile> coveredTiles = new List<Tile>();
                    foreach (Vector2Int coveredTile in footprint)
                    {
                        if ((hitObjectTile.Row + coveredTile.x) >= 0
                            && (hitObjectTile.Row + coveredTile.x) < Map.TileMatrix.GetLength(0)
                            && (hitObjectTile.Column + coveredTile.y) >= 0
                            && (hitObjectTile.Column + coveredTile.y) < Map.TileMatrix.GetLength(1))
                        {
                            Tile tile = Map.TileMatrix[hitObjectTile.Row + coveredTile.x, hitObjectTile.Column + coveredTile.y];
                            valid &= !tile.ContainsStructure;
                            tile.Highlight(!tile.ContainsStructure);
                            coveredTiles.Add(tile);
                        }
                        else
                        {
                            valid = false;
                        }
                    }
                    if (valid && Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        SpawnTower(hitObjectTile, coveredTiles);
                    }
                }
            }
        }
    }

    public void SetGhostStructure(Structure ghostStructure)
    {
        if (this.ghostStructure != null)
        {
            Destroy(this.ghostStructure.gameObject);
        }
        this.ghostStructure = ghostStructure;
    }

    public void DeselectTower()
    {
        HUD.DeselectTower();
        if (ghostStructure != null)
        {
            Destroy(ghostStructure.gameObject);
            ghostStructure = null;
        }
    }

    private void SpawnTower(Tile centerTile, List<Tile> coveredTiles)
    {
        Structure constructedStructure = null;
        switch (LocalPlayer.CurrentTowerTypeSelected)
        {
            case PlayerController.TowerTypeSelected.None:
                {
                    // Do nothing
                }
                break;
            case PlayerController.TowerTypeSelected.ArrowTower:
                {
                    constructedStructure = TowerFactory.SpawnArrowTower(LocalPlayer.CurrentPlayerInfo.playerSlot, centerTile.transform.position, coveredTiles);
                }
                break;
            case PlayerController.TowerTypeSelected.FlameTower:
                {
                    constructedStructure = TowerFactory.SpawnFlameTower(LocalPlayer.CurrentPlayerInfo.playerSlot, centerTile.transform.position, coveredTiles);
                }
                break;
            case PlayerController.TowerTypeSelected.FrostTower:
                {
                    constructedStructure = TowerFactory.SpawnFrostTower(LocalPlayer.CurrentPlayerInfo.playerSlot, centerTile.transform.position, coveredTiles);
                }
                break;
            case PlayerController.TowerTypeSelected.RemoveTower:
                {
                    // This should not happen
                }
                break;
        }
        if (constructedStructure != null)
        {
            constructedStructure.OwningPlayerInfo = LocalPlayer.CurrentPlayerInfo;
        }
        foreach (Tile tile in coveredTiles)
        {
            tile.ContainedStructure = constructedStructure; // Can be null
        }
        DeselectTower();
    }

    private void RemoveTower(Tile selectedTile)
    {
        if (selectedTile.ContainedStructure != null)
        {
            Destroy(selectedTile.ContainedStructure.gameObject);
            foreach (Tile tile in selectedTile.ContainedStructure.CurrentTiles)
            {
                tile.ContainedStructure = null;
            }
        }
        DeselectTower();
    }
}