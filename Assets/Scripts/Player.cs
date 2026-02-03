using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private HUD hud;

    public enum PlayerNumber
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight
    };
    public PlayerNumber playerNumber { get; private set; }

    private enum TowerSelected
    {
        None,
        ArrowTower
    };
    private TowerSelected towerSelected;

    public void SelectArrowTower()
    {
        Debug.Log("Arrow Tower Selected");
    }
}
