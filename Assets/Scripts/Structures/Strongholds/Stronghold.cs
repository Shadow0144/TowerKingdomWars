using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerKingdomWars
{
    public abstract class Stronghold : Structure
    {
        public bool Alive = true;

        public List<Path> Paths { get; private set; } = new List<Path>();

        private const float _SPAWN_RATE_S = 3.0f;
        private float _spawnTimer;

        protected float _currentHealth;
        protected float _maxHealth = 100.0f;
        [SerializeField] protected HealthBar _healthBar;

        public override void Initialize(uint playerSlot, List<Tile> tiles)
        {
            base.Initialize(playerSlot, tiles);

            _currentHealth = _maxHealth;
            _healthBar.MaxHealth = _maxHealth;
            _healthBar.CurrentHealth = _currentHealth;

            _spawnTimer = _SPAWN_RATE_S;
        }

        private void Update()
        {
            if (OwningPlayerInfo.playerNumber == 0 || IsGhost || !Alive)
            {
                return;
            }

            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer < 0)
            {
                foreach (Path path in Paths)
                {
                    if (path.targetStronghold.Alive)
                    {
                        MonsterFactory.SpawnGoblin(transform.position, OwningPlayerInfo, path);
                    }
                }
                _spawnTimer = _SPAWN_RATE_S;
            }
        }

        public void AddPath(Path path)
        {
            Paths.Add(path);
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            _healthBar.CurrentHealth = _currentHealth;
            if (_currentHealth <= 0.0f)
            {
                Alive = false;
            }
        }
    }
}