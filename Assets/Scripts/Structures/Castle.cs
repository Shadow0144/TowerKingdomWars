using UnityEngine;
using System.Collections.Generic;

public class Castle : MonoBehaviour
{
    public Map map { get; private set; }

    public Player.PlayerNumber playerNumber { get; private set; }

    public List<List<PathTile>> paths { get; private set; }

    private const float SPAWN_RATE_S = 3.0f;
    private float spawnTimer;

    public void Initialize(Map map, Player.PlayerNumber playerNumber, List<List<PathTile>> paths)
    {
        this.map = map;
        this.playerNumber = playerNumber;
        this.paths = paths;
    }

    void Start()
    {
        spawnTimer = SPAWN_RATE_S;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0)
        {
            foreach (List<PathTile> path in paths)
            {
                MonsterFactory.SpawnGoblin(transform.position, playerNumber, map, path);
            }
            spawnTimer = SPAWN_RATE_S;
        }
    }
}
