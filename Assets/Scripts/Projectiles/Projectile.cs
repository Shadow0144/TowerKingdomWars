using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Player.PlayerNumber playerNumber { get; private set; }

    public Vector3 direction { get; set; }
    public float speed { get; set; }

    protected float timeToLiveS { get; set; }

    public void Initialize(Player.PlayerNumber playerNumber, Vector3 direction)
    {
        this.playerNumber = playerNumber;
        this.direction = direction;
    }

    internal void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        timeToLiveS -= Time.deltaTime;
        if (timeToLiveS < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
