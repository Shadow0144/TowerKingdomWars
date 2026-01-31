using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private int damage = 5;

    private void Start()
    {
        speed = 10;
        timeToLiveS = 3;
    }

    void OnTriggerEnter(Collider other)
    {
        Monster monster = other.gameObject.GetComponent<Monster>();
        if (monster != null)
        {
            monster.InflictDamage(damage);
            Destroy(gameObject);
        }
    }
}