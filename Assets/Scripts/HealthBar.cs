using UnityEngine;
using UnityEngine.UI;

namespace TowerKingdomWars
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _healthBarFill;

        [SerializeField, Min(0.0f)] private float _currentHealth = 0.0f;
        [SerializeField, Min(1.0f)] private float _maxHealth = 1.0f;

        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                UpdateFill();
            }
        }

        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                UpdateFill();
            }
        }

        private void Update()
        {
            if (GameSceneController.Instance?.MainCamera?.transform != null)
            {
                transform.LookAt(GameSceneController.Instance.MainCamera.transform, Vector3.up);
            }
        }

        private void OnValidate()
        {
            UpdateFill();
        }

        private void UpdateFill()
        {
            if (_healthBarFill != null && MaxHealth > 0.0f)
            {
                _healthBarFill.fillAmount =
                    Mathf.Clamp01(CurrentHealth / MaxHealth);
            }
        }
    }
}