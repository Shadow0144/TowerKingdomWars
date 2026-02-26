using UnityEngine;

namespace TowerKingdomWars
{
    public class Projectile : MonoBehaviour
    {
        public PlayerController.PlayerInfo OwningPlayerInfo { get; private set; }

        public Vector3 Direction { get; set; }

        [SerializeField] private float _speed;
        public float Speed { get { return _speed; } set { _speed = value; } }

        [SerializeField] protected float _timeToLiveS;

        public virtual void Initialize(PlayerController.PlayerInfo playerInfo, Vector3 direction)
        {
            OwningPlayerInfo = playerInfo;
            Direction = direction;
        }

        private void Update()
        {
            transform.position += Direction * Speed * Time.deltaTime;
            _timeToLiveS -= Time.deltaTime;
            if (_timeToLiveS < 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}