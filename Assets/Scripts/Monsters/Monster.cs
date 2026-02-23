using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum BuffDebuff
    {
        OnFire,
        Chilled
    };
    protected Dictionary<BuffDebuff, bool> _buffsDebuffs = new Dictionary<BuffDebuff, bool>() {
        { BuffDebuff.OnFire, false },
        { BuffDebuff.Chilled, false }
    };

    public PlayerController.PlayerInfo OwningPlayerInfo { get; private set; }

    private List<PathTile> _path;
    private int _targetPathIndex = 0;
    private Vector3 _currentTarget;
    private const float _MOVEMENT_SPEED = 0.3f;

    protected float _maxHealth = 10.0f;
    protected float _health;

    protected float _burnTimeS = 0.0f;
    protected float _maxBurnTimeS = 10.0f;
    protected float _fireDamage = 3.0f; // Per second

    protected float _chillTimeS = 0.0f;
    protected float _maxChillTimeS = 10.0f;
    protected float _chillSpeedModifier = 0.75f;

    public virtual void Initialize(PlayerController.PlayerInfo playerInfo, List<PathTile> path)
    {
        OwningPlayerInfo = playerInfo;
        _path = path;

        _health = _maxHealth;

        // Setup path
        _targetPathIndex = 0;
        _currentTarget = gameObject.transform.position;
        if (_path.Count > 1)
        {
            _targetPathIndex++;
            _currentTarget = new Vector3(_path[_targetPathIndex].transform.position.x, transform.position.y, _path[_targetPathIndex].transform.position.z);
        }
    }

    private void Update()
    {
        if (_path != null)
        {
            float movement = _MOVEMENT_SPEED * Time.deltaTime;
            movement *= (_buffsDebuffs[BuffDebuff.Chilled]) ? _chillSpeedModifier : 1.0f; // Apply the chilled debuff if necessary
            while (movement > 0 && _targetPathIndex < _path.Count)
            {
                float distance = Vector3.Distance(gameObject.transform.position, _currentTarget);
                if (distance != 0.0f && distance > movement)
                {
                    Vector3 velocity = (_currentTarget - gameObject.transform.position).normalized * movement;
                    gameObject.transform.position += velocity;
                    movement = 0;
                }
                else
                {
                    gameObject.transform.position = _currentTarget;
                    movement -= distance;
                    _targetPathIndex++;
                    if (_targetPathIndex >= _path.Count)
                    {
                        // Reached the end of the path, for now, just perish
                        Destroy(gameObject);
                        break;
                    }
                    else
                    {
                        _currentTarget = new Vector3(_path[_targetPathIndex].transform.position.x, transform.position.y, _path[_targetPathIndex].transform.position.z);
                    }
                }
            }
        }

        if (_buffsDebuffs[BuffDebuff.OnFire])
        {
            InflictDamage(_fireDamage * Time.deltaTime);
            _burnTimeS -= Time.deltaTime;
            if (_burnTimeS <= 0.0f)
            {
                _buffsDebuffs[BuffDebuff.OnFire] = false;
                GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
            }
        }
        
        if (_buffsDebuffs[BuffDebuff.Chilled])
        {
            _chillTimeS -= Time.deltaTime;
            if (_chillTimeS <= 0.0f)
            {
                _buffsDebuffs[BuffDebuff.Chilled] = false;
                GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
            }
        }
    }

    public void InflictDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void InflictBuffDebuff(BuffDebuff buffDebuff)
    {
        _buffsDebuffs[buffDebuff] = true;
        switch (buffDebuff)
        {
            case BuffDebuff.OnFire:
                {
                    if (_buffsDebuffs[BuffDebuff.Chilled])
                    {
                        // Cancel out the chilled and on fire debuffs
                        _buffsDebuffs[BuffDebuff.OnFire] = false;
                        _buffsDebuffs[BuffDebuff.Chilled] = false;
                        _burnTimeS = 0;
                        _chillTimeS = 0;
                        GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        _burnTimeS = _maxBurnTimeS;
                        GetComponent<Renderer>().material.color = new Color(1.0f, 0.5f, 0.5f);

                    }
                }
                break;
            case BuffDebuff.Chilled:
                {
                    if (_buffsDebuffs[BuffDebuff.OnFire])
                    {
                        // Cancel out the on fire and chilled debuffs
                        _buffsDebuffs[BuffDebuff.OnFire] = false;
                        _buffsDebuffs[BuffDebuff.Chilled] = false;
                        _burnTimeS = 0;
                        _chillTimeS = 0;
                        GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        _chillTimeS = _maxChillTimeS;
                        GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 1.0f);
                    }
                }
                break;
        }
    }
}
