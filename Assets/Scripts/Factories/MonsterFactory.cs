using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    [SerializeField] private Goblin _goblinPrefab;

    private static MonsterFactory _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static Goblin SpawnGoblin(Vector3 startingPos, PlayerController.PlayerNumber playerNumber, MapController map, List<PathTile> path)
    {
        if (_instance == null)
        {
            return null;
        }

        Goblin goblin = Instantiate(_instance._goblinPrefab, startingPos, Quaternion.identity);
        goblin.Initialize(playerNumber, map, path);
        goblin.transform.SetParent(GameSceneController.Instance.Map.Monsters.transform);
        return goblin;
    }
}
