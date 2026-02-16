using UnityEngine;

public class Flame : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        Monster monster = other.gameObject.GetComponent<Monster>();
        if (monster != null)
        {
            monster.InflictBuffDebuff(Monster.BuffDebuff.OnFire);
        }
    }
}