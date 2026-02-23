using System.Collections.Generic;
using UnityEngine;

// Factory for all non-tower structures

public class StructureFactory : MonoBehaviour
{
    [SerializeField] private Fort _fortPrefab;
    [SerializeField] private Castle _castlePrefab;

    private static StructureFactory _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static Fort SpawnFort(uint playerSlot, Vector3 position, List<Tile> tiles, List<List<PathTile>> paths)
    {
        if (_instance == null)
        {
            return null;
        }

        Fort fort = Instantiate(_instance._fortPrefab, position, Quaternion.identity);
        fort.Initialize(playerSlot, tiles, paths);
        fort.transform.SetParent(GameSceneController.Instance.Map.Strongholds.transform);
        return fort;
    }

    public static Castle SpawnCastle(uint playerSlot, Vector3 position, List<Tile> tiles, List<List<PathTile>> paths)
    {
        if (_instance == null)
        {
            return null;
        }

        Castle castle = Instantiate(_instance._castlePrefab, position, Quaternion.identity);
        castle.Initialize(playerSlot, tiles, paths);
        castle.transform.SetParent(GameSceneController.Instance.Map.Strongholds.transform);
        return castle;
    }
}
