using UnityEngine;

namespace TowerKingdomWars
{
    public class FrostStorm : Projectile
    {
        private void OnTriggerEnter(Collider other)
        {
            Monster monster = other.gameObject.GetComponent<Monster>();
            if (monster != null)
            {
                monster.ReceiveBuffDebuff(Monster.BuffDebuff.Chilled);
            }
        }
    }
}