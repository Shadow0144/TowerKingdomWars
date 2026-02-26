using UnityEngine;

namespace TowerKingdomWars
{
    public class Arrow : Projectile
    {
        [SerializeField] private int _damage = 5;

        private void OnTriggerEnter(Collider other)
        {
            Monster monster = other.gameObject.GetComponent<Monster>();
            if (monster != null
                && monster.OwningPlayerInfo.teamNumber != OwningPlayerInfo.teamNumber)
            {
                monster.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}