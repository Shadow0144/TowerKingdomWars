using Unity.VisualScripting;
using UnityEngine;

public class FlameTower : Tower
{
    private void Update()
    {
        fireCooldownS -= Time.deltaTime;
        if (fireCooldownS <= 0.0f)
        {
            fireCooldownS = 0.0f; // Avoid the unlikely error of running the game for far too long and underflowing
            foreach (Monster monster in GameSceneController.Instance.Map.MonsterList)
            {
                if (!monster.IsDestroyed() &&
                    monster.CurrentPlayerNumber != CurrentPlayerNumber &&
                    Vector3.Distance(monster.transform.position, firingPosition) <= FiringRadius)
                {
                    ProjectileFactory.SpawnFireball(CurrentPlayerNumber, firingPosition, (monster.transform.position - firingPosition).normalized);
                    fireCooldownS = fireRateS;
                    break;
                }
            }
        }
    }
}
