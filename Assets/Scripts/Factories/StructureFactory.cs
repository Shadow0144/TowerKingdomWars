using System.Collections.Generic;
using UnityEngine;

// Factory for all non-tower structures

public class StructureFactory : MonoBehaviour
{
    [SerializeField] private Castle castlePrefab;

    private static StructureFactory Instance;

    void Awake()
    {
        Instance = this;
    }

    public static Castle SpawnCastle(Player.PlayerNumber playerNumber, Vector3 position, List<List<PathTile>> paths, Map map)
    {
        if (Instance == null)
        {
            return null;
        }

        Castle castle = Instantiate(Instance.castlePrefab, position, Quaternion.identity);
        castle.Initialize(map, playerNumber, paths);
        return castle;
    }
}
