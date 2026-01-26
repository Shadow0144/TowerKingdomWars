using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    private List<GameObject> path;
    private int targetPathIndex = 0;
    private Vector3 currentTarget;
    private const float MOVEMENT_SPEED = 0.3f;

    void Start()
    {
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

    public void SetPath(List<GameObject> path)
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
}
