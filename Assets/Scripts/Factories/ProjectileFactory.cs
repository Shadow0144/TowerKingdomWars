using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] private Arrow _arrowPrefab;
    [SerializeField] private Fireball _fireballPrefab;
    [SerializeField] private Flame _flamePrefab;
    [SerializeField] private FrostStorm _frostStormPrefab;

    private static ProjectileFactory _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static Arrow SpawnArrow(PlayerController.PlayerNumber playerNumber, Vector3 position, Vector3 direction)
    {
        if (_instance == null)
        {
            return null;
        }

        Arrow arrow = Instantiate(_instance._arrowPrefab, position, ((direction.magnitude != 0) ? Quaternion.LookRotation(direction) : Quaternion.identity) * Quaternion.AngleAxis(90, Vector3.right));
        arrow.Initialize(playerNumber, direction);
        arrow.transform.SetParent(GameSceneController.Instance.Map.Projectiles.transform);
        return arrow;
    }

    public static Fireball SpawnFireball(PlayerController.PlayerNumber playerNumber, Vector3 position, Vector3 direction)
    {
        if (_instance == null)
        {
            return null;
        }

        Fireball fireball = Instantiate(_instance._fireballPrefab, position, (direction.magnitude != 0) ? Quaternion.LookRotation(direction) : Quaternion.identity);
        fireball.Initialize(playerNumber, direction);
        fireball.transform.SetParent(GameSceneController.Instance.Map.Projectiles.transform);
        return fireball;
    }

    public static Flame SpawnFlame(PlayerController.PlayerNumber playerNumber, Vector3 position, Vector3 direction)
    {
        if (_instance == null)
        {
            return null;
        }

        Flame flame = Instantiate(_instance._flamePrefab, position, Quaternion.identity);
        flame.Initialize(playerNumber, direction);
        flame.transform.SetParent(GameSceneController.Instance.Map.Projectiles.transform);
        return flame;
    }

    public static FrostStorm SpawnFrostStorm(PlayerController.PlayerNumber playerNumber, Vector3 position)
    {
        if (_instance == null)
        {
            return null;
        }

        FrostStorm frostStorm = Instantiate(_instance._frostStormPrefab, position, Quaternion.identity);
        frostStorm.Initialize(playerNumber, Vector3.up);
        frostStorm.transform.SetParent(GameSceneController.Instance.Map.Projectiles.transform);
        return frostStorm;
    }
}
