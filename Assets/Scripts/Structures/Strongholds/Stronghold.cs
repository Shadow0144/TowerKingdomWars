using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stronghold : Structure
{
    public List<List<PathTile>> Paths { get; private set; } = new List<List<PathTile>>();

    private const float _SPAWN_RATE_S = 3.0f;
    private float _spawnTimer;

    public void Initialize(uint playerSlot, List<Tile> tiles, List<List<PathTile>> paths)
    {
        base.Initialize(playerSlot, tiles);

        Paths = paths;

        _spawnTimer = _SPAWN_RATE_S;
    }

    public override void Initialize(uint playerSlot, List<Tile> tiles)
    {
        throw new NotSupportedException("This base Initialize function is superceded by the one with additional parameters");
    }

    private void Update()
    {
        if (OwningPlayerInfo.playerNumber == 0 || IsGhost)
        {
            return;
        }

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0)
        {
            foreach (List<PathTile> path in Paths)
            {
                MonsterFactory.SpawnGoblin(transform.position, OwningPlayerInfo, path);
            }
            _spawnTimer = _SPAWN_RATE_S;
        }
    }
}
