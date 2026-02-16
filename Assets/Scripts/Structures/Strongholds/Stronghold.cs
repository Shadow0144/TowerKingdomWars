using UnityEngine;
using System.Collections.Generic;

public class Stronghold : MonoBehaviour
{
    public MapController CurrentMap { get; private set; }

    public PlayerController.PlayerNumber CurrentPlayerNumber { get; private set; }

    public List<List<PathTile>> Paths { get; private set; } = new List<List<PathTile>>();

    private const float _SPAWN_RATE_S = 3.0f;
    private float _spawnTimer;

    public void Initialize(MapController map, PlayerController.PlayerNumber playerNumber, List<List<PathTile>> paths)
    {
        CurrentMap = map;
        CurrentPlayerNumber = playerNumber;
        Paths = paths;

        _spawnTimer = _SPAWN_RATE_S;
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0)
        {
            foreach (List<PathTile> path in Paths)
            {
                MonsterFactory.SpawnGoblin(transform.position, CurrentPlayerNumber, CurrentMap, path);
            }
            _spawnTimer = _SPAWN_RATE_S;
        }
    }
}
