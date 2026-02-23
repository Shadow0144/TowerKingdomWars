using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public struct PlayerInfo
    {
        public string username;
        public uint playerSlot;
        public uint playerNumber; // 0 for unowned
        public uint teamNumber;

        public PlayerInfo(string username, uint playerSlot = 0, uint playerNumber = 0, uint teamNumber = 0)
        {
            this.username = username;
            this.playerSlot = playerSlot;
            this.playerNumber = playerNumber;
            this.teamNumber = teamNumber;
        }
    };
    public PlayerInfo CurrentPlayerInfo { get; set; }

    public enum TowerTypeSelected
    {
        None,
        ArrowTower,
        FlameTower,
        FrostTower,
        RemoveTower
    };
    public TowerTypeSelected CurrentTowerTypeSelected { get; set; }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private const float _CAMERA_MOVE_SPEED = 10.0f;

    Vector3 topLeftBounds;
    Vector3 bottomRightBounds;

    public void SetBounds(Vector3 topLeft, Vector3 bottomRight)
    {
        topLeftBounds = topLeft;
        bottomRightBounds = bottomRight;
    }

    private void Update()
    {
        Vector2 moveDirection = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
        mainCamera.transform.position += (new Vector3(moveDirection.x, 0, moveDirection.y) * _CAMERA_MOVE_SPEED) * Time.deltaTime;
        if (mainCamera.transform.position.x < topLeftBounds.x)
        {
            mainCamera.transform.position = new Vector3(topLeftBounds.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
        else if (mainCamera.transform.position.x > bottomRightBounds.x)
        {
            mainCamera.transform.position = new Vector3(bottomRightBounds.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
        if (mainCamera.transform.position.z > topLeftBounds.z)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, topLeftBounds.z);
        }
        else if (mainCamera.transform.position.z < bottomRightBounds.z)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, bottomRightBounds.z);
        }
    }

    public void SelectNoTower()
    {
        CurrentTowerTypeSelected = TowerTypeSelected.None;
        GameSceneController.Instance.SetGhostStructure(null);
    }

    public Vector3 MousePositionToWorldPosition()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreen);
        Vector3 worldPosition = new Vector3();
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            worldPosition = hit.point;
        }
        return worldPosition;
    }

    public void SelectArrowTower()
    {
        CurrentTowerTypeSelected = TowerTypeSelected.ArrowTower;
        GameSceneController.Instance.SetGhostStructure(TowerFactory.getArrowTowerGhost(MousePositionToWorldPosition()));
    }

    public void SelectFlameTower()
    {
        CurrentTowerTypeSelected = TowerTypeSelected.FlameTower;
        GameSceneController.Instance.SetGhostStructure(TowerFactory.getFlameTowerGhost(MousePositionToWorldPosition()));
    }

    public void SelectFrostTower()
    {
        CurrentTowerTypeSelected = TowerTypeSelected.FrostTower;
        GameSceneController.Instance.SetGhostStructure(TowerFactory.getFrostTowerGhost(MousePositionToWorldPosition()));
    }

    public void SelectRemoveTower()
    {
        CurrentTowerTypeSelected = TowerTypeSelected.RemoveTower;
    }
}
