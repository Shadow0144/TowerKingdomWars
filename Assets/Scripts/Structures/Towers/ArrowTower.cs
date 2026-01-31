using Unity.VisualScripting;
using UnityEngine;

public class ArrowTower : Tower
{
    public Arrow arrowPrefab;

    private void Update()
    {
        fireCooldownS -= Time.deltaTime;
        if (fireCooldownS <= 0.0f)
        {
            fireCooldownS = 0.0f; // Avoid the unlikely error of running the game for far too long and underflowing
            foreach (Monster monster in tile.map.monsters)
            {
                if (!monster.IsDestroyed() &&
                    monster.playerNumber != playerNumber &&
                    Vector3.Distance(monster.transform.position, transform.position) <= FiringRadius)
                {
                    ProjectileFactory.SpawnArrow(playerNumber, transform.position, (monster.transform.position - transform.position).normalized);
                    fireCooldownS = fireRateS;
                    break;
                }
            }
        }
    }
}
