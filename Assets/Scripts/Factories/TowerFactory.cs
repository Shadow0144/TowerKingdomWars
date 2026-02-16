using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
	[SerializeField] private ArrowTower _arrowTowerPrefab;
    [SerializeField] private FlameTower _flameTowerPrefab;
    [SerializeField] private FrostTower _frostTowerPrefab;

    private static TowerFactory _instance;

	private void Awake()
	{
		_instance = this;
    }

    public static ArrowTower SpawnArrowTower(PlayerController.PlayerNumber playerNumber, Tile tile)
    {
        if (_instance == null)
        {
            return null;
        }

        ArrowTower arrowTower = Instantiate(_instance._arrowTowerPrefab, tile.transform.position, Quaternion.identity);
        arrowTower.Initialize(playerNumber, tile);
        arrowTower.gameObject.transform.SetParent(GameSceneController.Instance.Map.Towers.transform);
        return arrowTower;
    }

    public static FlameTower SpawnFlameTower(PlayerController.PlayerNumber playerNumber, Tile tile)
    {
        if (_instance == null)
        {
            return null;
        }

        FlameTower flameTower = Instantiate(_instance._flameTowerPrefab, tile.transform.position, Quaternion.identity);
        flameTower.Initialize(playerNumber, tile);
        flameTower.gameObject.transform.SetParent(GameSceneController.Instance.Map.Towers.transform);
        return flameTower;
    }

    public static FrostTower SpawnFrostTower(PlayerController.PlayerNumber playerNumber, Tile tile)
    {
        if (_instance == null)
        {
            return null;
        }

        FrostTower frostTower = Instantiate(_instance._frostTowerPrefab, tile.transform.position, Quaternion.identity);
        frostTower.Initialize(playerNumber, tile);
        frostTower.gameObject.transform.SetParent(GameSceneController.Instance.Map.Towers.transform);
        return frostTower;
    }
}
