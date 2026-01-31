using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    [SerializeField] private Goblin goblinPrefab;

    private static MonsterFactory Instance;

    void Awake()
    {
        Instance = this;
    }

    public static Goblin SpawnGoblin(Vector3 startingPos, Player.PlayerNumber playerNumber, Map map, List<PathTile> path)
    {
        if (Instance == null)
        {
            return null;
        }

        Goblin goblin = Instantiate(Instance.goblinPrefab, startingPos, Quaternion.identity);
        goblin.playerNumber = playerNumber;
        goblin.map = map;
        goblin.SetPath(path);
        return goblin;
    }
}
