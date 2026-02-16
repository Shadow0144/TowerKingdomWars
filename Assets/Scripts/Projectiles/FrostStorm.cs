using UnityEngine;

public class FrostStorm : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        Monster monster = other.gameObject.GetComponent<Monster>();
        if (monster != null)
        {
            monster.InflictBuffDebuff(Monster.BuffDebuff.Chilled);
        }
    }
}