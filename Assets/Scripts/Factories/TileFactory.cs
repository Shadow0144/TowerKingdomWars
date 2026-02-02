using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
	[SerializeField] private Tile tilePrefab;
	[SerializeField] private PathTile pathTilePrefab;

	private static TileFactory Instance;

	void Awake()
	{
		Instance = this;
	}

	public static Tile SpawnTile(Map map, string name, Vector3 position)
	{
		if (Instance == null)
		{
			return null;
		}

		Tile tile = Instantiate(Instance.tilePrefab, position, Instance.tilePrefab.transform.rotation);
		tile.Initialize(name, map);
		return tile;
	}

	public static Tile SpawnPathTile(Map map, string name, Vector3 position)
	{
		if (Instance == null)
		{
			return null;
		}

		PathTile pathTile = Instantiate(Instance.pathTilePrefab, position, Instance.pathTilePrefab.transform.rotation);
        pathTile.Initialize(name, map);
        return pathTile;
	}
}
