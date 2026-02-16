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

    public static Fort SpawnFort(PlayerController.PlayerNumber playerNumber, Vector3 position, List<List<PathTile>> paths, MapController map)
    {
        if (_instance == null)
        {
            return null;
        }

        Fort fort = Instantiate(_instance._fortPrefab, position, Quaternion.identity);
        fort.Initialize(map, playerNumber, paths);
        fort.transform.SetParent(GameSceneController.Instance.Map.Strongholds.transform);
        return fort;
    }

    public static Castle SpawnCastle(PlayerController.PlayerNumber playerNumber, Vector3 position, List<List<PathTile>> paths, MapController map)
    {
        if (_instance == null)
        {
            return null;
        }

        Castle castle = Instantiate(_instance._castlePrefab, position, Quaternion.identity);
        castle.Initialize(map, playerNumber, paths);
        castle.transform.SetParent(GameSceneController.Instance.Map.Strongholds.transform);
        return castle;
    }
}
