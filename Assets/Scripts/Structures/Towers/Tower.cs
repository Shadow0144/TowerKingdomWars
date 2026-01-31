using UnityEngine;

public class Tower : MonoBehaviour
{
    public Player.PlayerNumber playerNumber { get; set; }
    public Tile tile { get; set; }

    private GameObject firingRange;
    [SerializeField]
    private bool showFiringRange;

    [SerializeField, Min(0.0f)]
    private float firingRadius;
    public float FiringRadius { get => firingRadius;
        set
        {
            firingRadius = value;
            if (firingRange != null)
            {
                firingRange.transform.localScale = new Vector3(
                    (firingRadius * 2.0f) / transform.localScale.x,
                    firingRange.transform.localScale.y, 
                    (firingRadius * 2.0f) / transform.localScale.z);
            }
        }

    }

    [SerializeField, Min(0.0f)] protected float fireRateS;
    protected float fireCooldownS;

    private void Awake()
    {
        if (firingRange == null)
        {
            firingRange = transform.Find("FiringRange")?.gameObject;
        }
    }


    private void OnValidate()
    {
        FiringRadius = firingRadius;
        fireCooldownS = Mathf.Min(fireCooldownS, fireRateS);
        if (firingRange != null)
        {
            firingRange.SetActive(showFiringRange);
        }
    }
}
