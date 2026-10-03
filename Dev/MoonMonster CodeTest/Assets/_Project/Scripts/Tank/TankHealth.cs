using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace MoonMonster.Codetest
{
    public class TankHealth : MonoBehaviour
    {
        [SerializeField] private float _startingHealth = 100f;
        [SerializeField, Required] private Slider _slider;
        [SerializeField, Required] private Image _fillImage;
        [SerializeField] private Color _fullHealthColor = Color.green;
        [SerializeField] private Color _zeroHealthColor = Color.red;
        [SerializeField, Required] private GameObject _explosionPrefab;

        [Header("Pickup drop settings")]
        [SerializeField] private GameObject _wrenchPrefab;
        [SerializeField, Range(0, 1)] private float _dropChance;

        private AudioSource _explosionAudio;
        private ParticleSystem _explosionParticles;

        private float _currentHealth;
        private float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                if (_currentHealth > _startingHealth)
                    _currentHealth = _startingHealth;
                if (_currentHealth <= 0f && !_dead)
                    OnDeath();

                SetHealthUI();
            }
        }

        private bool _dead;

        private void Awake()
        {
            _explosionParticles = Instantiate(_explosionPrefab).GetComponent<ParticleSystem>();
            _explosionAudio = _explosionParticles.GetComponent<AudioSource>();
            _explosionParticles.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            CurrentHealth = _startingHealth;
            _dead = false;

            SetHealthUI();
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
        }

        public void RestoreHealth(float amount)
        {
            CurrentHealth += amount;
        }

        private void SetHealthUI()
        {
            _slider.value = CurrentHealth;

            _fillImage.color = Color.Lerp(_zeroHealthColor, _fullHealthColor, CurrentHealth / _startingHealth);
        }

        private void OnDeath()
        {
            _dead = true;

            _explosionParticles.transform.position = transform.position;
            _explosionParticles.gameObject.SetActive(true);

            _explosionParticles.Play();

            _explosionAudio.Play();
            SpawnWrenchPickup();

            gameObject.SetActive(false);
        }

        private void SpawnWrenchPickup()
        {
            float random = Random.Range(0f, 1f);
            if (random <= _dropChance)
                Instantiate(_wrenchPrefab, new(transform.position.x, 1.55f, transform.position.z), Quaternion.identity);
        }
    }
}