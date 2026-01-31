using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Player.PlayerNumber playerNumber { get; set; }

    public Vector3 direction { get; set; }
    public float speed { get; set; }

    protected float timeToLiveS { get; set; }

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        timeToLiveS -= Time.deltaTime;
        if (timeToLiveS < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
    }
}
