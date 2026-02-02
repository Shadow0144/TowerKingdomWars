using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
	[SerializeField] private ArrowTower arrowTowerPrefab;

	private static TowerFactory Instance;

	void Awake()
	{
		Instance = this;
	}

	public static ArrowTower SpawnArrowTower(Player.PlayerNumber playerNumber, Tile tile)
	{
		if (Instance == null)
		{
			return null;
		}

        ArrowTower arrowTower = Instantiate(Instance.arrowTowerPrefab, tile.transform.position, Quaternion.identity);
		arrowTower.Initialize(playerNumber, tile);
		return arrowTower;
	}
}
