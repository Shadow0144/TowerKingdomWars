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

	public static Tile SpawnTile(string name, Vector3 position)
	{
		if (_instance == null)
		{
			return null;
		}

		Tile tile = Instantiate(_instance._tilePrefab, position, _instance._tilePrefab.transform.rotation);
		tile.Initialize(name);
		tile.gameObject.transform.SetParent(GameSceneController.Instance.Map.Tiles.transform);
		return tile;
	}

	public static Tile SpawnPathTile(string name, Vector3 position)
	{
		if (_instance == null)
		{
			return null;
		}

		PathTile pathTile = Instantiate(_instance._pathTilePrefab, position, _instance._pathTilePrefab.transform.rotation);
        pathTile.Initialize(name);
        pathTile.gameObject.transform.SetParent(GameSceneController.Instance.Map.Tiles.transform);
        return pathTile;
	}
}
