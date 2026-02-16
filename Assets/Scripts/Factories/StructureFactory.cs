using System.Collections.Generic;
using UnityEngine;

// Factory for all non-tower structures

public class StructureFactory : MonoBehaviour
{
    [SerializeField] private Castle _castlePrefab;

    private static StructureFactory _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static Castle SpawnCastle(PlayerController.PlayerNumber playerNumber, Vector3 position, List<List<PathTile>> paths, MapController map)
    {
        if (_instance == null)
        {
            return null;
        }

        Castle castle = Instantiate(_instance._castlePrefab, position, Quaternion.identity);
        castle.Initialize(map, playerNumber, paths);
        castle.transform.SetParent(GameSceneController.Instance.Map.Castles.transform);
        return castle;
    }
}
