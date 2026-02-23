using UnityEngine;

public class Fireball : Projectile
{
    [SerializeField] private int damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        Monster monster = other.gameObject.GetComponent<Monster>();
        Tile tile = other.gameObject.GetComponent<Tile>();
        // Collided with a monster?
        if (monster != null
            && monster.OwningPlayerInfo.teamNumber != OwningPlayerInfo.teamNumber)
        {
            monster.InflictDamage(damage);
            ProjectileFactory.SpawnFlame(OwningPlayerInfo, transform.position, Direction);
            Destroy(gameObject);
        }
        // Collided with a tile?
        else if (tile != null)
        {
            ProjectileFactory.SpawnFlame(OwningPlayerInfo, transform.position, Direction);
            Destroy(gameObject);
        }
    }
}