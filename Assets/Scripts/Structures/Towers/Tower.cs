using System;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public PlayerController.PlayerNumber CurrentPlayerNumber { get; set; }

    public Tile CurrentTile { get; private set; }

    [SerializeField] protected Vector3 firingPosition;

    [SerializeField] private GameObject _firingRange;
    [SerializeField] private bool _showFiringRange;

    [SerializeField, Min(0.0f)] private float _firingRadius;
    public float FiringRadius { get => _firingRadius;
        set
        {
            _firingRadius = value;
            if (_firingRange != null)
            {
                _firingRange.transform.localScale = new Vector3(
                    (_firingRadius * 2.0f) / transform.localScale.x,
                    _firingRange.transform.localScale.y, 
                    (_firingRadius * 2.0f) / transform.localScale.z);
            }
        }

    }

    [SerializeField, Min(0.0f)] protected float fireRateS;
    protected float fireCooldownS;

    [SerializeField, Min(0.0f)] private float _firingPositionYOffset = 0.5f;

    public virtual void Initialize(PlayerController.PlayerNumber playerNumber, Tile tile)
    {
        CurrentPlayerNumber = playerNumber;
        CurrentTile = tile;
    }

    private void Awake()
    {
        if (_firingRange == null)
        {
            _firingRange = transform.Find("FiringRange")?.gameObject;
        }
        firingPosition = transform.position + (Vector3.up * _firingPositionYOffset);
    }

    private void OnValidate()
    {
        FiringRadius = _firingRadius;
        fireCooldownS = Mathf.Min(fireCooldownS, fireRateS);
        if (_firingRange != null)
        {
            _firingRange.SetActive(_showFiringRange);
        }
        firingPosition = transform.position + (Vector3.up * _firingPositionYOffset);
    }
}
