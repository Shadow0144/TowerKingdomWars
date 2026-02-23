using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlameTower : Tower
{
    private static List<Vector2Int> _footprint = new List<Vector2Int>() 
    { 
        new Vector2Int(+0, +0)
    };
    public new static List<Vector2Int> Footprint => _footprint;
    public override List<Vector2Int> GetFootprint()
    {
        return _footprint;
    }

    private void Update()
    {
        if (OwningPlayerInfo.playerNumber == 0 || IsGhost)
        {
            return;
        }

        fireCooldownS -= Time.deltaTime;
        if (fireCooldownS <= 0.0f)
        {
            fireCooldownS = 0.0f; // Avoid the unlikely error of running the game for far too long and underflowing
            foreach (GameObject monsterGameObject in GameObject.FindGameObjectsWithTag("Monster"))
            {
                Monster monster = monsterGameObject.GetComponent<Monster>();
                if (monster != null
                    && !monster.IsDestroyed()
                    && monster.OwningPlayerInfo.teamNumber != OwningPlayerInfo.teamNumber
                    && Vector3.Distance(monster.transform.position, firingPosition) <= FiringRadius)
                {
                    ProjectileFactory.SpawnFireball(OwningPlayerInfo, firingPosition, (monster.transform.position - firingPosition).normalized);
                    fireCooldownS = fireRateS;
                    break;
                }
            }
        }
    }
}
