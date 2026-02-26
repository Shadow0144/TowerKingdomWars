using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace TowerKingdomWars
{
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

        private Path _path;
        private int _targetPathIndex = 0;
        private Vector3 _currentTarget;
        private const float _MOVEMENT_SPEED = 0.3f;

        protected float _currentHealth;
        protected float _maxHealth = 10.0f;
        [SerializeField] protected HealthBar _healthBar;

        protected float _damage = 1.0f;
        protected float _attackTimer;
        protected float _maxAttackTimer = 10.0f;

        protected float _burnTimeS = 0.0f;
        protected float _maxBurnTimeS = 10.0f;
        protected float _fireDamage = 3.0f; // Per second

        protected float _chillTimeS = 0.0f;
        protected float _maxChillTimeS = 10.0f;
        protected float _chillSpeedModifier = 0.75f;

        protected bool _attacking = false;

        public virtual void Initialize(PlayerController.PlayerInfo playerInfo, Path path)
        {
            OwningPlayerInfo = playerInfo;
            _path = path;

            _currentHealth = _maxHealth;
            _healthBar.MaxHealth = _maxHealth;
            _healthBar.CurrentHealth = _currentHealth;

            _attackTimer = _maxAttackTimer;

            // Setup path
            _targetPathIndex = 0;
            _currentTarget = gameObject.transform.position;
            if (_path.pathTiles.Count > 1)
            {
                _targetPathIndex++; // Target the next tile since we begin on the first one
                _currentTarget = new Vector3(_path.pathTiles[_targetPathIndex].transform.position.x, transform.position.y, _path.pathTiles[_targetPathIndex].transform.position.z);
            }
        }

        private void Update()
        {
            if (_path != null)
            {
                // Movement
                if (!_attacking)
                {
                    float movement = _MOVEMENT_SPEED * Time.deltaTime;
                    movement *= (_buffsDebuffs[BuffDebuff.Chilled]) ? _chillSpeedModifier : 1.0f; // Apply the chilled debuff if necessary
                    while (movement > 0.0f)
                    {
                        float distance = Vector3.Distance(gameObject.transform.position, _currentTarget);
                        if (distance != 0.0f && distance > movement)
                        {
                            // Move full movement
                            Vector3 velocity = (_currentTarget - gameObject.transform.position).normalized * movement;
                            gameObject.transform.position += velocity;
                            movement = 0.0f;
                        }
                        else
                        {
                            // The movement remaining is higher than the distance to the next node, so arrive at the node, remove that distance, and continue forward
                            gameObject.transform.position = _currentTarget; // Move to the node
                            movement -= distance; // Remove that distance traveled
                            _targetPathIndex++; // Target the next node
                            if (_targetPathIndex < _path.pathTiles.Count)
                            {
                                _currentTarget = new Vector3(_path.pathTiles[_targetPathIndex].transform.position.x, transform.position.y, _path.pathTiles[_targetPathIndex].transform.position.z);
                            }
                            else
                            {
                                // Break here if we've reached the end of the path
                                _attacking = true;
                                break;
                            }
                        }
                    }
                }

                if (_attacking)
                {
                    // Reached the end of the path, attempt to destroy the structure
                    if (_path.targetStronghold.Alive)
                    {
                        _attackTimer -= Time.deltaTime;
                        if (_attackTimer < 0.0f)
                        {
                            _path.targetStronghold.TakeDamage(_damage);
                            _attackTimer = _maxAttackTimer;
                        }
                    }
                    else
                    {
                        // The path is empty, just perish
                        Destroy(gameObject);
                    }
                }
            }

            // Handle buffs / debuffs

            if (_buffsDebuffs[BuffDebuff.OnFire])
            {
                TakeDamage(_fireDamage * Time.deltaTime);
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

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            _healthBar.CurrentHealth = _currentHealth;
            if (_currentHealth <= 0.0f)
            {
                Destroy(gameObject);
            }
        }

        public void ReceiveBuffDebuff(BuffDebuff buffDebuff)
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
}