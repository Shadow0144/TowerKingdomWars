using UnityEngine;
using System.Collections.Generic;

public class CastleScript : MonoBehaviour
{
    public GameObject monsterPrefab;

    private List<List<GameObject>> paths = new List<List<GameObject>>();

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
            foreach (List<GameObject> path in paths)
            {
                GameObject monsterInstance = Instantiate(monsterPrefab, gameObject.transform.position, Quaternion.identity);
                MonsterScript monster = monsterInstance.GetComponent<MonsterScript>();
                if (monster != null)
                {
                    monster.SetPath(path);
                }
            }
            spawnTimer = SPAWN_RATE_S;
        }
    }

    public void SetPaths(List<List<GameObject>> paths)
    {
        this.paths = paths;
    }

    public void AddPath(List<GameObject> path)
    {
        paths.Add(path);
    }
}
