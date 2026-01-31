using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] private Arrow arrowPrefab;

    private static ProjectileFactory Instance;

    void Awake()
    {
        Instance = this;
    }

    public static Arrow SpawnArrow(Player.PlayerNumber playerNumber, Vector3 position, Vector3 direction)
    {
        if (Instance == null)
        {
            return null;
        }

        Arrow arrow = Instantiate(Instance.arrowPrefab, position, (direction.magnitude != 0) ? Quaternion.LookRotation(direction) : Quaternion.identity);
        arrow.playerNumber = playerNumber;
        arrow.direction = direction;
        return arrow;
    }
}
