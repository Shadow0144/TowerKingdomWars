using UnityEngine;

public class Fireball : Projectile
{
    [SerializeField] private int damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        Monster monster = other.gameObject.GetComponent<Monster>();
        if (monster != null)
        {
            monster.InflictDamage(damage);
            ProjectileFactory.SpawnFlame(CurrentPlayerNumber, transform.position, Direction);
            Destroy(gameObject);
        }
        MapController map = other.gameObject.GetComponent<MapController>();
        if (map != null)
        {
            ProjectileFactory.SpawnFlame(CurrentPlayerNumber, transform.position, Direction);
            Destroy(gameObject);
        }
    }
}