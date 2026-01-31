using UnityEngine;
using System.Collections.Generic;

public class Castle : MonoBehaviour
{
    public Map map;

    public Player.PlayerNumber playerNumber;

    public GameObject monsterPrefab;

    private List<List<PathTile>> paths = new List<List<PathTile>>();

    private const float SPAWN_RATE_S = 3.0f;
    private float spawnTimer;

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

    public void SetPaths(List<List<PathTile>> paths)
    {
        this.paths = paths;
    }

    public void AddPath(List<PathTile> path)
    {
        paths.Add(path);
    }
}
