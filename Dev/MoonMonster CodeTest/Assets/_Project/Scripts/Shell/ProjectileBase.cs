using NaughtyAttributes;
using System.Linq;
using UnityEngine;

namespace MoonMonster.Codetest
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        [Header("Projectile settings")]
        [SerializeField] private LayerMask _tankMask;
        [SerializeField, Required] private AudioSource _explosionAudio;
        [SerializeField, Required] private ParticleSystem _explosionParticles;
        [SerializeField] protected float _maxLifeTime = 2f;
        [SerializeField] protected float _explosionRadius = 5f;
        [SerializeField] protected float _maxDamage = 100f;
        [SerializeField] protected float _explosionForce = 1000f;
        [SerializeField] private GameObject _sender;

        private bool _isAiProjectile;

        protected virtual void Start()
        {
            Destroy(this.gameObject, _maxLifeTime);
            if (_sender.TryGetComponent<AIController>(out AIController ai))
                _isAiProjectile = true;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _tankMask);           


            foreach(Collider collider in colliders)
            {
                if (collider.gameObject == _sender)
                    return;
                if (_isAiProjectile && collider.gameObject.TryGetComponent<AIController>(out AIController ai))
                    continue;
                if (!collider.TryGetComponent(out Rigidbody targetRigidbody))
                    continue;
                if (!targetRigidbody.TryGetComponent(out TankHealth targetHealth))
                    continue;

                targetRigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
                float damage = CalculateDamage(targetRigidbody.position);
                targetHealth.TakeDamage(damage);
            }

            //for (int i = 0; i < colliders.Length; i++)
            //{
            //    if (!colliders[i].TryGetComponent(out Rigidbody targetRigidbody))
            //        continue;

            //    targetRigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);

            //    if (!targetRigidbody.TryGetComponent(out TankHealth targetHealth))
            //        continue;

            //    float damage = CalculateDamage(targetRigidbody.position);
            //    targetHealth.TakeDamage(damage);
            //}

            _explosionParticles.transform.parent = null;
            _explosionParticles.Play();
            _explosionAudio.Play();

            ParticleSystem.MainModule mainModule = _explosionParticles.main;
            Destroy(_explosionParticles.gameObject, mainModule.duration);

            Destroy(gameObject);
        }

        protected virtual float CalculateDamage(Vector3 targetPosition)
        {
            Vector3 explosionToTarget = targetPosition - transform.position;
            float explosionDistance = explosionToTarget.magnitude;
            float relativeDistance = (_explosionRadius - explosionDistance) / _explosionRadius;
            float damage = relativeDistance * _maxDamage;
            damage = Mathf.Max(0f, damage);

            return damage;
        }

        public void SetSender(GameObject sender)
        {
            _sender = sender;
        }
    }
}
