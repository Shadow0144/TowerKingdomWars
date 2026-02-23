using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
	[SerializeField] private Tile _tilePrefab;
	[SerializeField] private PathTile _pathTilePrefab;

	private static TileFactory _instance;

	private void Awake()
	{
		_instance = this;
	}

	public static Tile SpawnTile(Vector3 position, uint row, uint col)
	{
		if (_instance == null)
		{
			return null;
		}

		Tile tile = Instantiate(_instance._tilePrefab, position, _instance._tilePrefab.transform.rotation);
		tile.Initialize(row, col);
		tile.gameObject.transform.SetParent(GameSceneController.Instance.Map.Tiles.transform);
		return tile;
	}

	public static PathTile SpawnPathTile(Vector3 position, uint row, uint col)
	{
		if (_instance == null)
		{
			return null;
		}

		PathTile pathTile = Instantiate(_instance._pathTilePrefab, position, _instance._pathTilePrefab.transform.rotation);
        pathTile.Initialize(row, col);
        pathTile.gameObject.transform.SetParent(GameSceneController.Instance.Map.Tiles.transform);
        return pathTile;
	}
}
