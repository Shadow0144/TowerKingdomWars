using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    public PlayerNumber CurrentPlayerNumber { get; set; }

    public enum TowerSelected
    {
        None,
        ArrowTower,
        FlameTower,
        FrostTower,
        RemoveTower
    };
    public TowerSelected CurrentTowerSelected { get; set; }

    public void SelectNoTower()
    {
        CurrentTowerSelected = TowerSelected.None;
    }

    public void SelectArrowTower()
    {
        CurrentTowerSelected = TowerSelected.ArrowTower;
    }

    public void SelectFlameTower()
    {
        CurrentTowerSelected = TowerSelected.FlameTower;
    }

    public void SelectFrostTower()
    {
        CurrentTowerSelected = TowerSelected.FrostTower;
    }

    public void SelectRemoveTower()
    {
        CurrentTowerSelected = TowerSelected.RemoveTower;
    }
}
