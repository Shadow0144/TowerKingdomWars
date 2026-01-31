using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Player.PlayerNumber playerNumber { get; set; }

    public Map map { get; set; }
    private List<PathTile> path;
    private int targetPathIndex = 0;
    private Vector3 currentTarget;
    private const float MOVEMENT_SPEED = 0.3f;

    protected int maxHealth = 10;
    protected int health;

    void Start()
    {
        health = maxHealth;
        map.AddMonsterToMap(this);
    }

    void Update()
    {
        if (path != null)
        {
            float movement = MOVEMENT_SPEED * Time.deltaTime;
            while (movement > 0 && targetPathIndex < path.Count)
            {
                float distance = Vector3.Distance(gameObject.transform.position, currentTarget);
                if (distance != 0.0f && distance > movement)
                {
                    Vector3 velocity = (currentTarget - gameObject.transform.position).normalized * movement;
                    gameObject.transform.position += velocity;
                    movement = 0;
                }
                else
                {
                    gameObject.transform.position = currentTarget;
                    movement -= distance;
                    targetPathIndex++;
                    if (targetPathIndex >= path.Count)
                    {
                        // Reached the end of the path, for now, just perish
                        map.RemoveMonsterFromMap(this);
                        Destroy(gameObject);
                        break;
                    }
                    else
                    {
                        currentTarget = new Vector3(path[targetPathIndex].transform.position.x, transform.position.y, path[targetPathIndex].transform.position.z);
                    }
                }
            }
        }
    }

    public void SetPath(List<PathTile> path)
    { 
        this.path = path;
        targetPathIndex = 0;
        currentTarget = gameObject.transform.position;
        if (path.Count > 1)
        {
            targetPathIndex++;
            currentTarget = new Vector3(path[targetPathIndex].transform.position.x, transform.position.y, path[targetPathIndex].transform.position.z);
        }
    }

    public void InflictDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            map.RemoveMonsterFromMap(this);
            Destroy(gameObject);
        }
    }
}
